using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class InputRangeForm : Form
    {
        private Excel.Application m_app;
        private Excel.Worksheet m_worksheet;
        public Excel.Range Range;
        private Excel.Window m_window;
        private bool m_modify = true;

        public InputRangeForm(Excel.Application app, string placeholder = null, string title = null)
        {
            InitializeComponent();
            m_app = app;
            m_worksheet = app.ActiveSheet;
            if (!string.IsNullOrEmpty(placeholder))
                rangeTextBox.Text = placeholder;
            if (!string.IsNullOrEmpty(title))
                this.Text = title;
            m_app.SheetSelectionChange += SelectionChanged;
            m_app.SheetActivate += (o) => SelectionChanged(o, m_app.ActiveWindow.RangeSelection);
        }

        public void SelectionChanged(object sender, Excel.Range rng)
        {
            if (!m_modify)
                return;

            if (Control.ModifierKeys == Keys.Shift && rng == Range)
                return;

            Range = rng;
            foreach (Excel.Window window in m_app.Windows)
            {
                if (window.ActiveSheet.Parent.Name == Range.Worksheet.Parent.Name)
                    m_window = window;
            }
            rangeTextBox.Text = $"\'{(rng.Worksheet.Parent as Excel.Workbook).Name}\'.\'{rng.Worksheet.Name}\'.({rng.Address})";
            rangeTextBox.Update();
            okBtn.Focus();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            m_modify = false;
            DialogResult = DialogResult.OK;
            m_app.SheetSelectionChange -= SelectionChanged;
            m_app.SheetActivate -= (o) => SelectionChanged(null, m_app.ActiveWindow.RangeSelection);
            m_worksheet.Activate();
            Close();
        }

        private void InputRangeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control) && m_window != null)
            {
                Utils.SetForegroundWindow(new IntPtr(m_window.Hwnd));
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            m_app.SheetSelectionChange -= SelectionChanged;
            m_app.SheetActivate -= (o) => SelectionChanged(null, m_app.ActiveWindow.RangeSelection);
            DialogResult = DialogResult.Cancel;
            m_worksheet.Activate();
            Close();
        }

        ~InputRangeForm()
        {
            m_app.SheetSelectionChange -= SelectionChanged;
            m_app.SheetActivate -= (o) => SelectionChanged(null, null);
            m_worksheet.Activate();
            Close();
        }

        private void InputRangeForm_Activated(object sender, EventArgs e)
        {
            Utils.EnsureWindowIsVisible(this);
        }
    }
}
