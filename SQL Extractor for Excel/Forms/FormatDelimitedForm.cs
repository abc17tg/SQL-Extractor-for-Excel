using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class FormatDelimitedForm : Form
    {
        public List<string> FormattedTexts = new List<string>();
        public List<string> FormattedValues = new List<string>();
        public string FormattedText => FormattedTexts.Count > 0 ? string.Join(Environment.NewLine, FormattedTexts) : string.Empty;
        public Excel.Range SelectedRange;

        private List<TextBox> m_textBoxes;
        private bool m_updates = true;
        private Excel.Window m_window;

        public FormatDelimitedForm(Excel.Range rng)
        {
            InitializeComponent();
            SelectedRange = rng;
            m_textBoxes = tableLayoutPanel.Controls.OfType<TextBox>().ToList();
            m_textBoxes.ForEach(p => p.TextChanged += (o, s) => UpdateExample());
            m_textBoxes.ForEach(p => p.TextChanged += (o, s) => Format());
            rng.Application.SheetSelectionChange += SelectionChanged;
            rng.Application.SheetActivate += (o) => SelectionChanged(o, rng.Application.ActiveWindow.RangeSelection.GetUsableRange());

            FormClosed += (oo, s) =>
            {
                rng.Application.SheetSelectionChange -= SelectionChanged;
                rng.Application.SheetActivate -= (o) => SelectionChanged(o, rng.Application.ActiveWindow.RangeSelection.GetUsableRange());
            };

            if (rng.Valid())
            {
                Format();
                dividerNumericUpDown.Maximum = Math.Max(FormattedValues.Count, dividerNumericUpDown.Minimum);
                UpdateCount();
            }
        }

        public void SelectionChanged(object sender, Excel.Range rng)
        {
            if (!m_updates || !rng.Valid())
                return;

            if (Control.ModifierKeys == Keys.Shift && rng == SelectedRange)
                return;

            SelectedRange = rng;
            foreach (Excel.Window window in SelectedRange.Application.Windows)
            {
                if (window.ActiveSheet.Parent.Name == SelectedRange.Worksheet.Parent.Name)
                    m_window = window;
            }

            if (rng.Valid())
            {
                Format();
                dividerNumericUpDown.Maximum = Math.Max(FormattedValues.Count, dividerNumericUpDown.Minimum);
            }
            else
            {
                dividerNumericUpDown.Maximum = 1;
                FormattedTexts = new List<string>();
                FormattedValues = new List<string>();
            }

            UpdateCount();
        }

        private void UpdateCount()
        {
            if (SelectedRange.Valid())
                countLabel.Text = dividerNumericUpDown.Value > 1 ? $"Count per part: {(FormattedValues.Count / dividerNumericUpDown.Value).ToString("0.##")}" : $"Count: {FormattedValues.Count}";
            else
                countLabel.Text = "Not valid range";
        }

        private void UpdateExample()
        {
            if (SelectedRange.Valid())
                exampleLabel.Text = $"{startTextBox.Text}{prependTextBox.Text}value1{appendTextBox.Text}{delimiterTextBox.Text}{prependTextBox.Text}value2{appendTextBox.Text}{delimiterTextBox.Text} ... {endTextBox.Text}";
            else
                exampleLabel.Text = "Not valid range";
        }

        private void Format()
        {
            if (!m_updates)
                return;

            FormattedValues = SelectedRange.Cells.Cast<Excel.Range>().Select(p => ((object)p.Value2)?.ToString() ?? "").Where(p => !string.IsNullOrEmpty(p)).ToList();

            if (uniqueValuesCheckBox.Checked)
                FormattedValues = FormattedValues.Distinct().ToList();

            FormattedValues = FormattedValues.Select(p => $"{prependTextBox.Text}{p}{appendTextBox.Text}").ToList();

            FormattedTexts = dividerNumericUpDown.Value == 1 ? new List<string> { $"{startTextBox.Text}{string.Join(delimiterTextBox.Text, FormattedValues)}{endTextBox.Text}" } : FormattedValues.Split((int)dividerNumericUpDown.Value).Select(p => $"{startTextBox.Text}{string.Join(delimiterTextBox.Text, p)}{endTextBox.Text}").ToList();
        }

        private void toSqlFormatButton_Click(object sender, EventArgs e)
        {
            m_updates = false;

            startTextBox.Text = "(";
            prependTextBox.Text = "'";
            delimiterTextBox.Text = ", ";
            appendTextBox.Text = prependTextBox.Text;
            endTextBox.Text = ")";

            m_updates = true;
        }

        private void addBracketsButton_Click(object sender, EventArgs e)
        {
            m_updates = false;

            startTextBox.Text = "(";
            endTextBox.Text = ")";

            m_updates = true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FormattedTexts.FirstOrDefault()))
            {
                DialogResult = DialogResult.OK;
                Clipboard.SetText(FormattedText);
            }
            else
                DialogResult = DialogResult.Abort;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void startTextBox_TextChanged(object sender, EventArgs e)
        {
            m_updates = false;
            switch (startTextBox.Text)
            {
                case "(":
                    endTextBox.Text = ")";
                    break;
                case "{":
                    endTextBox.Text = "}";
                    break;
                case "[":
                    endTextBox.Text = "]";
                    break;
                case "<":
                    endTextBox.Text = ">";
                    break;
                default:
                    endTextBox.Text = startTextBox.Text;
                    break;
            }
            m_updates = true;
        }

        private void prependTextBox_TextChanged(object sender, EventArgs e)
        {
            m_updates = false;
            switch (prependTextBox.Text)
            {
                case "(":
                    appendTextBox.Text = ")";
                    break;
                case "{":
                    appendTextBox.Text = "}";
                    break;
                case "[":
                    appendTextBox.Text = "]";
                    break;
                case "<":
                    appendTextBox.Text = ">";
                    break;
                default:
                    appendTextBox.Text = prependTextBox.Text;
                    break;
            }
            m_updates = true;
        }

        private void uniqueValuesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Format();
            UpdateCount();
        }

        private void dividerNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Format();
            UpdateCount();
        }

        private void FormatDelimitedForm_Activated(object sender, EventArgs e)
        {
            Utils.EnsureWindowIsVisible(this);
        }
    }
}
