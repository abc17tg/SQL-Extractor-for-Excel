using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_Extractor_for_Excel.Scripts
{
    public static class FileExport
    {
        public enum ExportFormat
        {
            TXT,
            SE4EDT,
            // CSV,
            // JSON,
            // XML,
        }

        public class ExportFormatInfo
        {
            public string Extension { get; set; }
            public string FilterDescription { get; set; }
            public int FilterIndex { get; set; }
        }

        public static Dictionary<ExportFormat, ExportFormatInfo> GetExportFormats()
        {
            return new Dictionary<ExportFormat, ExportFormatInfo>
            {
                { ExportFormat.TXT, new ExportFormatInfo { Extension = ".txt", FilterDescription = "Text Files (*.txt)", FilterIndex = 1 } },
                { ExportFormat.SE4EDT, new ExportFormatInfo { Extension = ".se4edt", FilterDescription = "SE4EDT Files (*.se4edt)", FilterIndex = 2 } },
                // { ExportFormat.CSV, new ExportFormatInfo { Extension = ".csv", FilterDescription = "CSV Files (*.csv)", FilterIndex = 3 } },
                // { ExportFormat.JSON, new ExportFormatInfo { Extension = ".json", FilterDescription = "JSON Files (*.json)", FilterIndex = 4 } },
            };
        }

        private static string BuildFilterString()
        {
            var formats = GetExportFormats();
            return string.Join("|", formats.Values.Select(f => $"{f.FilterDescription}|*{f.Extension}"));
        }

        public static ExportFormat GetFormatFromExtension(string extension)
        {
            var formats = GetExportFormats();
            extension = extension.ToLower();

            foreach (var kvp in formats)
            {
                if (kvp.Value.Extension == extension)
                    return kvp.Key;
            }

            return ExportFormat.TXT;
        }

        public static async Task SaveDataWithFormatChoice(string fileName, string dbName, string query, DataTable table, string initialPath = null, ExportFormat defaultFormat = ExportFormat.TXT)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            if (initialPath != null && !Directory.Exists(initialPath))
                initialPath = null;

            var formats = GetExportFormats();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = fileName;
                sfd.InitialDirectory = initialPath ?? FileManager.DownloadsPath;
                sfd.Filter = BuildFilterString();
                sfd.FilterIndex = formats[defaultFormat].FilterIndex;
                sfd.DefaultExt = formats[defaultFormat].Extension;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string fullPath = sfd.FileName;
                ExportFormat selectedFormat = GetFormatFromExtension(Path.GetExtension(fullPath));

                await SaveDataByFormat(selectedFormat, fullPath, dbName, query, table);
            }
        }

        public static async Task SaveDataByFormat(ExportFormat format, string fullPath, string dbName, string query, DataTable table)
        {
            switch (format)
            {
                case ExportFormat.TXT:
                    SaveAsTabDelimited(table, fullPath);
                    break;

                case ExportFormat.SE4EDT:
                    var data = new SE4EDTData(dbName, query, table);
                    using (DataTransferService dts = new DataTransferService())
                    {
                        await dts.SaveSE4EDTAsync(fullPath, data);
                    }
                    break;

                // case ExportFormat.CSV:
                //     SaveAsCsv(table, fullPath);
                //     break;

                // case ExportFormat.JSON:
                //     SaveAsJson(table, fullPath);
                //     break;

                default:
                    throw new NotSupportedException($"Export format {format} is not supported");
            }
        }

        public static void SaveAsTabDelimited(this DataTable dt, string fullPath, string delimiter = "\t")
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string EscapeField(string field)
                {
                    if (string.IsNullOrEmpty(field)) return "";

                    if (field.Contains(delimiter) || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
                    {
                        return $"\"{field.Replace("\"", "\"\"")}\"";
                    }
                    return field;
                }

                string[] columnNames = dt.Columns.Cast<DataColumn>()
                                                 .Select(c => EscapeField(c.ColumnName))
                                                 .ToArray();
                sb.AppendLine(string.Join(delimiter, columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = row.ItemArray
                                         .Select(field => EscapeField(field?.ToString()))
                                         .ToArray();
                    sb.AppendLine(string.Join(delimiter, fields));
                }

                File.WriteAllText(fullPath, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
