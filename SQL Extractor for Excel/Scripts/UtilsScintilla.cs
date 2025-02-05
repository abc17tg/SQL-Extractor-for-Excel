using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;

namespace SQL_Extractor_for_Excel.Scripts
{
    public static class UtilsScintilla
    {
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
                editor.GotoPosition(editor.Lines[currentLine].EndPosition - (editor.Lines[currentLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
            }
        }

        public static void ReformatTextToSql(Scintilla editor, string text = null)
        {
            if (text == null)
                text = editor.SelectedText;
            if (string.IsNullOrWhiteSpace(text))
                return;

            // Regex patterns to detect formats
            var singleQuotedPattern = @"^\(\s*'[^']*'(,\s*'[^']*')*\s*\)$";
            var unquotedPattern = @"^\(\s*[^,']+(,\s*[^,']+)*\s*\)$";

            if (Regex.IsMatch(text, singleQuotedPattern))
            {
                // Format from ('x','x','x',...) to (x, x, x, ...)
                text = text.Trim('(', ')'); // Remove outer parentheses
                text = string.Join(", ", text.Split(',')
                    .Select(p => p.Trim().Trim('\''))); // Remove single quotes and trim
                text = $"({text})";
            }
            else if (Regex.IsMatch(text, unquotedPattern))
            {
                // Format from (x, x, x, ...) to ('x','x','x',...)
                text = text.Trim('(', ')'); // Remove outer parentheses
                text = string.Join(", ", text.Split(',')
                    .Select(p => $"'{p.Trim()}'")); // Add single quotes and trim
                text = $"({text})";
            }
            else
            {
                // If neither format is matched, use the default formatting logic
                List<char> DelimiterChars = new List<char> { ' ', '\'', '(', ')', ',', '\t', '\n', '\r', ';', '|' };
                text = $"({string.Join(", ", text.Split(DelimiterChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => $"'{p.Trim()}'").ToArray())})";
            }

            // Replace the selected text in the editor
            editor.ReplaceSelection(text);
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
                return new string('\t', indentation);
            else
                return new string(lineText.Select(p => char.IsWhiteSpace(p) ? p : ' ').ToArray());

            //int position = scintilla.SelectionStart;
            //int lineNumber = scintilla.LineFromPosition(position);
            //int lineStartPos = scintilla.Lines[lineNumber].Position;

            //if (position == lineStartPos) 
            //    return string.Empty;

            //string lineText = scintilla.GetTextRange(lineStartPos, position - lineStartPos);
            //return new string(lineText.Select(p => char.IsWhiteSpace(p) ? p : ' ').ToArray());
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

        //public static void WrapIntoSqlBlock(Scintilla editor)
        //{
        //    using (new ScintillaPauseUpdatesBlock(editor))
        //    {
        //        int indentation = editor.Lines[editor.CurrentLine].Indentation;
        //        int selStartPos = editor.SelectionStart;
        //        int linesCount = editor.LineFromPosition(editor.SelectionEnd) - editor.LineFromPosition(selStartPos) + 1;
        //        string text = $"(\n{editor.SelectedText}\n)";
        //        editor.ReplaceSelection(text);
        //        editor.Update();
        //        editor.SetSelection(editor.Lines[editor.LineFromPosition(selStartPos) + 1].Position, editor.Lines[editor.LineFromPosition(selStartPos) - 1 + linesCount].EndPosition);
        //        editor.Update();

        //        editor.Lines[editor.LineFromPosition(selStartPos)].Indentation = indentation;
        //        editor.Lines[editor.LineFromPosition(selStartPos) + linesCount + 1].Indentation = indentation;

        //        for (int i = editor.LineFromPosition(editor.SelectionStart); i <= editor.LineFromPosition(editor.SelectionEnd); i++)
        //            editor.Lines[i].Indentation = Math.Max(indentation, editor.Lines[i].Indentation) + editor.TabWidth;
        //    }
        //}

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

        public static void MoveLineUp(Scintilla editor)
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                int startLine = editor.LineFromPosition(editor.SelectionStart);
                int endLine = editor.LineFromPosition(editor.SelectionEnd);
                editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - Environment.NewLine.Length);

                string selectedText = editor.SelectedText;

                if (startLine > 0)
                {
                    editor.DeleteRange(editor.Lines[startLine].Position, selectedText.Length + Environment.NewLine.Length);
                    editor.InsertText(editor.Lines[startLine - 1].Position, selectedText + Environment.NewLine);

                    editor.SetSelection(editor.Lines[startLine - 1].Position, editor.Lines[endLine - 1].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
                }
            }
        }

        public static void MoveLineDown(Scintilla editor)
        {
            using (new ScintillaPauseUpdatesBlock(editor))
            {
                int startLine = editor.LineFromPosition(editor.SelectionStart);
                int endLine = editor.LineFromPosition(editor.SelectionEnd);
                editor.SetSelection(editor.Lines[startLine].Position, editor.Lines[endLine].EndPosition - Environment.NewLine.Length);

                string selectedText = editor.SelectedText;

                if (endLine < editor.Lines.Count - 1)
                {
                    editor.DeleteRange(editor.Lines[startLine].Position, selectedText.Length + Environment.NewLine.Length);
                    editor.InsertText(editor.Lines[endLine - (endLine - startLine)].EndPosition, selectedText + Environment.NewLine);

                    editor.SetSelection(editor.Lines[startLine + 1].Position, editor.Lines[endLine + 1].EndPosition - (editor.Lines[endLine].Text.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0));
                }
            }
        }

        public static void SetupSqlEditor(Scintilla editor)
        {
            editor.InsertCheck += Editor_InsertCheck;
            editor.TextChanged += Editor_TextChanged;
            editor.DragEnter += Editor_DragEnter;
            editor.DragDrop += Editor_DragDrop;
            editor.DragOver += Editor_DragOver;
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

            editor.ClearCmdKey(Keys.Control | Keys.F);
            editor.ClearCmdKey(Keys.Control | Keys.H);
            editor.ClearCmdKey(Keys.Control | Keys.Oem2);
            editor.ClearCmdKey(Keys.Control | Keys.Divide);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.Divide);
            editor.ClearCmdKey(Keys.Shift | Keys.Control | Keys.Oem2);

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
                         foreach (var ind in editor.Indicators.Select(p => p.Index).Where(p=>p != FindReplace.IndicatorIndex))
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
            editor.CharAdded += (sender, e) =>
            {
                // Get the char that was just added
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
            };

            /*            editor.BeforeDelete += (sender, e) =>
                        {
                            if (e.Source != ModificationSource.User)
                                return;

                            if (e.Text == "(" || e.Text == "[")
                            {
                                int currentPos = e.Position;
                                int match = editor.BraceMatch(currentPos);
                                if (match != Scintilla.InvalidPosition && editor.GetTextRange(currentPos + 1, match - currentPos - 1).Trim() == string.Empty)
                                {
                                    editor.DeleteRange(currentPos - 1, match - currentPos + 5);
                                }
                            }
                        };*/

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

