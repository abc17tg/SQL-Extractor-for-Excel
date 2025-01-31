using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using System.Security.Cryptography;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using SQL_Extractor_for_Excel.Forms;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

public static class Utils
{
    public static List<string> TextExt = new List<string> { ".txt", ".csv" };
    public static List<string> ExcelExt = new List<string> { ".xlsx", ".xlsb", ".xlsm", ".xltm", ".xls", ".xlt" };
    public static Random rand = new Random();

    public static char DetermineTableDelimiter(string filePath)
    {
        char[] delimiters = { '\t', ',', ';', '|' }; // set the candidate delimiters

        using (StreamReader reader = new StreamReader(filePath))
        {
            char[] foundDelimiters = new char[10];
            for (int i = 0; i < 10; i++) // read the first 10 lines of the file
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    char delimiter = delimiters.FirstOrDefault(d => line.Contains(d));
                    if (delimiter != default(char))
                    {
                        foundDelimiters[i] = delimiter; // return the first delimiter found
                    }
                }
                else
                    break;
            }
            if (foundDelimiters.Where(p => p != default(char)).ToArray().Length > 0 && foundDelimiters.Where(p => p != default(char)).All(p => p.Equals(foundDelimiters[0])))
                return foundDelimiters[0];
        }
        return default(char); // no delimiter found
    }

    public static HashSet<Type> NumericTypes = new HashSet<Type>
    {
        typeof(int), typeof(long), typeof(double), typeof(decimal), typeof(float),
        typeof(short), typeof(byte), typeof(sbyte), typeof(uint), typeof(ulong), typeof(ushort)
    };

    public enum LineEndingType
    {
        Default,   // Use Environment.NewLine
        Unix,      // Use \n
        Windows,   // Use \r\n
        MacOld     // Use \r (old Mac OS pre-OSX)
    }

    public static string RemoveLeadingTabsMultiline(this string input)
    {
        // Split the input into lines
        var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        // Find the minimum number of leading tabs for non-empty lines
        int minLeadingTabs = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.TakeWhile(c => c == '\t').Count())
            .DefaultIfEmpty(0) // Handle empty or whitespace-only input
            .Min();

        // Remove the minimum number of leading tabs from each line
        var normalizedLines = lines.Select(line => line.StartsWith(new string('\t', minLeadingTabs))
                                    ? line.Substring(minLeadingTabs)
                                    : line);

        // Join the lines back into a single string
        return string.Join(Environment.NewLine, normalizedLines);
    }

    public static string UnifyLineEndings(this string text, LineEndingType lineEndingType = LineEndingType.Default)
    {
        if (text == null)
            return null;

        // Determine the replacement string based on the specified line ending type
        string replacement;
        switch (lineEndingType)
        {
            case LineEndingType.Unix:
                replacement = "\n"; // Unix/Linux
                break;
            case LineEndingType.Windows:
                replacement = "\r\n"; // Windows
                break;
            case LineEndingType.MacOld:
                replacement = "\r"; // Old Mac OS
                break;
            default:
                replacement = Environment.NewLine; // Default to current environment's newline
                break;
        }

        // Use Regex to replace all forms of line endings with the desired format
        return Regex.Replace(text, @"\r\n|\n\r|\n|\r", replacement);
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
    {
        int i = 0;
        var splits = from item in list
                     group item by i++ % parts into part
                     select part.AsEnumerable();
        return splits;
    }

    public static object[,] NewObject2DArray(int iRows, int iCols)
    {
        int[] aiLowerBounds = new int[] { 1, 1 };
        int[] aiLengths = new int[] { iRows, iCols };

        return (object[,])Array.CreateInstance(typeof(object), aiLengths, aiLowerBounds);
    }

    // due to critical error not working for now
    public static bool IsSQLQueryValid(string sql, out List<string> errors)
    {
        errors = new List<string>();
        return true;

        /*        TSql140Parser parser = new TSql140Parser(false);
                TSqlFragment fragment;
                IList<ParseError> parseErrors = null;

                using (TextReader reader = new StringReader(sql))
                {
                    try
                    {
                        fragment = parser.Parse(reader, out parseErrors);
                    }
                    catch (StackOverflowException)
                    { 
                        return false; 
                    }

                    if (parseErrors != null && parseErrors.Count > 0)
                    {
                        errors = parseErrors.Select(e => e.Message).ToList();
                        return false;
                    }
                }
                return true;*/
    }

    public static string GenerateSqlFilter(DataTable table)
    {
        var orderedColumns = table.Columns.Cast<DataColumn>()
            .Select(col => new
            {
                Column = col,
                Count = table.AsEnumerable().Select(row => row[col]).Distinct().Count()
            })
            .OrderBy(x => x.Count)
            .Select(x => x.Column)
            .ToList();

        string BuildSql(List<DataColumn> columns, IEnumerable<DataRow> rows, int indentationLevel = 0)
        {
            if (!columns.Any()) return string.Empty;

            var currentColumn = columns.First();
            string currentColumnName = currentColumn.ColumnName.Contains(' ') ? $"[{currentColumn.ColumnName}]" : currentColumn.ColumnName;
            bool numeric = NumericTypes.Contains(currentColumn.DataType);
            var remainingColumns = columns.Skip(1).ToList();
            var groupedRows = rows.GroupBy(row => row[currentColumn.ColumnName]);

            var sb = new StringBuilder();
            string indentation = new string('\t', indentationLevel);
            bool isLeaf = !remainingColumns.Any();

            foreach (var group in groupedRows)
            {
                string condition;
                object key = group.Key;

                // Handle NULL/empty values
                if (key == DBNull.Value || string.IsNullOrWhiteSpace(key?.ToString()))
                {
                    condition = $"( {currentColumnName} IS NULL OR {currentColumnName} = '' )";
                }
                else
                {
                    if (numeric)
                        condition = $"{currentColumnName} = {key}";
                    else
                        condition = $"{currentColumnName} = '{key}'";
                }

                // Recursively build sub-conditions
                string subSql = BuildSql(remainingColumns, group, indentationLevel + 1);
                bool subSqlHasFreeORCondition = Regex.Matches(subSql, @"(?<=(^\s*\(\n*$)([\s\S]*?))(^\s*OR\n*$)(?=([\s\S]*?)(^\s*\)\n*$))", RegexOptions.Multiline).Count != Regex.Matches(subSql, @"^\s*OR\s*$", RegexOptions.Multiline).Count;

                if (!string.IsNullOrEmpty(subSql))
                {
                    if (subSqlHasFreeORCondition && Regex.Match(subSql, @"^\s*AND\s*$", RegexOptions.Multiline).Success)
                        condition += $"\n{indentation}\tAND\n{indentation}\t(\n{indentation}\t\t(\n{string.Join("\n", subSql.Split('\n').Select(p => $"\t{p}"))}\n{indentation}\t\t)\n{indentation}\t)";
                    else if (subSqlHasFreeORCondition)
                        condition += $"\n{indentation}\tAND\n{indentation}\t(\n{subSql}\n{indentation}\t)";
                    else if (isLeaf)
                        condition += $"\n{indentation}\tAND\n{subSql}";
                    else
                        condition += $"\n{indentation}\tAND\n{string.Join("\n", subSql.Split('\n').Select(p => string.Join("", p.Skip(1))))}";
                }

                // Add to final output
                bool multilineCondition = condition.Contains('\n');
                bool containedCondition = false;
                if (multilineCondition && Regex.Matches(condition, @"(?<=(^\s*\(\n*$)([\s\S]*?))(^\s*OR\n*$)(?=([\s\S]*?)(^\s*\)\n*$))", RegexOptions.Multiline).Count != Regex.Matches(condition, @"^\s*OR\s*$", RegexOptions.Multiline).Count)
                {
                    condition = $"{indentation}(\n{indentation}\t{condition}\n{indentation})";
                    containedCondition = true;
                }
                else
                    condition = $"{indentation}\t{condition}";


                string sbValue = sb.ToString();
                if (sbValue != string.Empty)
                {
                    bool sbContained = (Regex.Match(sbValue, @"^\s*\([\s\S]*\)\s*$").Success && sbValue.Count(p => p == '(') == sbValue.Count(p => p == ')'));
                    if ((sbContained && multilineCondition && containedCondition) || (sbContained && !multilineCondition))
                        sb.Append($"\n{indentation}OR\n");
                    else if (!sbContained && multilineCondition && containedCondition)
                        sb.Append($"\n{indentation})\n{indentation}OR\n");
                    else if ((sbContained || sbValue.EndsWith($"\n{indentation})")) && multilineCondition && !containedCondition)
                        sb.Append($"\n{indentation}OR\n{indentation}(\n");
                    else if (!sbContained && multilineCondition && !containedCondition)
                        sb.Append($"\n{indentation})\n{indentation}OR\n{indentation}(\n");
                    else
                        sb.Append($"\n{indentation}OR\n"); //sb.Append($"\n{indentation})\n{indentation}OR\n{indentation}(\n");
                }

                sb.Append(condition);
            }

            // Handle IN clauses for leaf nodes
            if (isLeaf && sb.ToString().Contains("OR"))
            {
                var values = groupedRows.Select(g => g.Key.ToString()).Distinct().ToList();
                if (values.Count > 1000)
                {
                    string valuesSeparated = string.Join($"\n{indentation}\tOR\n{indentation}\t", values.Split((int)Math.Ceiling((double)values.Count / 1000)).Select(chunk => $"{currentColumnName} IN ({(numeric ? string.Join(", ", chunk.Select(v => $"{v}")) : string.Join(", ", chunk.Select(v => $"'{v}'")))})"));
                    return $"{indentation}\t{valuesSeparated}";
                }
                return $"{indentation}\t{currentColumnName} IN ({(numeric ? string.Join(", ", values.Select(v => $"{v}")) : string.Join(", ", values.Select(v => $"'{v}'")))})";
            }
            return sb.ToString();
        }

        var sqlFilter = BuildSql(orderedColumns, table.AsEnumerable());
        //return $"(\n{sqlFilter}\n)";

        if (Regex.Match(sqlFilter.ToString(), @"^\s*\([\s\S]*\)\s*$").Success)
            return sqlFilter;

        return $"(\n{sqlFilter}\n)";
    }

    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }

    public static string GetUniqueString(List<string> existingStrings, string baseString)
    {
        baseString = baseString.Substring(0, Math.Min(31, baseString.Length));
        string newString = baseString;
        int i = 1;

        while (existingStrings.Contains(newString))
        {
            string suffix = $" ({i})";
            int baseStringLength = 31 - suffix.Length;

            // Cut characters from the base string to make room for the suffix
            newString = baseString.Substring(0, Math.Min(baseStringLength, baseString.Length)) + suffix;
            i++;
        }

        return newString;
    }

    public static SortedDictionary<string, long> GetCounts(DataTable dt, string searchWord = "")
    {
        var counts = new SortedDictionary<string, long>();
        long rowsCount = dt.Rows.Count;

        object lockObject = new object();
        Parallel.ForEach<DataColumn>(dt.Columns.Cast<DataColumn>(), column =>
        {
            long count = dt.AsEnumerable().Select(p => p[column]?.ToString()).LongCount(p => (!string.IsNullOrEmpty(p) && (string.IsNullOrEmpty(searchWord) || p.Contains(searchWord, StringComparison.OrdinalIgnoreCase))));

            lock (lockObject)
                counts[column.ColumnName] = count;
        });

        return counts;
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }

    public static void SuperShuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void MoveFormToCursor(Form form)
    {
        // Set the form's location to the cursor's position
        form.Location = Cursor.Position;

        // Get the working area of the screen that contains the form
        Rectangle screenWorkingArea = Screen.GetWorkingArea(form);

        // Check if the form is completely visible in the screen's working area
        if (!screenWorkingArea.Contains(form.Bounds))
        {
            // If not, adjust the form's location

            // If the form's right edge is out of the screen, move it to the left
            if (form.Right > screenWorkingArea.Right)
            {
                form.Left = screenWorkingArea.Right - form.Width;
            }

            // If the form's bottom edge is out of the screen, move it up
            if (form.Bottom > screenWorkingArea.Bottom)
            {
                form.Top = screenWorkingArea.Bottom - form.Height;
            }

            // If the form's left edge is out of the screen, move it to the right
            if (form.Left < screenWorkingArea.Left)
            {
                form.Left = screenWorkingArea.Left;
            }

            // If the form's top edge is out of the screen, move it down
            if (form.Top < screenWorkingArea.Top)
            {
                form.Top = screenWorkingArea.Top;
            }
        }
    }

    public static void MoveFormToCenter(Form form)
    {
        Point cursorPosition = Cursor.Position;

        Screen currentScreen = Screen.FromPoint(cursorPosition);

        Rectangle workingArea = currentScreen.WorkingArea;

        int newX = workingArea.X + (workingArea.Width - form.Width) / 2;
        int newY = workingArea.Y + (workingArea.Height - form.Height) / 2;

        form.Location = new Point(newX, newY);
    }

    public static void SaveAsTabDelimited(this DataTable dt, string delimiter = "\t", string folderPath = null)
    {
        object lockObject = new object();
        bool delimiterExist = false;
        Parallel.ForEach<DataColumn>(dt.Columns.Cast<DataColumn>(), column =>
        {
            bool exist = dt.AsEnumerable().Any(p => (p[column]?.ToString() ?? string.Empty).Contains(delimiter, StringComparison.OrdinalIgnoreCase));

            if (exist)
                lock (lockObject)
                    delimiterExist = true;
        });

        while (delimiterExist || string.IsNullOrEmpty(delimiter))
        {
            InputBoxForm inputBoxForm = new InputBoxForm("Choose delimiter", $"Delimiter \"{delimiter}\" contained in values, choose another: ");
            inputBoxForm.ShowDialog();

            if (inputBoxForm.DialogResult == DialogResult.Cancel)
                return;

            delimiter = inputBoxForm.Result;
            Parallel.ForEach<DataColumn>(dt.Columns.Cast<DataColumn>(), column =>
            {
                bool exist = dt.AsEnumerable().Any(p => (p[column]?.ToString() ?? string.Empty).Contains(delimiter, StringComparison.OrdinalIgnoreCase));

                if (exist)
                    lock (lockObject)
                        delimiterExist = true;
            });
        }

        SaveFileDialog saveDlg = new SaveFileDialog();

        if (!string.IsNullOrEmpty(folderPath))
            saveDlg.InitialDirectory = folderPath;
        else
            saveDlg.InitialDirectory = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

        saveDlg.FileName = (string.IsNullOrWhiteSpace(dt.TableName) ? "DT_Export" : dt.TableName) + DateTime.Now.ToString("_yyyy_MM_dd");
        saveDlg.OverwritePrompt = true;
        saveDlg.DefaultExt = ".txt";
        saveDlg.AddExtension = true;
        saveDlg.Filter = "Text Files | *.txt";

        if (saveDlg.ShowDialog() == DialogResult.OK)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Add column headers
                string[] columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                sb.AppendLine(string.Join(delimiter, columnNames));

                // Add rows
                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    sb.AppendLine(string.Join(delimiter, fields));
                }

                File.WriteAllText(saveDlg.FileName, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }

    public static IEnumerable<T> FindAllChildrenByType<T>(this Control control)
    {
        IEnumerable<Control> controls = control.Controls.Cast<Control>();
        return controls
            .OfType<T>()
            .Concat<T>(controls.SelectMany<Control, T>(ctrl => FindAllChildrenByType<T>(ctrl)));
    }

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

}

