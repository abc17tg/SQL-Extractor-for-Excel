using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;
using static ScintillaNET.Style;

namespace SQL_Extractor_for_Excel.Scripts
{
    public static class UtilsScintilla
    {
        public static readonly string ScintillaSqlQuerySeparator = new string('-', 50); // "--------------------------------------------------";

        public static void Comment(Scintilla editor, string commentSymbol = "--")
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                int startLine = editor.LineFromPosition(editor.SelectionStart);
                int endLine = editor.LineFromPosition(editor.SelectionEnd);
                editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - Environment.NewLine.Length);

                string selectedText = editor.SelectedText;

                if (!string.IsNullOrEmpty(selectedText))
                {
                    List<string> lines = selectedText.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                    if (lines.Where(p => p.Trim() != Environment.NewLine && p.Trim() != string.Empty).All(p => p.TrimStart().StartsWith(commentSymbol)))
                    {
                        for (int i = 0; i < lines.Count; i++)
                            if (lines[i].Trim() != Environment.NewLine && lines[i].Trim() != string.Empty)
                                lines[i] = lines[i].Split(new[] { commentSymbol }, 2, StringSplitOptions.None)[1];
                    }
                    else
                    {
                        for (int i = 0; i < lines.Count; i++)
                            if (lines[i].Trim() != Environment.NewLine && lines[i].Trim() != string.Empty)
                                lines[i] = commentSymbol + lines[i];
                    }

                    editor.ReplaceSelection(string.Join(Environment.NewLine, lines));
                }
                editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
            }
        }

        public static void IndentAfterReturn(Scintilla editor)
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                int currentPos = editor.CurrentPosition;
                int currentLine = editor.LineFromPosition(currentPos);
                editor.Lines[currentLine].Indentation = editor.Lines[currentLine - 1].Indentation;
                editor.GotoPosition(editor.Lines[currentLine].IndentPosition);
                //editor.GotoPosition(editor.Lines[currentLine].EndPosition - (editor.Lines[currentLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
            }
        }

        public static void SelectCurrentWord(Scintilla editor)
        {
            // Get the current position
            int currentPos = editor.CurrentPosition;

            // Find the start position of the current word
            int startPos = editor.WordStartPosition(currentPos, false);
            // Find the end position of the current word
            int endPos = editor.WordEndPosition(currentPos, false);

            if (!(!char.IsWhiteSpace(editor.GetCharAt(currentPos)) && ((currentPos > 0 && !char.IsWhiteSpace(editor.GetCharAt(currentPos - 1))) || (currentPos == 0))))
                if (!(!char.IsWhiteSpace(editor.GetCharAt(currentPos)) && ((currentPos < editor.TextLength && !char.IsWhiteSpace(editor.GetCharAt(currentPos + 1))) || (currentPos == editor.TextLength - 1))))
                    return;

            while (new char[] { '\r', '\n' }.Contains(editor.GetCharAt(startPos)))
                startPos += 1;
            if (startPos > 0 && new char[] { '\'', '(' }.Contains(editor.GetCharAt(startPos - 1)))
            {
                startPos -= 1;
                if (startPos > 0 && editor.GetCharAt(startPos - 1) == '(')
                    startPos -= 1;
            }

            while (new char[] { '\r', '\n' }.Contains(editor.GetCharAt(endPos)))
                endPos -= 1;
            if (endPos < editor.TextLength && new char[] { '\'', ')' }.Contains(editor.GetCharAt(endPos + 1)))
            {
                endPos = endPos + 1;
                if (endPos < editor.TextLength && editor.GetCharAt(endPos + 1) == ')')
                    endPos += 1;
            }

            if (editor.GetCharAt(startPos) == '(' ^ editor.GetCharAt(endPos) == ')')
            {
                if (editor.GetCharAt(startPos) == '(')
                    startPos += 1;
                else
                    endPos -= 1;
            }

            if (editor.GetCharAt(startPos) == '\'' ^ editor.GetCharAt(endPos) == '\'')
            {
                if (editor.GetCharAt(startPos) == '\'')
                    startPos += 1;
                else
                    endPos -= 1;
            }

            // If we found a word, select it
            if (startPos != endPos)
            {
                editor.SetSelection(startPos, endPos + 1);
            }
        }

        public static void ReformatTextToSqlFilter(Scintilla editor, string text = null)
        {
            if (text == null)
            {
                text = editor.SelectedText;
                if (string.IsNullOrWhiteSpace(text))
                {
                    SelectCurrentWord(editor);
                    text = editor.SelectedText;
                }
            }

            if (string.IsNullOrWhiteSpace(text))
                return;

            string origText = text;
            int startPos = editor.SelectionStart;

            // Efficient regex patterns - avoid catastrophic backtracking
            Regex columnPrefixPattern = new Regex(@"^\s*(?:\(\s*'[A-z_]+',\s*(?<ColumnName>\w+)\s*\)\s+IN\s+|\s+(?<ColumnName>\w+)\s+IN\s+)", RegexOptions.Compiled);
            Regex quotedTuplePattern = new Regex(@"^\s*(?:\(\s*'[A-z_]+',\s*(?<ColumnName>\w+)\s*\)\s+IN\s+)?\((?:\('X',\s*'[^']*'\)\s*,?\s*)+\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex unquotedTuplePattern = new Regex(@"^\s*(?:\(\s*'[A-z_]+',\s*(?<ColumnName>\w+)\s*\)\s+IN\s+)?\((?:\('X',\s*-?\d+(?:\.\d+)?\)\s*,?\s*)+\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex singleQuotedPattern = new Regex(@"^\(\s*'[^']*'(?:\s*,\s*'[^']*')*\s*\)$", RegexOptions.Compiled);
            Regex unquotedPattern = new Regex(@"^\(\s*-?\d+(?:\.\d+)?(?:\s*,\s*-?\d+(?:\.\d+)?)*\s*\)$", RegexOptions.Compiled);
            Regex numericPattern = new Regex(@"^-?\d+(?:\.\d+)?$", RegexOptions.Compiled);

            char[] DelimiterChars = { ' ', '\'', '(', ')', ',', '\t', '\n', '\r', ';', '|' };

            string result = string.Empty;
            string columnName = null;
            string workingText = text;

            // Extract column prefix if present
            var colMatch = columnPrefixPattern.Match(text);
            if (colMatch.Success)
            {
                columnName = colMatch.Groups[1].Value;
                workingText = text.Substring(colMatch.Length);
            }

            // Check format and toggle
            var quotedTupleMatch = quotedTuplePattern.Match(text);
            if (quotedTupleMatch.Success)
            {
                string tupleCol = quotedTupleMatch.Groups[1].Success ? quotedTupleMatch.Groups[1].Value : null;
                result = FormatTupleToUnquoted(text, numericPattern, tupleCol);
            }
            else
            {
                var unquotedTupleMatch = unquotedTuplePattern.Match(text);
                if (unquotedTupleMatch.Success)
                {
                    string tupleCol = unquotedTupleMatch.Groups[1].Success ? unquotedTupleMatch.Groups[1].Value : null;
                    result = FormatTupleToQuoted(text, tupleCol);
                }
                else if (singleQuotedPattern.IsMatch(workingText))
                {
                    result = FormatToUnquotedSqlFilter(workingText, numericPattern, columnName);
                }
                else if (unquotedPattern.IsMatch(workingText))
                {
                    result = FormatToQuotedSqlFilter(workingText, numericPattern, columnName);
                }
                else
                {
                    // Parse raw text
                    var allVals = text.Split(DelimiterChars, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .Distinct()
                        .ToList();

                    bool isNumeric = allVals.All(p => numericPattern.IsMatch(p));
                    if (allVals.Count > 1000)
                    {
                        result = UtilsExcel.CreateTupleFilter(allVals, columnName ?? "", isNumeric);
                    }
                    else
                    {
                        if (isNumeric)
                            result = FormatToUnquotedSqlFilter(string.Join(",", allVals), numericPattern, columnName);
                        else
                            result = FormatToQuotedSqlFilter(string.Join(",", allVals), numericPattern, columnName);
                    }
                }
            }

            /*editor.ReplaceSelection(result);
            editor.SetSelection(startPos, startPos + result.Length);*/
            editor.ReplaceSelection(result);

            int offset = result.IndexOf(") IN (('", StringComparison.OrdinalIgnoreCase) + (") IN ").Length;
            if (offset < 0)
                offset = result.IndexOf('(');
            if (offset < 0)
                offset = 0;

            editor.SetSelection(startPos + offset, startPos + result.Length);
        }

        private static string FormatTupleToUnquoted(string text, Regex numericPattern, string columnName)
        {
            // Extract values
            var valMatches = Regex.Matches(text, @"\('X',\s*'([^']*)'\)");
            if (valMatches.Count == 0) return text;

            var vals = valMatches.Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .ToList();

            // Check if all values are numeric
            if (!vals.All(v => numericPattern.IsMatch(v)))
                return text;

            // Format as unquoted tuple
            var tuples = vals.Select(v => $"('X', {v})");
            string tuplesStr = string.Join(", ", tuples);
            if (string.IsNullOrEmpty(columnName))
                return $"({tuplesStr})";
            else
                return $"('X', {columnName}) IN ({tuplesStr})";
        }

        private static string FormatTupleToQuoted(string text, string columnName)
        {
            // Extract values from tuples
            var valMatches = Regex.Matches(text, @"\('X',\s*([^)]+)\)");
            if (valMatches.Count == 0) return text;

            var vals = valMatches.Cast<Match>()
                .Select(m => m.Groups[1].Value.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .ToList();

            // If over 1000, return quoted tuple
            if (vals.Count > 1000)
            {
                return UtilsExcel.CreateTupleFilter(vals, columnName ?? "", false);
            }

            // Format as simple quoted list
            string valueList = vals.Count == 1 ? $"'{vals[0].Replace("'", "''")}'" : $"({string.Join(", ", vals.Select(v => $"'{v.Replace("'", "''")}'"))})";
            if (string.IsNullOrEmpty(columnName))
                return valueList;
            else
                return $"{columnName} IN {valueList}";
        }

        private static string FormatToUnquotedSqlFilter(string text, Regex numericPattern, string columnName)
        {
            text = text.Trim('(', ')');

            var vals = text.Split(',')
                .Select(p => p.Trim().Trim('\''))
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .ToList();

            if (vals.Count > 1000)
            {
                if (!vals.All(v => numericPattern.IsMatch(v)))
                    return columnName != null ? $"{columnName} IN {text}" : text;

                return UtilsExcel.CreateTupleFilter(vals, columnName ?? "", true);
            }

            string valueList;
            if (vals.Count == 1)
                valueList = vals[0];
            else
                valueList = $"({string.Join(", ", vals)})";

            if (columnName != null)
                return $"{columnName} IN {valueList}";
            else
                return valueList;
        }

        private static string FormatToQuotedSqlFilter(string text, Regex numericPattern, string columnName)
        {
            text = text.Trim('(', ')');

            var vals = text.Split(',')
                .Select(p => p.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .Distinct()
                .ToList();

            // Check if we should convert to tuple (over 1000 values)
            if (vals.Count > 1000)
            {
                return UtilsExcel.CreateTupleFilter(vals, columnName ?? "", false);
            }

            string valueList;
            if (vals.Count == 1)
                valueList = $"'{vals[0].Replace("'", "''")}'";
            else
                valueList = $"({string.Join(", ", vals.Select(v => $"'{v.Replace("'", "''")}'"))})";

            if (columnName != null)
                return $"{columnName} IN {valueList}";
            else
                return valueList;
        }

        public static void ToggleSpacesAndNewLines(Scintilla editor, string text = null)
        {
            if (text == null)
                text = editor.SelectedText;
            if (string.IsNullOrWhiteSpace(text))
                return;

            string originalText = text;

            int startPos = editor.SelectionStart;
            string firstLineIndentation = GetIndentationLevel(editor);

            // Use a regex to detect newlines that separate actual SQL code (ignoring comment-only lines)
            bool isMultiLine = Regex.IsMatch(text, @"(?<!--.*)\r?\n\s*(?!--)");

            if (isMultiLine)
            {
                // Collapse non-comment lines into a single line while preserving comment lines
                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var outputLines = new List<string>();
                var buffer = new List<string>();

                foreach (var line in lines)
                {
                    if (Regex.IsMatch(line.TrimStart(), @"^--"))
                    {
                        if (buffer.Count > 0)
                        {
                            // Merge the buffered non-comment lines
                            var joined = Regex.Replace(string.Join(" ", buffer), @"\s+", " ")
                                              .Replace(" ,", ",").Trim();
                            outputLines.Add(joined);
                            buffer.Clear();
                        }
                        outputLines.Add(line.TrimEnd());
                    }
                    else
                    {
                        buffer.Add(line.Trim());
                    }
                }
                if (buffer.Count > 0)
                {
                    var joined = Regex.Replace(string.Join(" ", buffer), @"\s+", " ")
                                      .Replace(" ,", ",").Trim();
                    outputLines.Add(joined);
                }
                text = string.Join(Environment.NewLine, outputLines);
            }
            else
            {
                // Expand to multi-line: for each non-comment line, split the text at commas outside quotes/brackets.
                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var processedLines = new List<string>();

                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line.TrimStart(), @"^--"))
                    {
                        processedLines.Add(line.TrimEnd());
                    }
                    else
                    {
                        var columns = Utils.SplitSqlColumns(line);
                        foreach (var col in columns)
                        {
                            if (!string.IsNullOrWhiteSpace(col))
                                processedLines.Add(firstLineIndentation + col.Trim());
                        }
                    }
                }
                text = string.Join(Environment.NewLine, processedLines).Trim();
            }

            int endPos = startPos + text.Length;
            if (Utils.CompareIgnoringWhitespace(originalText, text))
            {
                editor.ReplaceSelection(text);
                editor.SetSelection(startPos, endPos);
            }
        }

        public static string GetIndentationLevel(this Scintilla scintilla, bool replaceOtherCharToSpace = false)
        {
            int position = scintilla.SelectionStart;
            int lineNumber = scintilla.LineFromPosition(position);
            int indentation = scintilla.Lines[lineNumber].Indentation;
            int lineStartPos = scintilla.Lines[lineNumber].Position;

            if (position == lineStartPos)
                return string.Empty;

            string lineText = scintilla.GetTextRange(lineStartPos, position - lineStartPos);
            if (!replaceOtherCharToSpace)
                return new string('\t', indentation / scintilla.TabWidth);
            else
                return new string(lineText.Select(p => char.IsWhiteSpace(p) ? p : ' ').ToArray());
        }

        public static void SelectBlock(Scintilla editor, string blockStartIdentifier = "-----", string blockEndIdentifier = "-----") // Selects block that is serrounded by at least 5 '-'
        {
            int currentLine = editor.LineFromPosition(editor.CurrentPosition);

            // Find the start line
            int startLine = currentLine;
            if (string.IsNullOrWhiteSpace(blockStartIdentifier))
                startLine = 0;
            else
                while (startLine > 0)
                {
                    string lineText = editor.Lines[startLine].Text.Trim();
                    if (lineText.StartsWith(blockStartIdentifier))
                    {
                        break;
                    }
                    startLine--;
                }

            // Find the end line
            int endLine = currentLine;
            if (string.IsNullOrWhiteSpace(blockEndIdentifier))
                endLine = editor.Lines.Count - 1;
            else
                while (endLine < editor.Lines.Count - 1)
                {
                    string lineText = editor.Lines[endLine].Text.Trim();
                    if (lineText.StartsWith(blockEndIdentifier))
                    {
                        break;
                    }
                    endLine++;
                }

            // Select the lines
            int startPosition = editor.Lines[startLine].Position;
            int endPosition = editor.Lines[endLine].EndPosition;

            editor.SetSelection(startPosition, endPosition);
        }

        public static void WrapIntoSqlBlock(Scintilla editor)
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                string text = editor.SelectedText;
                int indentation = editor.Lines[editor.CurrentLine].Indentation;
                int selStartPos = editor.SelectionStart;
                int selEndPos = editor.SelectionEnd;
                int startLine = editor.LineFromPosition(selStartPos);
                int endLine = editor.LineFromPosition(selEndPos);
                int linesCount = endLine - startLine + 2;

                // Wrap the selected text with parentheses and newlines
                string newText = $"({Environment.NewLine}{text}{Environment.NewLine})";
                editor.ReplaceSelection(newText);

                // Calculate the new selection range
                int newSelStart = editor.Lines[startLine].Position; // Start of the first line after "("
                int newSelEnd = editor.Lines[startLine + linesCount].EndPosition - Environment.NewLine.Length; // End of the last line after ")"

                // Set the new selection
                editor.SetSelection(newSelStart, newSelEnd);

                // Restore indentation for the first and last lines
                editor.Lines[startLine].Indentation = indentation;
                editor.Lines[startLine + linesCount].Indentation = indentation;

                // Adjust indentation for the wrapped lines
                for (int i = startLine + 1; i <= startLine + linesCount - 1; i++)
                {
                    editor.Lines[i].Indentation = Math.Max(indentation, editor.Lines[i].Indentation) + editor.TabWidth;
                }

                if (string.IsNullOrEmpty(text))
                {
                    editor.GotoPosition(editor.Lines[startLine + 1].EndPosition - Environment.NewLine.Length);
                }
            }
        }

        public static string EndsWithNewLineString(this Line line)
        {
            if (line == null || line.Text == null)
                return string.Empty;

            string text = line.Text;

            switch (true)
            {
                case bool _ when text.EndsWith("\r\n"):
                    return "\r\n";
                case bool _ when text.EndsWith("\n"):
                    return "\n";
                case bool _ when text.EndsWith("\r"):
                    return "\r";
                default:
                    return string.Empty;
            }
        }

        public static void ReplaceLineText(this Line line, string newText, Scintilla editor)
        {
            if (line == null)
                return;

            // Handle null newText by converting to empty string
            newText = newText ?? string.Empty;

            // Get line boundaries
            int startPos = line.Position;
            int endPos = line.EndPosition;

            // Set target to entire line and replace
            editor.TargetStart = startPos;
            editor.TargetEnd = endPos;
            editor.ReplaceTarget(newText);
        }

        public static void MoveLineUp(Scintilla editor)
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                int startLine = editor.LineFromPosition(editor.SelectionStart);
                int endLine = editor.LineFromPosition(editor.SelectionEnd);
                editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));

                string selectedText = editor.SelectedText;

                if (startLine > 0)
                {
                    editor.DeleteRange(editor.Lines[startLine].Position, selectedText.Length + Environment.NewLine.Length);
                    editor.InsertText(editor.Lines[startLine - 1].Position, selectedText + Environment.NewLine);

                    editor.SetSelection(editor.Lines[startLine - 1].Position, editor.Lines[endLine - 1].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
                }
                else
                {
                    editor.InsertText(editor.Lines[endLine].EndPosition, Environment.NewLine);
                    editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - Environment.NewLine.Length);
                }
            }
        }

        public static void MoveLineDown(Scintilla editor)
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                int startLine = editor.LineFromPosition(editor.SelectionStart);
                int endLine = editor.LineFromPosition(editor.SelectionEnd);
                editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));

                string selectedText = editor.SelectedText;

                if (endLine < editor.Lines.Count - 1)
                {
                    editor.DeleteRange(editor.Lines[startLine].Position, selectedText.Length + Environment.NewLine.Length);
                    editor.InsertText(editor.Lines[endLine - (endLine - startLine)].EndPosition, (editor.Lines[endLine - (endLine - startLine)].Text == string.Empty ? Environment.NewLine : string.Empty) + selectedText + Environment.NewLine);

                    editor.SetSelection(editor.Lines[startLine + 1].Position, editor.Lines[endLine + 1].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
                }
                else
                {
                    editor.InsertText(editor.Lines[startLine].Position, Environment.NewLine);
                    editor.SetSelection(editor.Lines[startLine].Position + Environment.NewLine.Length, editor.Lines[endLine + 1].EndPosition);
                }
            }
        }

        public static void SetupSqlEditorStyle(Scintilla editor)
        {
            editor.LexerName = "sql";

            editor.StyleClearAll();
            editor.CaretLineBackColor = Color.FromArgb(35, 35, 35);
            editor.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
            editor.Styles[Style.Default].Font = "Consolas";
            editor.Styles[Style.Default].Size = 10;
            editor.Styles[Style.Default].Bold = true;
            editor.Margins[0].Width = 25;
            editor.Margins[1].Width = 8;

            // Set SQL syntax highlighting styles similar to Visual Studio Dark Theme
            editor.Styles[Style.Sql.Default].ForeColor = Color.FromArgb(240, 240, 240); // Almost white
            editor.Styles[Style.Sql.Comment].ForeColor = Color.FromArgb(100, 100, 100); // Gray
            editor.Styles[Style.Sql.CommentLine].ForeColor = Color.FromArgb(100, 100, 100); // Gray
            editor.Styles[Style.Sql.CommentDoc].ForeColor = Color.FromArgb(100, 100, 100); // Gray
            editor.Styles[Style.Sql.Number].ForeColor = Color.FromArgb(214, 157, 133); // Orange
            editor.Styles[Style.Sql.Word].ForeColor = Color.FromArgb(86, 156, 214); // Blue
            editor.Styles[Style.Sql.Word2].ForeColor = Color.FromArgb(86, 156, 214); // Blue
            editor.Styles[Style.Sql.String].ForeColor = Color.FromArgb(181, 220, 168); // Light green
            editor.Styles[Style.Sql.Character].ForeColor = Color.FromArgb(181, 220, 168); // Light green
            editor.Styles[Style.Sql.Operator].ForeColor = Color.FromArgb(240, 240, 240); // Almost white
            editor.Styles[Style.Sql.Identifier].ForeColor = Color.FromArgb(240, 240, 240); // Almost white
            editor.Styles[Style.LineNumber].ForeColor = Color.FromArgb(100, 100, 100); // Gray
            editor.Styles[Style.BraceLight].ForeColor = Color.Yellow;
            editor.Styles[Style.BraceBad].ForeColor = Color.LightGoldenrodYellow;

            editor.Styles[Style.Sql.Default].BackColor = Color.FromArgb(30, 30, 30);
            editor.Styles[Style.Sql.Comment].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.CommentLine].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.CommentDoc].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.Number].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.Word].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.Word2].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.String].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.Character].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.Operator].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.Sql.Identifier].BackColor = editor.Styles[Style.Default].BackColor;
            editor.Styles[Style.LineNumber].BackColor = Color.FromArgb(15, 15, 15); ;
            editor.Styles[Style.BraceLight].BackColor = Color.DarkGray;
            editor.Styles[Style.BraceBad].BackColor = Color.Red;

            editor.Styles[Style.Sql.Word].Case = StyleCase.Upper;
            editor.Styles[Style.Sql.Word2].Case = StyleCase.Upper;

            editor.Styles[Style.Sql.Default].Bold = true;
            editor.Styles[Style.Sql.Comment].Bold = true;
            editor.Styles[Style.Sql.CommentLine].Bold = true;
            editor.Styles[Style.Sql.CommentDoc].Bold = true;
            editor.Styles[Style.Sql.Number].Bold = true;
            editor.Styles[Style.Sql.Word].Bold = true;
            editor.Styles[Style.Sql.Word2].Bold = true;
            editor.Styles[Style.Sql.String].Bold = true;
            editor.Styles[Style.Sql.Character].Bold = true;
            editor.Styles[Style.Sql.Operator].Bold = true;
            editor.Styles[Style.Sql.Identifier].Bold = true;
            editor.Styles[Style.BraceLight].Bold = true;
            editor.Styles[Style.BraceBad].Bold = true;

            // Set SQL keywords
            editor.SetKeywords(0, FileManager.SqlKeywords);
        }

        public static void SetupSqlEditor(Scintilla editor)
        {
            editor.InsertCheck += Editor_InsertCheck;
            editor.TextChanged += Editor_TextChanged;
            editor.DragEnter += Editor_DragEnter;
            editor.DragDrop += Editor_DragDrop;
            editor.DragOver += Editor_DragOver;
            editor.KeyUp += Editor_KeyUp;
            editor.KeyDown += Editor_KeyDown;
            editor.KeyPress += Editor_KeyPress;

            SetupSqlEditorStyle(editor);

            editor.ClearCmdKey(Keys.Alt | Keys.Up);
            editor.ClearCmdKey(Keys.Alt | Keys.Down);
            editor.ClearCmdKey(Keys.Alt | Keys.F);
            editor.ClearCmdKey(Keys.Control | Keys.F);
            editor.ClearCmdKey(Keys.Control | Keys.G);
            editor.ClearCmdKey(Keys.Control | Keys.H);
            editor.ClearCmdKey(Keys.Control | Keys.Q);
            editor.ClearCmdKey(Keys.Control | Keys.R);
            editor.ClearCmdKey(Keys.Control | Keys.W);
            editor.ClearCmdKey(Keys.Control | Keys.Divide);
            editor.ClearCmdKey(Keys.Control | Keys.Oem2);
            editor.ClearCmdKey(Keys.Control | Keys.OemMinus);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.A);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.B);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.F);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.R);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.S);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.V);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.Divide);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.Oem2);

            ContextMenu cm = editor.ContextMenu ?? new ContextMenu();

            MenuItem copyCMI = new MenuItem("Copy", (o, e) => { editor.Copy(); });
            MenuItem pasteCMI = new MenuItem("Paste", (o, e) => { editor.Paste(); });
            MenuItem pasteClipboardRangeCMI = new MenuItem("Paste rng from clipboard", (o, e) => { PasteFromClipboard(editor); });
            MenuItem formatToSqlCMI = new MenuItem("Format to SQL", (o, e) => { ReformatTextToSqlFilter(editor); });
            MenuItem toggleWrapModeCMI = new MenuItem("Toggle text wrap mode (Ctrl + W)", (o, e) => { ToggleTextWrapModeScintilla(editor); });
            cm.MenuItems.Add(pasteCMI);
            cm.MenuItems.Add(copyCMI);
            cm.MenuItems.Add(pasteClipboardRangeCMI);
            cm.MenuItems.Add(formatToSqlCMI);
            cm.MenuItems.Add(toggleWrapModeCMI);
            editor.ContextMenu = cm;

            // Set up an indicator for highlighting words starting with ':::'
            int indicatorIndex = 8; // Choose an unused indicator index
            editor.Indicators[indicatorIndex].Style = IndicatorStyle.RoundBox;
            editor.Indicators[indicatorIndex].ForeColor = Color.Aquamarine;
            editor.Indicators[indicatorIndex].Alpha = 45;
            editor.Indicators[indicatorIndex].OutlineAlpha = 120;
            editor.Indicators[indicatorIndex].Under = true; // Set to true to highlight under the text

            // Set up an indicator for highlighting words case sensitive
            int indicatorIndexForSelection = 9; // Choose an unused indicator index
            editor.Indicators[indicatorIndexForSelection].Style = IndicatorStyle.RoundBox;
            editor.Indicators[indicatorIndexForSelection].ForeColor = Color.GreenYellow;
            editor.Indicators[indicatorIndexForSelection].Alpha = 40;
            editor.Indicators[indicatorIndexForSelection].OutlineAlpha = 110;
            editor.Indicators[indicatorIndexForSelection].Under = true; // Set to true to highlight under the text

            // Set up an indicator for highlighting matching brackets
            int indicatorIndexForBrackets = 10; // Choose an unused indicator index
            editor.Indicators[indicatorIndexForBrackets].Style = IndicatorStyle.PointCharacter;
            editor.Indicators[indicatorIndexForBrackets].ForeColor = Color.OrangeRed;
            editor.Indicators[indicatorIndexForBrackets].Alpha = 255;
            //editor.Indicators[indicatorIndexForBrackets].OutlineAlpha = 120;
            //editor.Indicators[indicatorIndexForBrackets].Under = true; // Set to true to highlight under the text 

            editor.UpdateUI += (sender, e) =>
             {
                 int position, matchPos, currentChar;
                 switch (e.Change)
                 {
                     case UpdateChange.Content:
                         foreach (var ind in editor.Indicators.Select(p => p.Index).Where(p => p != FindReplace.IndicatorIndex))
                         {
                             editor.IndicatorCurrent = ind;
                             editor.IndicatorClearRange(0, editor.TextLength);
                         }
                         //editor.IndicatorClearRange(0, editor.TextLength);
                         HighlightVariables(editor, indicatorIndex);
                         position = editor.CurrentPosition;
                         matchPos = editor.BraceMatch(position);
                         currentChar = editor.GetCharAt(position);
                         if (matchPos != Scintilla.InvalidPosition)
                             editor.BraceHighlight(position, matchPos); // Highlight matching brackets
                         else if (currentChar == '(' || currentChar == ')')
                         {
                             editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                             editor.BraceBadLight(position); // Highlight if there's a mismatched bracket
                         }
                         else
                             editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                         break;

                     case UpdateChange.Selection:
                         foreach (var ind in editor.Indicators.Select(p => p.Index).Where(p => p != FindReplace.IndicatorIndex))
                         {
                             editor.IndicatorCurrent = ind;
                             editor.IndicatorClearRange(0, editor.TextLength);
                         }
                         //editor.IndicatorClearRange(0, editor.TextLength);
                         HighlightVariables(editor, indicatorIndex);
                         if (editor.TextLength < 50000) // to avoid slowness
                             HighlightCustomWords(editor, indicatorIndexForSelection, editor.SelectedText);

                         position = editor.CurrentPosition;
                         matchPos = editor.BraceMatch(position);
                         currentChar = editor.GetCharAt(position);
                         if (matchPos != Scintilla.InvalidPosition)
                             editor.BraceHighlight(position, matchPos); // Highlight matching brackets
                         else if (currentChar == '(' || currentChar == ')')
                         {
                             editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                             editor.BraceBadLight(position); // Highlight if there's a mismatched bracket
                         }
                         else
                             editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);

                         //HighlightMatchingBrackets(editor, indicatorIndexForBrackets);
                         break;
                 }
             };

            // Add this in your constructor or initialization function
            editor.CharAdded += Editor_CharAdded;
        }

        private static void Editor_CharAdded(object sender, CharAddedEventArgs e)
        {

            // Get the char that was just added
            Scintilla editor = sender as Scintilla;
            char addedChar = (char)e.Char;

            switch (addedChar)
            {
                case '(':
                    InsertMatchingBracket(editor, addedChar, ')');
                    break;
                case '[':
                    InsertMatchingBracket(editor, addedChar, ']');
                    break;
                case '"':
                    InsertMatchingBracket(editor, addedChar, '"');
                    break;
                case '\'':
                    InsertMatchingBracket(editor, addedChar, '\'');
                    break;
                case ')':
                    SkipClosingBracket(editor, '(', addedChar);
                    break;
                case ']':
                    SkipClosingBracket(editor, '[', addedChar);
                    break;
            }
        }

        private static void ToggleTextWrapModeScintilla(Scintilla editor)
        {
            if (editor.WrapMode == ScintillaNET.WrapMode.None) editor.WrapMode = ScintillaNET.WrapMode.Word; else editor.WrapMode = ScintillaNET.WrapMode.None;
        }

        public static void SetupSqlEditorReadOnly(Scintilla editor)
        {
            editor.InsertCheck += Editor_InsertCheck;
            editor.TextChanged += Editor_TextChanged;
            editor.KeyUp += Editor_KeyUp;

            SetupSqlEditorStyle(editor);
            editor.ReadOnly = true;

            editor.ClearCmdKey(Keys.Control | Keys.F);
            editor.ClearCmdKey(Keys.Control | Keys.W);

            ContextMenu cm = editor.ContextMenu ?? new ContextMenu();
            MenuItem copyCMI = new MenuItem("Copy", (o, e) => { editor.Copy(); });
            MenuItem toggleWrapModeCMI = new MenuItem("Toggle text wrap mode (Ctrl + W)", (o, e) => { ToggleTextWrapModeScintilla(editor); });
            cm.MenuItems.Add(copyCMI);
            cm.MenuItems.Add(toggleWrapModeCMI);
            editor.ContextMenu = cm;

            // Set up an indicator for highlighting words starting with ':::'
            int indicatorIndex = 8; // Choose an unused indicator index
            editor.Indicators[indicatorIndex].Style = IndicatorStyle.RoundBox;
            editor.Indicators[indicatorIndex].ForeColor = Color.Aquamarine;
            editor.Indicators[indicatorIndex].Alpha = 45;
            editor.Indicators[indicatorIndex].OutlineAlpha = 120;
            editor.Indicators[indicatorIndex].Under = true; // Set to true to highlight under the text

            // Set up an indicator for highlighting words case sensitive
            int indicatorIndexForSelection = 9; // Choose an unused indicator index
            editor.Indicators[indicatorIndexForSelection].Style = IndicatorStyle.RoundBox;
            editor.Indicators[indicatorIndexForSelection].ForeColor = Color.GreenYellow;
            editor.Indicators[indicatorIndexForSelection].Alpha = 40;
            editor.Indicators[indicatorIndexForSelection].OutlineAlpha = 110;
            editor.Indicators[indicatorIndexForSelection].Under = true; // Set to true to highlight under the text

            // Set up an indicator for highlighting matching brackets
            int indicatorIndexForBrackets = 10; // Choose an unused indicator index
            editor.Indicators[indicatorIndexForBrackets].Style = IndicatorStyle.PointCharacter;
            editor.Indicators[indicatorIndexForBrackets].ForeColor = Color.OrangeRed;
            editor.Indicators[indicatorIndexForBrackets].Alpha = 255;
            //editor.Indicators[indicatorIndexForBrackets].OutlineAlpha = 120;
            //editor.Indicators[indicatorIndexForBrackets].Under = true; // Set to true to highlight under the text 

            editor.UpdateUI += (sender, e) =>
            {
                int position, matchPos, currentChar;
                switch (e.Change)
                {
                    case UpdateChange.Content:
                        foreach (var ind in editor.Indicators.Select(p => p.Index).Where(p => p != FindReplace.IndicatorIndex))
                        {
                            editor.IndicatorCurrent = ind;
                            editor.IndicatorClearRange(0, editor.TextLength);
                        }
                        //editor.IndicatorClearRange(0, editor.TextLength);
                        HighlightVariables(editor, indicatorIndex);
                        position = editor.CurrentPosition;
                        matchPos = editor.BraceMatch(position);
                        currentChar = editor.GetCharAt(position);
                        if (matchPos != Scintilla.InvalidPosition)
                            editor.BraceHighlight(position, matchPos); // Highlight matching brackets
                        else if (currentChar == '(' || currentChar == ')')
                        {
                            editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                            editor.BraceBadLight(position); // Highlight if there's a mismatched bracket
                        }
                        else
                            editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                        break;

                    case UpdateChange.Selection:
                        foreach (var ind in editor.Indicators.Select(p => p.Index).Where(p => p != FindReplace.IndicatorIndex))
                        {
                            editor.IndicatorCurrent = ind;
                            editor.IndicatorClearRange(0, editor.TextLength);
                        }
                        //editor.IndicatorClearRange(0, editor.TextLength);
                        HighlightVariables(editor, indicatorIndex);
                        if (editor.TextLength < 50000) // to avoid slowness
                            HighlightCustomWords(editor, indicatorIndexForSelection, editor.SelectedText);

                        position = editor.CurrentPosition;
                        matchPos = editor.BraceMatch(position);
                        currentChar = editor.GetCharAt(position);
                        if (matchPos != Scintilla.InvalidPosition)
                            editor.BraceHighlight(position, matchPos); // Highlight matching brackets
                        else if (currentChar == '(' || currentChar == ')')
                        {
                            editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                            editor.BraceBadLight(position); // Highlight if there's a mismatched bracket
                        }
                        else
                            editor.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);

                        //HighlightMatchingBrackets(editor, indicatorIndexForBrackets);
                        break;
                }
            };
        }

        public static void InitializeScintillaAutocomplete(Scintilla editor)
        {
            // Set the separator character for keyword lists
            editor.AutoCSeparator = ' '; // Use space as the separator in your keyword strings
            editor.AutoCOrder = Order.Presorted;//.Custom;

            // Set autocompletion to be case-insensitive (common for SQL)
            editor.AutoCIgnoreCase = true;

            // Prevent autocompletion from cancelling if you type characters at the *beginning* of the word
            editor.AutoCCancelAtStart = false;

            // When an item is selected, automatically delete the rest of the word being typed
            editor.AutoCDropRestOfWord = true;
            editor.AutoCCancelAtStart = false;
            editor.AutoCMaxHeight = 9;
        }

        private static void Editor_TextChanged(object sender, EventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            string text = editor.Text.UnifyLineEndings();
            if (editor.Text == text)
                return;
            else
                editor.Text = text ?? string.Empty;
        }

        private static void Editor_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            string text = e.Text.UnifyLineEndings();
            if (e.Text == text)
                return;
            else
                e.Text = text ?? string.Empty;
        }

        private static void InsertMatchingBracket(Scintilla editor, char addedChar, char closingChar)
        {
            int currentPos = editor.CurrentPosition;
            bool isNotBracket = new char[] { '"', '\'' }.Contains(closingChar);

            if (!isNotBracket)
            {
                editor.InsertText(currentPos, addedChar.ToString());
                if (editor.BraceMatch(currentPos) != Scintilla.InvalidPosition)
                {
                    editor.DeleteRange(currentPos - 1, 1);
                    editor.GotoPosition(currentPos);
                    return;
                }
                editor.DeleteRange(currentPos - 1, 1);
            }

            editor.InsertText(currentPos, closingChar.ToString());
            editor.GotoPosition(currentPos);
        }

        private static void Editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            if (e.KeyChar == (char)Keys.Return)
                IndentAfterReturn(editor);
        }

        private static void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            {
                if (e.Alt || e.KeyCode == Keys.Alt)
                    if (e.KeyCode == Keys.Up)
                    {
                        MoveLineUp(editor);
                        editor.ScrollCaret();
                        e.Handled = true;
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        MoveLineDown(editor);
                        editor.ScrollCaret();
                        e.Handled = true;
                    }
            }
        }


        private static void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            if (e.Control)
            {
                if (e.Shift)
                {
                    if (e.KeyCode == Keys.Divide || e.KeyCode == Keys.Oem2)
                    {
                        Comment(editor);
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.B)
                    {
                        WrapIntoSqlBlock(editor);
                        editor.Focus();
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.S)
                    {
                        ToggleSpacesAndNewLines(editor);
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.A)
                    {
                        SelectBlock(editor);
                        e.SuppressKeyPress = true;
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.H || e.KeyCode == Keys.F)
                    {
                        FindReplace findReplace = new FindReplace(editor);
                        if (e.KeyCode == Keys.F)
                            findReplace.ShowFind();
                        else
                            findReplace.ShowReplace();
                        findReplace.Window.FormClosed += (o, ea) => editor.MultipleSelection = false;
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.OemMinus)
                    {
                        editor.ReplaceSelection($"{Environment.NewLine}{ScintillaSqlQuerySeparator}{Environment.NewLine}");
                        editor.Focus();
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.Q)
                    {
                        ReformatTextToSqlFilter(editor);
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.W)
                    {
                        ToggleTextWrapModeScintilla(editor);
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.G)
                    {
                        RemoveSqlAliases(editor);
                        e.SuppressKeyPress = true;
                    }
                }
            }
        }

        public static void RemoveSqlAliases(Scintilla editor)
        {
            // Get the selected text
            string text = editor.SelectedText;
            if (string.IsNullOrWhiteSpace(text))
                return;

            // Record the starting position of the selection
            int startPos = editor.SelectionStart;

            // Process the text to remove aliases
            string processedText = Utils.RemoveSqlAliasesFromText(text);

            // Replace the selection with the processed text
            editor.ReplaceSelection(processedText);

            // Update the selection to cover the new text
            int endPos = startPos + processedText.Length;
            editor.SetSelection(startPos, endPos);
        }

        private static void SkipClosingBracket(Scintilla editor, char openingChar, char closingChar)
        {
            int currentPos = editor.CurrentPosition;
            int nextChar = editor.GetCharAt(currentPos - 1);
            int previousChar = editor.GetCharAt(currentPos - 2);

            if (nextChar == closingChar && previousChar == openingChar && editor.BraceMatch(currentPos - 2) != Scintilla.InvalidPosition)
            {
                editor.DeleteRange(currentPos - 1, 1);
                editor.GotoPosition(currentPos);
            }
        }

        private static void HighlightMatchingBrackets(Scintilla editor, int indicatorIndexForBrackets)
        {
            if (editor == null || !editor.Indicators.Select(p => p.Index).Contains(indicatorIndexForBrackets))
                return;

            int position = editor.CurrentPosition;

            // Check for an opening or closing bracket at the current position
            if (editor.GetCharAt(position) == '(')
            {
                int matchPos = editor.BraceMatch(position);
                if (matchPos != Scintilla.InvalidPosition)
                {
                    // Highlight the matching brackets
                    editor.IndicatorCurrent = indicatorIndexForBrackets;
                    editor.IndicatorFillRange(position, 1); // Highlight the opening bracket
                    editor.IndicatorFillRange(matchPos, 1); // Highlight the matching closing bracket
                }
            }
            else if (editor.GetCharAt(position) == ')')
            {
                int matchPos = editor.BraceMatch(position);
                if (matchPos != Scintilla.InvalidPosition)
                {
                    // Highlight the matching brackets
                    editor.IndicatorCurrent = indicatorIndexForBrackets;
                    editor.IndicatorFillRange(position, 1); // Highlight the closing bracket
                    editor.IndicatorFillRange(matchPos, 1); // Highlight the matching opening bracket
                }
            }
        }

        private static void PasteFromClipboard(Scintilla editor)
        {
            string text = Clipboard.GetText(TextDataFormat.Text);
            if (string.IsNullOrWhiteSpace(text))
                return;

            ReformatTextToSqlFilter(editor, text);
        }
        private static void HighlightVariables(Scintilla editor, int indicatorIndex)
        {
            if (editor == null || !editor.Indicators.Select(p => p.Index).Contains(indicatorIndex))
                return;

            // Define the regex pattern to match words that start with ":::" and are not connected to other characters
            string pattern = @"(?<!\S):::\w+";
            var matches = Regex.Matches(editor.Text, pattern);
            foreach (Match match in matches)
            {
                // Apply the indicator to the matched range
                foreach (var ind in editor.Indicators.Select(p => p.Index).Where(p => p != indicatorIndex))
                {
                    editor.IndicatorCurrent = ind;
                    editor.IndicatorClearRange(match.Index, match.Length);
                }
                editor.IndicatorCurrent = indicatorIndex;
                editor.IndicatorFillRange(match.Index, match.Length);
            }
        }

        private static void HighlightCustomWords(Scintilla editor, int indicatorIndex, string search, string regexPatternL = @"(?i)", string regexPatternR = "")
        {
            if (editor == null || search == null || !editor.Indicators.Select(p => p.Index).Contains(indicatorIndex))
                return;

            // Define the regex pattern to match words that start with ":::" and are not connected to other characters
            string pattern = regexPatternL + Regex.Escape(search) + regexPatternR;
            var matches = Regex.Matches(editor.Text, pattern);
            foreach (Match match in matches)
            {
                // Apply the indicator to the matched range except selection
                if (editor.SelectionStart == match.Index)
                    continue;
                editor.IndicatorCurrent = indicatorIndex;
                editor.IndicatorFillRange(match.Index, match.Length);
            }
        }

        private static Point? m_lastLineStart = null;
        private static Point? m_lastLineEnd = null;

        private static void Editor_DragOver(object sender, DragEventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            if (Control.ModifierKeys == Keys.Control)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
            Point point = editor.PointToClient(new Point(e.X, e.Y));
            int insertPos = editor.CharPositionFromPoint(point.X, point.Y);

            Point posLocation = new Point(editor.PointXFromPosition(insertPos), editor.PointYFromPosition(insertPos));

            // Convert that position to screen coordinates.
            Point screenPoint = editor.PointToScreen(posLocation);
            Point newLineStart = new Point(screenPoint.X, screenPoint.Y + 15);
            Point newLineEnd = new Point(screenPoint.X, screenPoint.Y - 5);

            // If a line was previously drawn, erase it by drawing it again.
            if (m_lastLineStart.HasValue && m_lastLineEnd.HasValue)
                ControlPaint.DrawReversibleLine(m_lastLineStart.Value, m_lastLineEnd.Value, Color.White);

            ControlPaint.DrawReversibleLine(newLineStart, newLineEnd, Color.White);

            // Update the stored endpoints.
            m_lastLineStart = newLineStart;
            m_lastLineEnd = newLineEnd;

            var line = editor.Lines[editor.LineFromPosition(insertPos)];
            if (editor.Selections.Count <= 1)
                editor.AddSelection(line.Position, line.EndPosition);

            editor.Selections[1].Caret = line.Position;
            editor.Selections[1].Anchor = line.EndPosition;

            editor.MainSelection = 0;
        }

        private static void Editor_DragEnter(object sender, DragEventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            if (Control.ModifierKeys == Keys.Control)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
            editor.MultipleSelection = true;
            editor.SelectionAdditionalBackColor = Color.FromArgb(60, 60, 60);
        }

        private static void Editor_DragDrop(object sender, DragEventArgs e)
        {
            Scintilla editor = sender as Scintilla;
            // Erase the reversible line if it exists.
            if (m_lastLineStart.HasValue && m_lastLineEnd.HasValue)
            {
                ControlPaint.DrawReversibleLine(m_lastLineStart.Value, m_lastLineEnd.Value, Color.White);
                m_lastLineStart = null;
                m_lastLineEnd = null;
            }

            Point point = editor.PointToClient(new Point(e.X, e.Y));
            int insertPos = editor.CharPositionFromPoint(point.X, point.Y);
            var selection = editor.Selections[editor.MainSelection];
            int startSelection = selection.Start;
            int endSelection = selection.End;
            string selectedText = editor.GetTextRange(startSelection, endSelection - startSelection);

            // Insert the selected text at the new position
            editor.InsertText(insertPos, selectedText);
            editor.MultipleSelection = false;

            if (Control.ModifierKeys == Keys.Control)
                return;

            // If the new position is before the original position adjust the original position
            if (insertPos < startSelection)
            {
                startSelection += selectedText.Length;
                endSelection += selectedText.Length;
            }

            // Remove the selected text from the original position
            editor.DeleteRange(startSelection, endSelection - startSelection);
        }
    }
}

