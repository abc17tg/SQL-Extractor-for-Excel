using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Forms;
using SQL_Extractor_for_Excel.Scripts;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Controls
{
    public partial class DataTableControl : UserControl
    {
        private CancellationTokenSource m_liveCts;
        private IProgress<DataTable> m_liveProgress;
        private NumberFormatInfo m_nfi;

        public string ControlName;
        public SqlResult SqlResult = null;
        public DataTable DataTable;
        public string Query;
        public string DisplayQuery;
        public Excel.Application ExcelApp;

        public const string REFRESHING_STRING_VALUE = "[Refreshing]";
        public const string ERROR_STRING_VALUE = "[Error]";
        public const string CANCELLED_STRING_VALUE = "[Cancelled]";

        public DataTableControl()
        {
            InitializeComponent();
            m_nfi = new CultureInfo("en-US", false).NumberFormat;
            m_nfi.NumberGroupSeparator = " ";
        }

        // ---------------------------------------------------------
        // Initialization Methods
        // ---------------------------------------------------------

        public void InitializeFromDataTable(DataTable dataTable, string query, Excel.Application app, string name, string displayQuery)
        {
            ControlName = name ?? "DataTable";
            UpdateTabTitle(ControlName);

            DataTable = dataTable;
            Query = query;
            DisplayQuery = !string.IsNullOrEmpty(displayQuery) ? displayQuery : !string.IsNullOrEmpty(query) ? query : "Query missing.";
            ExcelApp = app;

            SetupUI();
        }

        public void InitializeFromSqlResult(SqlResult sqlResult, string query, Excel.Application app, string name, string displayQuery)
        {
            ControlName = name ?? "DataTable";
            UpdateTabTitle(ControlName);

            SqlResult = sqlResult;
            DataTable = sqlResult.DataTable;
            Query = query;
            DisplayQuery = !string.IsNullOrEmpty(displayQuery) ? displayQuery : !string.IsNullOrEmpty(query) ? query : "Query missing.";
            ExcelApp = app;

            SetupUI();
        }

        public void InitializeLive(SqlServerManager sqlServerManager, string query, Excel.Application app, SqlConn sqlConn, string name, int batchSize, int timeout)
        {
            this.mainTableLayoutPanel.BackColor = Color.FromArgb(255, 140, 125);

            ControlName = name ?? "DataTable" + " (Live)";
            UpdateTabTitle(ControlName);

            Query = query;
            ExcelApp = app;
            DisplayQuery = query;

            refreshButton.Enabled = false;
            queryRichTextBox.Text = DisplayQuery;

            DataTable = new DataTable();
            SetupGrid();
            RefreshDimentions();

            m_liveProgress = new Progress<DataTable>(dt =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateLiveData(dt)));
                }
                else
                {
                    UpdateLiveData(dt);
                }
            });

            LoadDataAsync(sqlServerManager, query, sqlConn, batchSize, timeout);
        }

        // ---------------------------------------------------------
        // Core Logic
        // ---------------------------------------------------------

        private void SetupUI()
        {
            queryRichTextBox.Text = DisplayQuery;
            refreshButton.Enabled = SqlResult != null; // Only enable if we have a result object to refresh from
            SetupGrid();
            RefreshDimentions();
        }

        private void SetupGrid()
        {
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = DataTable;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            dataGridView.ReadOnly = false;
        }

        private void UpdateLiveData(DataTable dt)
        {
            DataTable = dt;
            dataGridView.DataSource = DataTable;
            RefreshDimentions();
        }

        // Helper to update the Parent TabPage text
        private void UpdateTabTitle(string text)
        {
            if (this.Parent is TabPage page)
            {
                page.Text = text;
            }
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
                        ControlName = $"{ControlName.Replace("(Live)", "(Ready)")} [ET: {Math.Floor((DateTime.Now.Subtract((DateTime)SqlResult.SqlElement.m_startTime).TotalMinutes))} min]";
                    }
                    catch (Exception)
                    {
                        ControlName = ControlName.Replace("(Live)", "(Ready)");
                    }

                    if (SqlResult.HasErrors)
                    {
                        UpdateTabTitle($"{ControlName} {ERROR_STRING_VALUE}");
                        DataTable = null;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = false;
                        saveButton.Enabled = false;
                        headersCheckBox.Enabled = false;
                        DisplayQuery = $"Query finished with errors:\n\n{SqlResult.Errors}\n\nQuery:\n\n{Query}";
                        queryRichTextBox.Text = DisplayQuery;

                        this.Cursor = Cursors.Default;
                        this.mainTableLayoutPanel.BackColor = Color.Red;
                    }
                    else if (result.Cancelled)
                    {
                        UpdateTabTitle($"{ControlName} {CANCELLED_STRING_VALUE}");
                        DataTable = SqlResult.DataTable;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = true;
                        saveButton.Enabled = true;
                        headersCheckBox.Enabled = true;
                        DisplayQuery = SqlElement.FormatQueryDetailsMessage(SqlResult.SqlElement);
                        queryRichTextBox.Text = DisplayQuery;

                        this.Cursor = Cursors.Default;
                        this.mainTableLayoutPanel.BackColor = Color.DarkGray;
                    }
                    else
                    {
                        UpdateTabTitle(ControlName);
                        DataTable = SqlResult.DataTable;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = true;
                        saveButton.Enabled = true;
                        headersCheckBox.Enabled = true;
                        DisplayQuery = SqlElement.FormatQueryDetailsMessage(SqlResult.SqlElement);
                        queryRichTextBox.Text = DisplayQuery;

                        this.Cursor = Cursors.Default;
                        this.mainTableLayoutPanel.BackColor = Color.FromKnownColor(KnownColor.Control);
                    }

                    // Ensure refresh button is enabled if we have a result
                    refreshButton.Enabled = true;
                });
            }
        }

        private Task InvokeAsync(Action action)
        {
            return Task.Run(() => { if (InvokeRequired) Invoke(action); else action(); });
        }

        // ---------------------------------------------------------
        // Event Handlers & UI Logic
        // ---------------------------------------------------------

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

        public async void Paste()
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
                        string fileName = "Export_" + DateTime.Now.ToString("yyyy_MM_dd");
                        string dbName = "";
                        await FileExport.SaveDataWithFormatChoice(fileName, dbName, Query, DataTable);
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

        public void ToogleQueryView()
        {
            if(queryRichTextBox.Visible)
            {
                queryRichTextBox.SendToBack();
                queryRichTextBox.Visible = false;
            }
            else
            {
                queryRichTextBox.BringToFront();
                queryRichTextBox.Visible = true;
            }
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

        private void closeTabButton_Click(object sender, EventArgs e)
        {
            var parent = this.FindForm() as DataTableTabbedForm;
            parent?.CloseActiveTab();
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            string fileName = "Export_" + DateTime.Now.ToString("yyyy_MM_dd");
            string dbName = "";
            await FileExport.SaveDataWithFormatChoice(fileName, dbName, Query, DataTable);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            // Check if SqlResult is valid before trying to refresh
            if (SqlResult == null || SqlResult.SqlConn == null) return;

            Task<(SqlResult, bool)> runQueryWithResult = new Task<(SqlResult, bool)>(() => (SqlServerManager.GetDataFromServer(new SqlServerManager(), Query, SqlResult.SqlConn, 0), true));

            runQueryWithResult.GetAwaiter().OnCompleted(() =>
            {
                if (this.IsDisposed || this.Disposing)
                    return;

                this.Invoke(new Action(() =>
                {
                    if (ControlName.Contains(REFRESHING_STRING_VALUE))
                        UpdateTabTitle(ControlName);

                    SqlResult sqlResult = runQueryWithResult.Result.Item1;
                    try
                    {
                        UpdateTabTitle($"DataTable [ET: {Math.Floor((DateTime.Now.Subtract((DateTime)sqlResult.SqlElement.m_startTime).TotalMinutes))} min]");
                    }
                    catch (Exception)
                    {
                        UpdateTabTitle("DataTable");
                    }

                    if (sqlResult.HasErrors)
                    {
                        UpdateTabTitle($"{ControlName} {ERROR_STRING_VALUE}");
                        DataTable = null;
                        dataGridView.DataSource = DataTable;
                        RefreshDimentions();
                        pasteButton.Enabled = false;
                        saveButton.Enabled = false;
                        headersCheckBox.Enabled = false;
                        DisplayQuery = $"Query finished with errors:\n\n{sqlResult.Errors}\n\nQuery:\n\n{Query}";
                        queryRichTextBox.Text = DisplayQuery;
                        this.Cursor = Cursors.Default;
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
                    this.Cursor = Cursors.Default;
                    return;
                }));
            });

            UpdateTabTitle($"{ControlName} {REFRESHING_STRING_VALUE}");

            runQueryWithResult.Start();
            this.Cursor = Cursors.WaitCursor;
        }

        private void pinBeforePasteCheckBoxToggle_CheckedChanged(object sender, EventArgs e)
        {
            // Logic to toggle TopMost on the PARENT FORM
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.TopMost = pinBeforePasteCheckBoxToggle.Checked;
            }
        }

        private void pasteToNewWorksheetButton_Click(object sender, EventArgs e)
        {
            Excel.Worksheet ws = ExcelApp.ActiveWorkbook.Worksheets.Add(Before: ExcelApp.ActiveSheet);
            ws.Cells[1, 1].Select();
            Paste();
            pinBeforePasteCheckBoxToggle.Checked = false;
        }
    }
}