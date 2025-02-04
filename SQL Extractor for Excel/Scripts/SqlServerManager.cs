using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;
using Oracle.ManagedDataAccess.Client;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class SqlServerManager
    {
        public List<SqlElement> SqlElements = new List<SqlElement>();

        // Define events for command completion
        public event Action CommandFinished;
        public static readonly string LookupString = "#TkL@.qKs1Hm8hJ-[nxB";

        private event Action<SqlCommand> SqlServerCommandFinished;
        private event Action<OracleCommand> OracleCommandFinished;
        private event Action<dynamic> CommandCancelled;

        public SqlServerManager()
        {
            SqlServerCommandFinished += OnCommandFinished;
            OracleCommandFinished += OnCommandFinished;
            CommandCancelled += OnCommandFinished;
        }

        private void OnCommandFinished(dynamic cmd)
        {
            try
            {
                SqlElement sqlElement = SqlElements.FirstOrDefault(p => p.Cmd == cmd);
                if (sqlElement != null)
                    SqlElements.Remove(sqlElement);
                else
                    SqlElements.RemoveAll(p => p.Cmd == null);
            }
            catch (Exception)
            {
                SqlElements.RemoveAll(p => p.Cmd == null);
            }
            CommandFinished.Invoke();
        }

        public void CancelCmd(dynamic cmd)
        {
            CommandCancelled.Invoke(cmd);
        }

        public static bool AddSqlConnection()
        {
            Form.ActiveForm.TopMost = false;
            ServerConnectionForm serverConnectionForm = new ServerConnectionForm();
            //serverConnectionForm.TopMost = true;
            var result = serverConnectionForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                Form.ActiveForm.TopMost = true;
                return true;
            }
            else
            {
                Form.ActiveForm.TopMost = true;
                return false;
            }
        }

        public static bool TestConnectionOracle(string connectionString)
        {
            bool result;
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    result = true;
                }
            }
            catch (OracleException ex)
            {
                result = false;
                MessageBox.Show(ex.Message.ToString());
            }
            return result;
        }

        public static bool TestConnectionSqlServer(string connectionString)
        {
            bool result;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    result = true;
                }
            }
            catch (SqlException ex)
            {
                result = false;
                MessageBox.Show(ex.Message.ToString());
            }
            return result;
        }

        public void CreateWbConnectionAndRunBackgroundQuery(Excel.Range destinationRange, string sqlQuery, string dbName)
        {
            // Get the Excel application and active workbook.
            Excel.Application app = Globals.ThisAddIn.Application;
            Excel.Workbook workbook = app.ActiveWorkbook;

            // Define a name for the query.
            string queryName = "Query1";

            // Build the M (Power Query) formula.
            string mFormula = "let\r\n" +
                              $"    Source = Oracle.Database(\"{dbName}\", " +
                              "[HierarchicalNavigation=true, " +
                              $"Query=\"{sqlQuery}\", " +
                              "CreateNavigationProperties=false])\r\n" +
                              "in\r\n" +
                              "    Source";

            // Use dynamic to access the Queries collection.
            // (Note: The Queries property is not part of the primary interop assembly.)
            dynamic wbDynamic = workbook;
            dynamic queries = null;
            try
            {
                // Attempt to get the Queries property using dynamic.
                queries = wbDynamic.Queries;
            }
            catch (RuntimeBinderException ex)
            {
                throw new Exception("Could not access the Queries collection. Ensure that your Excel version supports Power Query.", ex);
            }

            // Remove any existing query with the same name (optional).
            try
            {
                // Loop through the queries (using dynamic enumeration)
                foreach (dynamic query in queries)
                {
                    if (query.Name == queryName)
                    {
                        query.Delete();
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // Ignore if query doesn't exist.
            }

            // Add the new query.
            try
            {
                queries.Add(queryName, mFormula);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding the query.", ex);
            }

            // Use the provided destinationRange or add a new worksheet if needed.
            Excel.Worksheet ws;
            Excel.Range dest;
            if (destinationRange != null)
            {
                dest = destinationRange;
                ws = destinationRange.Worksheet;
            }
            else
            {
                ws = workbook.Worksheets.Add();
                dest = ws.Range["A1"];
            }

            // Create the connection string for the query using the Microsoft Mashup OLE DB provider.
            string connectionString = $"OLEDB;Provider=Microsoft.Mashup.OleDb.1;Data Source=$Workbook$;Location={queryName};Extended Properties=\"\"";

            // Add a ListObject (table) with a QueryTable based on the query.
            Excel.ListObject listObj = ws.ListObjects.Add(
                SourceType: Excel.XlListObjectSourceType.xlSrcQuery,
                Source: connectionString,
                Destination: dest,
                XlListObjectHasHeaders: Excel.XlYesNoGuess.xlYes);

            // Configure the QueryTable.
            Excel.QueryTable queryTable = listObj.QueryTable;
            queryTable.CommandType = Excel.XlCmdType.xlCmdSql;
            queryTable.CommandText = new object[] { $"SELECT * FROM [{queryName}]" };
            queryTable.RowNumbers = false;
            queryTable.FillAdjacentFormulas = false;
            queryTable.PreserveFormatting = true;
            queryTable.RefreshOnFileOpen = false;
            queryTable.BackgroundQuery = true;
            queryTable.RefreshStyle = Excel.XlCellInsertionMode.xlInsertDeleteCells;
            queryTable.SavePassword = false;
            queryTable.SaveData = true;
            queryTable.AdjustColumnWidth = true;
            queryTable.RefreshPeriod = 0;
            queryTable.PreserveColumnInfo = true;
            listObj.Name = queryName;

            // Refresh the query table in the background.
            queryTable.Refresh(true);
        }

        public static (SqlResult, bool OperationSuccessfullyCompleted) GetDataFromServerToExcelRange(SqlServerManager manager, string query, SqlConn sqlConn, Excel.Range rng, bool headers = true, int timeout = -1)
        {
            SqlResult sqlResult = GetDataFromServer(manager, query, sqlConn, timeout);
            if (sqlResult.HasErrors || sqlResult.DataTable.Rows.Count < 1)
                return (sqlResult, true);

            if (!rng.Valid() || (sqlResult.DataTable.Rows.Count >= rng.Worksheet.Rows.Count - rng.Row + 1))
                return (sqlResult, false);

            UtilsExcel.PasteDataTableToRange(sqlResult.DataTable, rng, headers);
            UtilsExcel.Updating(rng.Application, true);
            return (sqlResult, true);
        }

        public static string CheckOracleSqlSyntax(string query, SqlConn sqlConn)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(sqlConn.ConnectionString()))
                {
                    con.Open();

                    // Check the syntax of the query
                    string syntaxCheckQuery = $"BEGIN DBMS_SQL.PARSE(DBMS_SQL.OPEN_CURSOR(), '{query.Replace("'", "''")}', DBMS_SQL.NATIVE); END;";

                    using (OracleCommand syntaxCheckCmd = new OracleCommand(syntaxCheckQuery, con))
                    {
                        syntaxCheckCmd.ExecuteNonQuery();
                    }

                    return string.Empty;
                }
            }
            catch (OracleException ex)
            {
                return "Syntax error: " + ex.Message;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string CheckSqlServerQuerySyntax(string query, SqlConn sqlConn)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConn.ConnectionString()))
                {
                    con.Open();

                    // Check the syntax of the query
                    string syntaxCheckQuery = $"SET PARSEONLY ON; {query}; SET PARSEONLY OFF;";

                    using (SqlCommand syntaxCheckCmd = new SqlCommand(syntaxCheckQuery, con))
                    {
                        syntaxCheckCmd.CommandTimeout = 2;
                        syntaxCheckCmd.ExecuteNonQuery();
                    }

                    return string.Empty;
                }
            }
            catch (SqlException ex)
            {
                // query to check tries to pull data when correct so timeout is treated as error free
                if (ex.Message == "Execution Timeout Expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.\r\nOperation cancelled by user.")
                    return string.Empty;

                if (ex.Errors.Count > 0)
                {
                    return $"Syntax error:\n{string.Join(Environment.NewLine, ex.Errors.Cast<SqlError>().Select(p => $"Line {p.LineNumber}\tError: {p.Message}"))}";
                }
                return $"Syntax error: {ex.Message}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string CheckSqlQuerySyntaxOnline(string query, SqlConn sqlConn)
        {
            string err = null;
            switch (sqlConn.Type)
            {
                case ServerType.SqlServer:
                    err = CheckSqlServerQuerySyntax(query, sqlConn);
                    break;
                case ServerType.Oracle:
                    err = CheckOracleSqlSyntax(query, sqlConn);
                    break;
                case ServerType.Excel:
                    break;
            }
            return err;
        }

        public static bool CheckSqlQueriesSyntaxOnline(List<string> queries, SqlConn sqlConn)
        {
            if(queries.All(p=>CheckSqlQuerySyntaxOnline(p, sqlConn) == string.Empty))
                return true;
            else
                return false;
        }

        public static SqlResult GetDataFromServer(SqlServerManager manager, string query, SqlConn sqlConn, int timeout = -1)
        {
            DataTable dt = new DataTable();
            SqlResult sqlResult = null;
            switch (sqlConn.Type)
            {
                case ServerType.SqlServer:
                    sqlResult = GetDataFromSqlServer(manager, query, sqlConn, timeout);
                    break;
                case ServerType.Oracle:
                    sqlResult = GetDataFromOracleSqlServer(manager, query, sqlConn, timeout);
                    break;
                case ServerType.Excel:
                    sqlResult = GetDataFromExcelSqlTables(query);
                    break;
            }
            return sqlResult;
        }

        public static SqlResult GetDataFromExcelSqlTables(string query)
        {
            return null;
/*            try
            {
                object rs = UtilsExcel.RunMacro("SqlQueries.ExecuteSQLQuery", new object[] { query });

                OleDbDataAdapter adapter = new OleDbDataAdapter();
                DataTable dt = new DataTable();
                adapter.Fill(dt, rs);

                return new SqlResult(dt, null);
            }
            catch (Exception ex)
            {
                return new SqlResult(null, ex.Message);
            }*/
        }

        public static SqlResult GetDataFromOracleSqlServer(SqlServerManager manager, string query, SqlConn sqlConn, int timeout = -1)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(sqlConn.ConnectionString()))
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.CommandTimeout = timeout >= 0 ? timeout : cmd.CommandTimeout;
                    SqlElement sqlElement = new SqlElement(cmd, sqlConn.Type, sqlConn.Name, string.IsNullOrEmpty(con.Database) ? con.Database : "Oracle query");
                    manager.SqlElements.Add(sqlElement);
                    try
                    {
                        using (OracleDataReader rdr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            rdr.SuppressGetDecimalInvalidCastException = true;
                            dt.Load(rdr);
                            manager.OracleCommandFinished?.Invoke(cmd);
                            return new SqlResult(dt, null, sqlElement);
                        }
                    }
                    catch (OracleException ex)
                    {
                        manager.OracleCommandFinished?.Invoke(cmd);
                        return new SqlResult(null, ex.Message, sqlElement);
                    }
                }
            }
            catch (OracleException ex)
            {
                manager.OracleCommandFinished?.Invoke(null);
                return new SqlResult(null, ex.Message, null);
            }
        }

        public static SqlResult GetDataFromSqlServer(SqlServerManager manager, string query, SqlConn sqlConn, int timeout = -1)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConn.ConnectionString()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = timeout >= 0 ? timeout : cmd.CommandTimeout;
                    SqlElement sqlElement = new SqlElement(cmd, sqlConn.Type, sqlConn.Name, string.IsNullOrEmpty(con.Database) ? con.Database : "MS Sql query");
                    manager.SqlElements.Add(sqlElement);
                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(rdr);
                            manager.SqlServerCommandFinished?.Invoke(cmd);
                            return new SqlResult(dt, null, sqlElement);
                        }
                    }
                    catch (SqlException ex)
                    {
                        manager.SqlServerCommandFinished?.Invoke(cmd);
                        return new SqlResult(null, ex.Message, sqlElement);
                    }
                }
            }
            catch (SqlException ex)
            {
                manager.SqlServerCommandFinished?.Invoke(null);
                return new SqlResult(null, ex.Message, null);
            }
        }

        public enum ServerType
        {
            SqlServer = 0,
            Oracle = 1,
            Excel = 2
        }
    }
}
