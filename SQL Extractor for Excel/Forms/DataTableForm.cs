using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class DataTableForm : Form
    {
        public DataTable DataTable;
        public string Query;
        Excel.Application ExcelApp;

        private NumberFormatInfo m_nfi;

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 ToggleTopMostMenuItem = 1000;
        public const Int32 CenterFormMenuItem = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);


        public DataTableForm(DataTable dataTable, string query, Excel.Application app)
        {
            InitializeComponent();
            m_nfi = new CultureInfo("en-US", false).NumberFormat;
            m_nfi.NumberGroupSeparator = " ";
            DataTable = dataTable;
            Query = query;
            ExcelApp = app;
            queryRichTextBox.Text = query;
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = DataTable;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            dataGridView.ReadOnly = false;
            RefreshDimentions();
        }

        private void DataTableForm_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 5, MF_BYPOSITION, ToggleTopMostMenuItem, "Pin/Unpin this window");
            InsertMenu(MenuHandle, 6, MF_BYPOSITION, CenterFormMenuItem, "Center window");
        }

        ~DataTableForm() 
        {
            DataTable.Dispose(); 
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case ToggleTopMostMenuItem:
                        ToggleTopMost();
                        return;
                    case CenterFormMenuItem:
                        Utils.MoveFormToCenter(this);
                        return;
                    default:
                        break;
                }
            }
            base.WndProc(ref msg);
        }

        private void ToggleTopMost()
        {
            this.TopMost = !this.TopMost;
        }
        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void pasteButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        public void Paste()
        {
            Excel.Range rng = ExcelApp.ActiveWindow.RangeSelection;

            if (rng.Valid())
            {
                if ((rng.Worksheet.Rows.Count - rng.Row - 1) >= (DataTable.Rows.Count + (headersCheckBox.Checked ? 1 : 0)))
                {
                    UtilsExcel.PasteDataTableToRange(DataTable, rng, headersCheckBox.Checked);
                }
                else
                {
                    var result = MessageBox.Show("Range too small to paste\n\nYes: Save as tab delimited text\nNo: Save splitted to new sheets\nCancel: abort paste operation", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Yes)
                    {
                        DataTable.SaveAsTabDelimited();
                    }
                    else if (result == DialogResult.No)
                    {
                        UtilsExcel.SplitDataTableAndPasteToExcel(DataTable, rng, headersCheckBox.Checked);
                    }
                    else
                        return;
                }
            }
            else
            {
                var result = MessageBox.Show("No selection to paste", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                    Paste();
            }
        }

        private void queryRichTextBox_DoubleClick(object sender, EventArgs e)
        {
            queryRichTextBox.SendToBack();
            queryRichTextBox.Visible = false;
        }

        private void queryLabel_Click(object sender, EventArgs e)
        {
            queryRichTextBox.BringToFront();
            queryRichTextBox.Visible = true;
        }

        private void RefreshDimentions()
        {
            dataTableDimentionsLabel.Text = $"{(headersCheckBox.Checked ? "Rows with headers" : "Rows")}: {(DataTable.Rows.Count + (headersCheckBox.Checked ? 1 : 0)).ToString("N0", m_nfi)}\nColumns: {DataTable.Columns.Count.ToString("N0", m_nfi)}";
        }

        private void headersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDimentions();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DataTable.SaveAsTabDelimited();
        }
    }
}
