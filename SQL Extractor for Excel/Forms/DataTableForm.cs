using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Scripts;
using static ScintillaNET.Style;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class DataTableForm : Form
    {
        private CancellationTokenSource m_liveCts;
        private IProgress<DataTable> m_liveProgress;
        private bool m_isLiveRunning = false;

        public string FormName;
        public SqlResult SqlResult = null;
        public DataTable DataTable;
        public string Query;
        public string DisplayQuery;
        Excel.Application ExcelApp;
        public const string REFRESHING_STRING_VALUE = "[Refreshing]";
        public const string ERROR_STRING_VALUE = "[Error]";
        public const string CANCELLED_STRING_VALUE = "[Cancelled]";

        private NumberFormatInfo m_nfi;

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 CenterFormMenuItem = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public DataTableForm(DataTable dataTable, string query, Excel.Application app, string name = null, string displayQuery = null)
        {
            InitializeComponent();

            m_nfi = new CultureInfo("en-US", false).NumberFormat;
            m_nfi.NumberGroupSeparator = " ";
            FormName = name ?? "DataTable";
            Text = FormName;
            DataTable = dataTable;
            Query = query;
            DisplayQuery = !string.IsNullOrEmpty(displayQuery) ? displayQuery : !string.IsNullOrEmpty(query) ? query : "Query missing.";
            ExcelApp = app;
            queryRichTextBox.Text = DisplayQuery;
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = DataTable;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            dataGridView.ReadOnly = false;
            refreshButton.Enabled = false;
            RefreshDimentions();
        }

        public DataTableForm(SqlResult sqlResult, string query, Excel.Application app, string name = null, string displayQuery = null)
        {
            InitializeComponent();

            m_nfi = new CultureInfo("en-US", false).NumberFormat;
            m_nfi.NumberGroupSeparator = " ";
            FormName = name ?? "DataTable";
            Text = FormName;
            SqlResult = sqlResult;
            DataTable = sqlResult.DataTable;
            Query = query;
            DisplayQuery = !string.IsNullOrEmpty(displayQuery) ? displayQuery : !string.IsNullOrEmpty(query) ? query : "Query missing.";
            ExcelApp = app;
            queryRichTextBox.Text = DisplayQuery;
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = DataTable;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            dataGridView.ReadOnly = false;
            RefreshDimentions();
        }

        public DataTableForm(SqlServerManager sqlServerManager, string query, Excel.Application app, SqlConn sqlConn, string name = null, int batchSize = 500, int timeout = 0)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(255, 140, 125);
            m_nfi = new CultureInfo("en-US", false).NumberFormat;
            m_nfi.NumberGroupSeparator = " ";
            FormName = name ?? "DataTable" + " (Live)";
            Text = FormName;
            Query = query;
            ExcelApp = app;
            DisplayQuery = query;
            refreshButton.Enabled = false;
            queryRichTextBox.Text = DisplayQuery;
            DataTable = new DataTable();
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = DataTable;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            RefreshDimentions();

            m_liveProgress = new Progress<DataTable>(dt =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        DataTable = dt;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                    }));
                }
                else
                {
                    DataTable = dt;
                    dataGridView.DataSource = DataTable;
                    RefreshDimentions();
                }
            });

            LoadDataAsync(sqlServerManager, query, sqlConn, batchSize, timeout);
        }

        private async void LoadDataAsync(SqlServerManager sqlServerManager, string query, SqlConn sqlConn, int batchSize, int timeout)
        {
            m_liveCts = new CancellationTokenSource();
            SqlResult result = sqlConn.Type == SqlServerManager.ServerType.SqlServer
                ? await SqlServerManager.GetDataFromSqlServerLiveAsync(sqlServerManager, query, sqlConn, m_liveProgress, m_liveCts.Token, batchSize, timeout)
                : await SqlServerManager.GetDataFromOracleLiveAsync(sqlServerManager, query, sqlConn, m_liveProgress, m_liveCts.Token, batchSize, timeout);

            if (!IsDisposed)
            {
                await InvokeAsync(() =>
                {
                    SqlResult = result;
                    DataTable = result.DataTable;

                    try
                    {
                        FormName = $"{Text.Replace("(Live)", "(Ready)")} [ET: {Math.Floor((DateTime.Now.Subtract((DateTime)SqlResult.SqlElement.m_startTime).TotalMinutes))} min]";
                    }
                    catch (Exception)
                    {
                        FormName = Text.Replace("(Live)", "(Ready)");
                    }

                    if (SqlResult.HasErrors)
                    {
                        this.Text = $"{FormName} {ERROR_STRING_VALUE}";
                        DataTable = null;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = false;
                        saveButton.Enabled = false;
                        headersCheckBox.Enabled = false;
                        DisplayQuery = $"Query finished with errors:\n\n{SqlResult.Errors}\n\nQuery:\n\n{Query}";
                        queryRichTextBox.Text = DisplayQuery;
                        this.Activate();
                        this.UseWaitCursor = false;
                        BackColor = Color.Red;
                    }
                    else if (result.Cancelled)
                    {
                        this.Text = $"{FormName} {CANCELLED_STRING_VALUE}";
                        DataTable = SqlResult.DataTable;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = true;
                        saveButton.Enabled = true;
                        headersCheckBox.Enabled = true;
                        DisplayQuery = SqlElement.FormatQueryDetailsMessage(SqlResult.SqlElement);
                        queryRichTextBox.Text = DisplayQuery;
                        this.Activate();
                        this.UseWaitCursor = false;
                        BackColor = Color.DarkGray;
                    }
                    else
                    {
                        this.Text = FormName;
                        DataTable = SqlResult.DataTable;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = true;
                        saveButton.Enabled = true;
                        headersCheckBox.Enabled = true;
                        DisplayQuery = SqlElement.FormatQueryDetailsMessage(SqlResult.SqlElement);
                        queryRichTextBox.Text = DisplayQuery;
                        this.Activate();
                        this.UseWaitCursor = false;
                        BackColor = Color.FromKnownColor(KnownColor.Control);
                    }
                });
            }
        }

        private Task InvokeAsync(Action action)
        {
            return Task.Run(() => { if (InvokeRequired) Invoke(action); else action(); });
        }


        private void DataTableForm_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 6, MF_BYPOSITION, CenterFormMenuItem, "Center window");
        }

        ~DataTableForm()
        {
            DataTable?.Dispose();
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case CenterFormMenuItem:
                        Utils.MoveFormToCenter(this);
                        return;
                    default:
                        break;
                }
            }
            base.WndProc(ref msg);
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
            pinBeforePasteCheckBoxToggle.Checked = false;
        }

        public void Paste()
        {
            Excel.Range rng = ExcelApp.ActiveWindow.RangeSelection;

            if (rng.Valid())
            {
                if ((rng.Worksheet.Rows.Count - rng.Row - 1) >= (DataTable.Rows.Count + (headersCheckBox.Checked ? 1 : 0)))
                {
                    bool success = UtilsExcel.PasteDataTableToRange(DataTable, rng, headersCheckBox.Checked);
                    if (!success)
                    {
                        var result = MessageBox.Show("No selection to paste", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        if (result == DialogResult.Retry)
                            Paste();
                    }
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
            if (DataTable == null)
                dataTableDimentionsLabel.Text = string.Empty;
            else
                dataTableDimentionsLabel.Text = $"{(headersCheckBox.Checked ? "Rows with headers" : "Rows")}: {(DataTable.Rows.Count + (headersCheckBox.Checked ? 1 : 0)).ToString("N0", m_nfi)}\nColumns: {DataTable.Columns.Count.ToString("N0", m_nfi)}";
        }

        private void headersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDimentions();
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            string fileName = "Export_" + DateTime.Now.ToString("yyyy_MM_dd");
            string dbName = "";
            await FileExport.SaveDataWithFormatChoice(fileName, dbName, Query, DataTable);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            Task<(SqlResult, bool)> runQueryWithResult = new Task<(SqlResult, bool)>(() => (SqlServerManager.GetDataFromServer(new SqlServerManager(), Query, SqlResult.SqlConn, 0), true));

            runQueryWithResult.GetAwaiter().OnCompleted(() =>
            {
                if (this.IsDisposed || this.Disposing || this == null)
                    return;

                this.Invoke(new Action(() =>
                {
                    if (this.Text.Contains(REFRESHING_STRING_VALUE))
                        this.Text = FormName;

                    SqlResult sqlResult = runQueryWithResult.Result.Item1;
                    try
                    {
                        FormName = $"DataTable [ET: {Math.Floor((DateTime.Now.Subtract((DateTime)sqlResult.SqlElement.m_startTime).TotalMinutes))} min]";
                    }
                    catch (Exception)
                    {
                        FormName = "DataTable";
                    }

                    if (sqlResult.HasErrors)
                    {
                        this.Text = $"{FormName} {ERROR_STRING_VALUE}";
                        DataTable = null;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = false;
                        saveButton.Enabled = false;
                        headersCheckBox.Enabled = false;
                        DisplayQuery = $"Query finished with errors:\n\n{sqlResult.Errors}\n\nQuery:\n\n{Query}";
                        queryRichTextBox.Text = DisplayQuery;
                        this.Show();
                        this.Activate();
                        this.UseWaitCursor = false;
                        return;
                    }

                    DataTable = sqlResult.DataTable;
                    dataGridView.DataSource = DataTable;
                    RefreshDimentions();
                    pasteButton.Enabled = true;
                    saveButton.Enabled = true;
                    headersCheckBox.Enabled = true;
                    DisplayQuery = SqlElement.FormatQueryDetailsMessage(sqlResult.SqlElement);
                    queryRichTextBox.Text = DisplayQuery;
                    this.Show();
                    this.Activate();
                    this.UseWaitCursor = false;
                    return;
                }));
            });


            if (!this.Text.Contains(REFRESHING_STRING_VALUE))
                this.Text = $"{FormName} {REFRESHING_STRING_VALUE}";

            runQueryWithResult.Start();
            this.UseWaitCursor = true;
        }

        private void DataTableForm_Activated(object sender, EventArgs e)
        {
        }

        private void pinBeforePasteCheckBoxToggle_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = pinBeforePasteCheckBoxToggle.Checked;
        }

        private void pasteToNewWorksheetButton_Click(object sender, EventArgs e)
        {
            Excel.Worksheet ws = ExcelApp.ActiveWorkbook.Worksheets.Add(Before: ExcelApp.ActiveSheet);
            ws.Cells[1, 1].Select();
            Paste();
            pinBeforePasteCheckBoxToggle.Checked = false;
        }

        private void DataTableForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                Utils.EnsureWindowIsVisible(this);
        }
    }
}
