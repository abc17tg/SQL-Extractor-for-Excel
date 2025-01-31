using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Shell32;

namespace SQL_Extractor_for_Excel.Scripts
{

    internal class FileManager
    {
        public static string BasePath => AppDomain.CurrentDomain.BaseDirectory;
        public static string SqlKeywords => GetSqlKeywords();
        public static string SqlEditorBackupPath => Path.Combine(BasePath, "SQL Editor backup");
        public static string SqlQueriesPath => Path.Combine(BasePath, "SQL Queries");
        public static string PropertiesFilesPath => Path.Combine(BasePath, "Properties Files");
        public static string SqlServerQueriesPath => Path.Combine(SqlQueriesPath, "SqlServer");
        public static string OracleQueriesPath => Path.Combine(SqlQueriesPath, "Oracle");
        public static string ExcelQueriesPath => Path.Combine(SqlQueriesPath, "Excel");
        public static string ResourcesPath => Path.Combine(BasePath, "Resources");
        public static string DownloadsPath => Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        public static string PythonDir => Path.Combine(FileManager.BasePath, "Python");
        public static string PythonExe => Path.Combine(PythonDir, "python.exe");
        public static string PythonFormatSqlScriptPath => Path.Combine(PythonDir, "format_sql.py");

        public static bool EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true; // Directory exists or was successfully created
            }
            catch
            {
                return false; // Failed to ensure the directory exists
            }
        }

        private static string GetSqlKeywords()
        {
            try
            {
                string filePath = Path.Combine(PropertiesFilesPath, "SqlKeywords.txt");
                return File.ReadAllText(filePath);
            }
            catch (IOException)
            {
                return "select from where group by in not is sum count null";
            }
            catch (Exception)
            {
                return "select from where group by in not is sum count null";
            }
        }

        public static bool IsExplorerPathOpen(string path)
        {
            Shell shell = new Shell();
            var windows = shell.Windows();
            for (int i = 0; i < windows.Count; i++)
            {
                var window = windows.Item(i);
                if (window != null && window.Path.ToLower() == path.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public static Dictionary<string, string> GetOracleQueries()
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(OracleQueriesPath, "*.sql", SearchOption.AllDirectories))
                    queries.Add(filePath, File.ReadAllText(filePath));
            }
            catch { }
            return queries;
        }

        public static Dictionary<string, string> GetSqlServerQueries()
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(SqlServerQueriesPath, "*.sql", SearchOption.AllDirectories))
                    queries.Add(filePath, File.ReadAllText(filePath));
            }
            catch { }
            return queries;
        }

        public static Dictionary<string, string> GetExcelQueries()
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(ExcelQueriesPath, "*.sql", SearchOption.AllDirectories))
                    queries.Add(filePath, File.ReadAllText(filePath));
            }
            catch { }
            return queries;
        }

        public static Dictionary<string, SqlConn> GetOracleConnectionValues() =>
            GetConnectionValues(OracleQueriesPath);

        public static Dictionary<string, SqlConn> GetSqlServerConnectionValues() =>
            GetConnectionValues(SqlServerQueriesPath);

        private static Dictionary<string, SqlConn> GetConnectionValues(string path)
        {
            Dictionary<string, SqlConn> connD = new Dictionary<string, SqlConn>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
                    connD.Add(Path.GetFileNameWithoutExtension(filePath), SqlConn.LoadSqlConn(filePath));
                if (connD.Count < 1)
                    throw new Exception();
            }
            catch
            {
                var result = MessageBox.Show("Can not find any saved server.\n\nDo you want to create connection?", "No servers found", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ServerConnectionForm serverConnectionForm = new ServerConnectionForm();
                    result = serverConnectionForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        return GetConnectionValues(path);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return connD;
        }

        public static List<string> GetOracleQueryKeys()
        {
            List<string> keys = new List<string>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(OracleQueriesPath, "*.sql", SearchOption.AllDirectories))
                    keys.Add(filePath);
            }
            catch { }
            return keys;
        }

        public static List<string> GetSqlServerQueryKeys()
        {
            List<string> keys = new List<string>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(SqlServerQueriesPath, "*.sql", SearchOption.AllDirectories))
                    keys.Add(filePath);
            }
            catch { }
            return keys;
        }

        public static List<string> GetOracleServerNames() =>
            GetServerNames(OracleQueriesPath);

        public static List<string> GetSqlServerNames() =>
            GetServerNames(SqlServerQueriesPath);

        private static List<string> GetServerNames(string path)
        {
            List<string> names = new List<string>();
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
                    names.Add(Path.GetFileNameWithoutExtension(filePath));
                if (names.Count < 1)
                    throw new Exception();
            }
            catch
            {
                var result = MessageBox.Show("Can not find any saved server.\n\nDo you want to create connection?", "No servers found", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ServerConnectionForm serverConnectionForm = new ServerConnectionForm();
                    result = serverConnectionForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        return GetServerNames(path);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return names;
        }

        public static string GetPathByDialog(string initialName = "", string initialDirectory = "", string filter = "Text Files | *.txt", string defaultExt = ".txt")
        {
            SaveFileDialog saveDlg = new SaveFileDialog();

            if (!string.IsNullOrEmpty(initialDirectory))
                saveDlg.InitialDirectory = initialDirectory;
            else
                saveDlg.InitialDirectory =

            saveDlg.FileName = initialName;
            saveDlg.OverwritePrompt = true;
            saveDlg.DefaultExt = defaultExt;
            saveDlg.AddExtension = true;
            saveDlg.Filter = filter;

            if (saveDlg.ShowDialog() == DialogResult.OK)
                return saveDlg.FileName;
            else
                return null;
        }

        public static void OpenStringWithNotepad(string text)
        {
            // Create a temporary file and write the text to it
            string path = string.Empty;
            try
            {
                path = Path.GetTempFileName();
                File.WriteAllText(path, text);
            }
            catch
            {
                MessageBox.Show("Error occured while creating temporary file to open in notepad!\nOpen and paste contents manually if you still wanna open current document in notepad");
                return;
            }

            try
            {
                Process.Start(Path.Combine(Environment.GetEnvironmentVariable("programfiles"), @"Sublime Text\sublime_text.exe"), $"\"{path}\"");
            }
            catch (Exception)
            {
                try
                {
                    Process.Start(Path.Combine(Environment.GetEnvironmentVariable("programfiles"), @"Notepad++\notepad++.exe"), $"\"{path}\"");
                }
                catch (Exception)
                {
                    try
                    {
                        Process.Start(Path.Combine(Environment.GetEnvironmentVariable("programfiles(x86)"), @"Notepad++\notepad++.exe"), $"\"{path}\"");
                    }
                    catch (Exception)
                    {
                        Process.Start("notepad.exe", $"\"{path}\"");
                    }
                }
            }
        }
    }
}
