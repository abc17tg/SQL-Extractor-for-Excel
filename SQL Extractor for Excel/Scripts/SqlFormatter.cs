using System;
using System.Diagnostics;
using System.IO;
using SQL_Extractor_for_Excel.Scripts;

public static class SqlFormatter
{
    public enum SqlDialect
    {
        Oracle,
        TSql
    }

    public static string Format(string sql, SqlDialect dialect = SqlDialect.Oracle)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = FileManager.PythonExe,
                Arguments = $"-u \"{FileManager.PythonFormatSqlScriptPath}\" \"{dialect}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = FileManager.PythonDir
            }
        };

        process.Start();

        // Send SQL query to Python script
        using (StreamWriter writer = process.StandardInput)
        {
            if (writer.BaseStream.CanWrite)
            {
                writer.WriteLine(sql);
                writer.Flush();
            }
        }

        // Read output
        string result = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(error))
        {
            throw new Exception($"SQL Format Error: {error}");
        }

        return result.Replace("    ","\t").Trim(); // Replace 4 spaces with tab and trim extra spaces or new lines
    }


}