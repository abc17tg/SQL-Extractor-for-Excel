using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Globalization;
using System.Text;

namespace SQL_Extractor_for_Excel.Scripts
{
    internal class SqlFilter
    {
        public static string GenerateSqlFilterFromExcelSelection(Excel.Range dataRange)
        {
            // Get values as 2D array for faster processing
            object[,] values = (object[,])dataRange.Value2;
            int totalRows = values.GetLength(0);
            int totalColumns = values.GetLength(1);

            var columns = new List<ColumnFilter>();

            // Process each column starting from first row as headers
            for (int col = 1; col <= totalColumns; col++)
            {
                // Get column header
                var header = values[1, col]?.ToString();
                if (string.IsNullOrEmpty(header)) continue;

                var uniqueValues = new HashSet<string>();
                bool hasEmpty = false;

                // Process data rows
                for (int row = 2; row <= totalRows; row++)
                {
                    var value = values[row, col];

                    if (value == null || value.ToString() == "")
                    {
                        hasEmpty = true;
                    }
                    else
                    {
                        // Handle numeric values without quotes
                        if (IsNumeric(value))
                        {
                            uniqueValues.Add(Convert.ToDouble(value)
                                .ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            uniqueValues.Add($"'{value.ToString().Replace("'", "''")}'");
                        }
                    }
                }

                // Build conditions for this column
                var conditions = new List<string>();

                // Add IN clause if we have values
                if (uniqueValues.Count > 0)
                {
                    conditions.Add($"{header} IN ({string.Join(",", uniqueValues)})");
                }

                // Add NULL/empty condition if needed
                if (hasEmpty)
                {
                    conditions.Add($"({header} IS NULL OR {header} = '')");
                }

                if (conditions.Count > 0)
                {
                    columns.Add(new ColumnFilter
                    {
                        Name = header,
                        DistinctCount = uniqueValues.Count + (hasEmpty ? 1 : 0),
                        Conditions = conditions
                    });
                }
            }

            // Order columns by number of distinct elements (ascending)
            var orderedColumns = columns.OrderBy(c => c.DistinctCount).ToList();
            if (orderedColumns.Count == 0) return string.Empty;

            // Build WHERE clause with proper formatting
            var sb = new StringBuilder();
            sb.AppendLine("WHERE");

            for (int i = 0; i < orderedColumns.Count; i++)
            {
                if (i > 0)
                {
                    sb.AppendLine("AND");
                }

                sb.AppendLine("(");
                sb.AppendLine(string.Join(Environment.NewLine + "OR" + Environment.NewLine, orderedColumns[i].Conditions));
                sb.AppendLine(")");
            }

            return sb.ToString();
        }

        private static bool IsNumeric(object value)
        {
            if (value == null) return false;
            return value is double || value is int || value is decimal || value is float;
        }

        private class ColumnFilter
        {
            public string Name { get; set; }
            public int DistinctCount { get; set; }
            public List<string> Conditions { get; set; }
        }

    }
}
