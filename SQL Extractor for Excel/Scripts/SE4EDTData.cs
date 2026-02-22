using MessagePack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQL_Extractor_for_Excel.Scripts
{
    [MessagePackObject]
    public class SE4EDTData
    {
        [Key(0)]
        public string DatabaseName { get; set; }

        [Key(1)]
        public string SqlQuery { get; set; }

        // Store schema separately to recreate DataTable columns correctly
        [Key(2)]
        public List<string> ColumnNames { get; set; }

        [Key(3)]
        public List<string> ColumnTypes { get; set; }

        // Typeless serialization is handled by the resolver in the service
        [Key(4)]
        public List<object[]> Rows { get; set; }

        public SE4EDTData() { }

        public SE4EDTData(string databaseName, string sqlQuery, DataTable table)
        {
            DatabaseName = databaseName;
            SqlQuery = sqlQuery;

            if (table != null)
            {
                ColumnNames = new List<string>();
                ColumnTypes = new List<string>();

                foreach (DataColumn col in table.Columns)
                {
                    ColumnNames.Add(col.ColumnName);
                    // Save the AssemblyQualifiedName to ensure exact type matching on restore
                    ColumnTypes.Add(col.DataType.AssemblyQualifiedName);
                }

                Rows = new List<object[]>(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    // Convert DBNull to null for cleaner serialization
                    object[] itemArray = row.ItemArray;
                    for (int i = 0; i < itemArray.Length; i++)
                    {
                        if (itemArray[i] == DBNull.Value) itemArray[i] = null;
                    }
                    Rows.Add(itemArray);
                }
            }
        }

        public DataTable ToDataTable()
        {
            var table = new DataTable(DatabaseName ?? "Export");

            // 1. Restore Schema
            if (ColumnNames != null && ColumnTypes != null)
            {
                for (int i = 0; i < ColumnNames.Count; i++)
                {
                    Type colType = Type.GetType(ColumnTypes[i]) ?? typeof(string);
                    table.Columns.Add(ColumnNames[i], colType);
                }
            }

            // 2. Restore Data
            if (Rows != null)
            {
                foreach (var rowData in Rows)
                {
                    DataRow row = table.NewRow();
                    for (int i = 0; i < rowData.Length; i++)
                    {
                        object val = rowData[i];
                        if (val == null)
                        {
                            row[i] = DBNull.Value;
                        }
                        else
                        {
                            // Ensure the value matches the column type (e.g. deserialize might give Int32 for a Byte column)
                            try
                            {
                                row[i] = Convert.ChangeType(val, table.Columns[i].DataType);
                            }
                            catch
                            {
                                // Fallback: try direct assignment if conversion fails
                                row[i] = val;
                            }
                        }
                    }
                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}