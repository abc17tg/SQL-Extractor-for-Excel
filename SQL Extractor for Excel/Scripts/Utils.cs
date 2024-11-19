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

    public enum LineEndingType
    {
        Default,   // Use Environment.NewLine
        Unix,      // Use \n
        Windows,   // Use \r\n
        MacOld     // Use \r (old Mac OS pre-OSX)
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

