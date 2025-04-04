using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class EditSqlVariableForm : Form
    {
        public string VariableName;
        public string VariableOrigName;
        public List<string> UnformattedValues = new List<string>();
        public List<string> FormattedValues => uniqueValuesCheckBox.Checked ? UnformattedValues.Distinct().Select(p => $"{prependTextBox.Text}{p}{appendTextBox.Text}").ToList() : UnformattedValues.Select(p => $"{prependTextBox.Text}{p}{appendTextBox.Text}").ToList();
        public List<string> FormattedTexts()
        {
            List<string> formattedValues = FormattedValues;
            return dividerNumericUpDown.Value == 1 ? new List<string> { $"{startTextBox.Text}{string.Join(delimiterTextBox.Text, formattedValues)}{endTextBox.Text}" } : formattedValues.Split((int)dividerNumericUpDown.Value).Select(p => $"{startTextBox.Text}{string.Join(delimiterTextBox.Text, p)}{endTextBox.Text}").ToList();
        }
        public string FormattedText => UnformattedValues.Count > 0 ? string.Join(Environment.NewLine, FormattedTexts()) : string.Empty;



        //public Excel.Range SelectedRange;

        private Excel.Application m_app;
        private List<TextBox> m_textBoxes;
        private bool m_updates = true;
        private Excel.Window m_window;


        public EditSqlVariableForm(string name, Excel.Application app, List<string> values = null)
        {
            InitializeComponent();
            m_app = app;
            VariableOrigName = name;
            VariableName = name;
            if (values != null)
                UnformattedValues = values;

            m_textBoxes = tableLayoutPanel.Controls.OfType<TextBox>().ToList();
            //m_textBoxes.ForEach(p => p.TextChanged += (o, s) => UpdateExample());
            m_textBoxes.ForEach(p => p.TextChanged += (o, s) => Format());
            //rng.Application.SheetSelectionChange += SelectionChanged;
            //rng.Application.SheetActivate += (o) => SelectionChanged(o, rng.Application.ActiveWindow.RangeSelection.GetUsableRange());
        }

        private void UpdateCount()
        {
            if (UnformattedValues != null)
                countLabel.Text = dividerNumericUpDown.Value > 1 ? $"Count per part: {(UnformattedValues.Count / dividerNumericUpDown.Value).ToString("0.##")}" : $"Count: {UnformattedValues.Count}";
            else
                countLabel.Text = "Count: -";
        }

        private void Fetch()
        {
            Excel.Range rng = m_app.ActiveWindow.RangeSelection.GetUsableRange();

            if (rng.Valid())
            {
                UnformattedValues = rng.Cells.Cast<Excel.Range>().Select(p => ((object)p.Value2)?.ToString() ?? "").Where(p => !string.IsNullOrEmpty(p)).ToList();
                dividerNumericUpDown.Maximum = Math.Max(UnformattedValues.Count, dividerNumericUpDown.Minimum);
            }
            else
            {
                dividerNumericUpDown.Maximum = 1;
                UnformattedValues = new List<string>();
            }

            UpdateCount();
        }

        private void Add()
        {
            Excel.Range rng = m_app.ActiveWindow.RangeSelection.GetUsableRange();

            if (rng.Valid())
            {
                UnformattedValues.AddRange(rng.Cells.Cast<Excel.Range>().Select(p => ((object)p.Value2)?.ToString() ?? "").Where(p => !string.IsNullOrEmpty(p)));
                dividerNumericUpDown.Maximum = Math.Max(UnformattedValues.Count, dividerNumericUpDown.Minimum);
            }
            else if (UnformattedValues.Count < 1)
            {
                dividerNumericUpDown.Maximum = 1;
                UnformattedValues = new List<string>();
            }
            else
                return;

            UpdateCount();
        }

        private void Format()
        {
            /*if (!m_updates || UnformattedValues == null || UnformattedValues.Count < 1)
                return;


            string text = valuesRichTextBox.Text;
            List<char> DelimiterChars = new List<char> { ' ', @"'"[0], '(', ')', ',', '.', '\t', '\n', '\r', ';', '|' };
            UnformattedValues = text.Split(DelimiterChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

            if (uniqueValuesCheckBox.Checked)
                UnformattedValues = UnformattedValues.Distinct().ToList();

            FormattedValues = UnformattedValues.Select(p => $"{prependTextBox.Text}{p}{appendTextBox.Text}").ToList();

            FormattedTexts = dividerNumericUpDown.Value == 1 ? new List<string> { $"{startTextBox.Text}{string.Join(delimiterTextBox.Text, FormattedValues)}{endTextBox.Text}" } : FormattedValues.Split((int)dividerNumericUpDown.Value).Select(p => $"{startTextBox.Text}{string.Join(delimiterTextBox.Text, p)}{endTextBox.Text}").ToList();

            valuesRichTextBox.Text = string.Join(Environment.NewLine, FormattedTexts);*/
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
/*            if (!string.IsNullOrEmpty(FormattedTexts.FirstOrDefault()))
            {
                DialogResult = DialogResult.OK;
                Clipboard.SetText(FormattedText);
            }
            else
                DialogResult = DialogResult.Abort;*/
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

        private void renameButton_Click(object sender, EventArgs e)
        {
            switch (renameButton.Text)
            {
                case "Rename":
                    renameButton.Text = "Save";

                    break;
                case "Save":

                    renameButton.Text = "Rename";
                    break;
                default:
                    renameButton.Text = "Rename";
                    break;
            };
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void fetchButton_Click(object sender, EventArgs e)
        {
            Fetch();
        }

        private void EditSqlVariableForm_Activated(object sender, EventArgs e)
        {
            Utils.EnsureWindowIsVisible(this);
        }
    }
}
