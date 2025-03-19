using System;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class SqlElement
    {
        public string Name;
        public DateTime? m_startTime;
        public bool Cancelled = false;
        private SqlServerManager.ServerType m_serverType;
        private string m_dbName;
        private object m_cmd;

        public dynamic Cmd => m_serverType == SqlServerManager.ServerType.SqlServer ? (SqlCommand)m_cmd : (m_serverType == SqlServerManager.ServerType.Oracle ? (OracleCommand)m_cmd : m_cmd);
        public SqlServerManager.ServerType ServerType => m_serverType;
        public string DbName => m_dbName;

        /*        public SqlElement(object cmd, SqlServerManager.ServerType serverType, string name = "query", DateTime? startTime = null)
                {
                    m_cmd = cmd;
                    m_serverType = serverType;
                    Name = name;
                    m_startTime = startTime ?? DateTime.Now;
                }*/

        public SqlElement(object cmd, SqlServerManager.ServerType serverType, string dbName, string name = "query", DateTime? startTime = null)
        {
            m_cmd = cmd;
            m_serverType = serverType;
            m_dbName = dbName;
            Name = name;
            m_startTime = startTime ?? DateTime.Now;
        }

        public static string FormatQueryDetailsMessage(SqlElement sqlElement)
        {
            string message;
            if (sqlElement != null)
                message = $@"
    Server Type: {sqlElement.ServerType}
    Database: 
        {sqlElement.Name}
    Elapsed Time: {Math.Floor((DateTime.Now.Subtract((DateTime)sqlElement.m_startTime).TotalMinutes))} minutes

    --------------------------------------

    Query:

    {((string)sqlElement.Cmd.CommandText).RemoveLeadingTabsMultiline()}";
            else
                message = $@"
    Server Type: -
    Database: -
    Elapsed Time: -
    --------------------------------------
    Query: -";

            return message;
        }

        public bool TryToCancelQuery()
        {
            try
            {
                if (Cmd == null)
                    return false;

                switch (m_serverType)
                {
                    case SqlServerManager.ServerType.SqlServer:
                        Cmd.Cancel();
                        break;
                    case SqlServerManager.ServerType.Oracle:
                        Cmd.Cancel();
                        break;
                }
                Cancelled = true;
                return true;
            }
            catch (SqlException) { return false; }
            catch (OracleException) { return false; }
            catch (Exception) { return false; }
        }
    }
}
