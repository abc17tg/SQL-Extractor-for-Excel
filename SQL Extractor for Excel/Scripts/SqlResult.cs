using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class SqlResult
    {
        public DataTable DataTable;
        public SqlElement SqlElement;
        public string Errors;
        public bool HasErrors => !string.IsNullOrEmpty(Errors) || DataTable == null;
        public bool Cancelled = false;
        
        public SqlResult(DataTable dataTable, string errors, SqlElement sqlElement) 
        { 
            DataTable = dataTable;
            SqlElement = sqlElement;
            Errors = errors;
            Cancelled = sqlElement?.Cancelled ?? false;
        }

        public static SqlResult MergeResults(List<SqlResult> results)
        {
            if (results == null)
                return null;

            if (results.Count == 1 || results.All(p=>p.DataTable.Rows.Count < 1))
                return results[0];

            results = results.Where(p=>p.DataTable.Rows.Count>0).ToList();
            
            if (results.Count == 1)
                return results[0];

            if (results.Count > 1)
            {
                SqlResult result = results[0];
                for(int i = 1; i < results.Count; i++)
                {
                    result.DataTable.Merge(results[i].DataTable);
                    result.Errors += $"\n\n{results[i].Errors}";
                }
                return result;
            }

            return null;
        }
    }
}
