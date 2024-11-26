using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Forms;
using SQL_Extractor_for_Excel.Scripts;
using static ScintillaNET.Style;
using Excel = Microsoft.Office.Interop.Excel;
using Timer = System.Windows.Forms.Timer;

namespace SQL_Extractor_for_Excel
{
    public partial class SqlEditorForm : Form
    {
        public string FormTitle;
        public string Query;
        public int RunningQueries = 0;
        public Excel.Application App;
        public SqlServerManager.ServerType ServerType;
        public SqlConn SqlConn;
        public static string DefaultSheetName = "Sql Query";
        public static string NewSheetName;
        public bool PasteHeaders => headersCheckBox.Checked;
        public bool PasteToSelection => pasteResultsToSelectionCheckBox.Checked;

        private readonly string m_guid;
        private readonly string m_backupName;
        private readonly object saveLock = new object();
        private SqlServerManager m_sqlManager;
        private Timer m_timer;
        private Timer m_autoSaveTimer;
        private string m_querySeparator = "--------------------------------------------------";
        private List<string> m_tablesListBoxAllItemsList = new List<string>();
        private List<string> m_tablesListBoxSelectedItemsList = new List<string>();
        private List<string> m_fieldsListBoxAllItemsList = new List<string>();
        private List<string> m_fieldsListBoxSelectedItemsList = new List<string>();
        private Dictionary<string, SqlConn> m_connDic;
        private Dictionary<string, string> m_queriesDic;
        private bool m_selectionChangedByCode = false;
        private static readonly string m_sheetNameTextBoxPlaceholder = "Worksheet name";
        private Dictionary<string, List<string>> m_variablesD = new Dictionary<string, List<string>>();

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        private const int WM_NCMOUSEMOVE = 0x00A0;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int HTCAPTION = 2;
        private bool isMouseOverForm = false;
        public const Int32 ToggleTopMostMenuItem = 1000;
        public const Int32 CenterFormMenuItem = 1001;
        public const Int32 NewFormMenuItem = 1002;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);


        public SqlEditorForm(Excel.Application app, string saveFile = null)
        {
            InitializeComponent();
            m_guid = Guid.NewGuid().ToString();
            m_backupName = $"{Text}_{m_guid}.json";
            FormTitle = Text;
            m_sqlManager = new SqlServerManager();
            App = app;
            TopMost = true;
            m_timer = new Timer();
            /*app.WindowActivate += (_, w) => this.TopMost = true;
            app.WindowDeactivate += (_, w) => this.TopMost = false;*/


            serverTypeComboBox.Items.AddRange(Directory.EnumerateDirectories(FileManager.SqlQueriesPath).Select(p => Path.GetFileName(p)).ToArray());

            UtilsScintilla.SetupSqlEditor(sqlEditorScintilla);

            serverTypeComboBox.ContextMenuStrip = new ContextMenuStrip();
            serverTypeComboBox.ContextMenuStrip.Items.Add("Add Server Connection").Click += (sender, e) => SqlServerManager.AddSqlConnection();

            serverComboBox.ContextMenuStrip = new ContextMenuStrip();
            serverComboBox.ContextMenuStrip.Items.Add("Add Server Connection").Click += (sender, e) => SqlServerManager.AddSqlConnection();

            if (!string.IsNullOrEmpty(saveFile))
                LoadEditorState(saveFile);

            sqlEditorScintilla.LostFocus += (o, s) => SaveEditorState();
            //app.WindowDeactivate += (_, w) => SaveEditorState();
            SetupAutoSaveTimer(30000);

            sheetNameTextBox.Enter += (s, e) =>
            {
                if (sheetNameTextBox.Text == m_sheetNameTextBoxPlaceholder)
                    sheetNameTextBox.Text = "";
            };

            searchTablesTextBox.Enter += (s, e) =>
            {
                if (searchTablesTextBox.Text == "Search")
                    searchTablesTextBox.Text = "";
                else
                    searchTablesTextBox.SelectAll();
            };

            searchTablesTextBox.Leave += (s, e) =>
            {
                if (searchTablesTextBox.Text == "")
                    searchTablesTextBox.Text = "Search";
            };

            searchFieldsTextBox.Enter += (s, e) =>
            {
                if (searchFieldsTextBox.Text == "Search")
                    searchFieldsTextBox.Text = "";
                else
                    searchFieldsTextBox.SelectAll();
            };

            searchFieldsTextBox.Leave += (s, e) =>
            {
                if (searchFieldsTextBox.Text == "")
                    searchFieldsTextBox.Text = "Search";
            };

            m_timer.Interval = 500;
            m_timer.Tick += (t, v) => RefreshRunningQueriesDataGridView();
            m_timer.Start();
            m_sqlManager.CommandFinished += RefreshRunningQueriesDataGridView;
            m_sqlManager.CommandFinished += SaveEditorState;
            ContextMenu cm = new ContextMenu();

            MenuItem copyCMI = new MenuItem("Copy", (o, e) => { sqlEditorScintilla.Copy(); });
            MenuItem pasteCMI = new MenuItem("Paste", (o, e) => { sqlEditorScintilla.Paste(); });
            MenuItem fetchCMI = new MenuItem("Fetch", (o, e) => { fetchTablesBtn.PerformClick(); });
            MenuItem commentCMI = new MenuItem("Comment", (o, e) => { commentBtn.PerformClick(); });
            MenuItem pasteRangeCMI = new MenuItem("Paste range", (o, e) => { pasteRngBtn.PerformClick(); });
            MenuItem pasteClipboardRangeCMI = new MenuItem("Paste rng from clipboard", (o, e) => { PasteFromClipboard(); });
            MenuItem formatToSqlCMI = new MenuItem("Format to SQL", (o, e) => { UtilsScintilla.ReformatTextToSql(sqlEditorScintilla); });
            MenuItem toggleWrapModeCMI = new MenuItem("Toggle text wrap mode", (o, e) => { if (sqlEditorScintilla.WrapMode == ScintillaNET.WrapMode.None) sqlEditorScintilla.WrapMode = ScintillaNET.WrapMode.Word; else sqlEditorScintilla.WrapMode = ScintillaNET.WrapMode.None; });
            MenuItem runSelectionCMI = new MenuItem("Run selected", (o, e) => { runSelectionBtn.PerformClick(); });
            MenuItem runBlockCMI = new MenuItem("Run block (block identifier '-----')", (o, e) => { UtilsScintilla.SelectBlock(sqlEditorScintilla); runSelectionBtn.PerformClick(); });
            cm.MenuItems.Add(pasteCMI);
            cm.MenuItems.Add(copyCMI);
            cm.MenuItems.Add(fetchCMI);
            cm.MenuItems.Add(commentCMI);
            cm.MenuItems.Add(pasteRangeCMI);
            cm.MenuItems.Add(pasteClipboardRangeCMI);
            cm.MenuItems.Add(formatToSqlCMI);
            cm.MenuItems.Add(toggleWrapModeCMI);
            cm.MenuItems.Add(runSelectionCMI);
            cm.MenuItems.Add(runBlockCMI);
            sqlEditorScintilla.ContextMenu = cm;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                string saveFile = Path.Combine(FileManager.SqlEditorBackupPath, m_backupName);
                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                }
            }
            else
                SaveEditorState();
        }

        ~SqlEditorForm()
        {
            m_timer?.Dispose();
            m_autoSaveTimer?.Dispose();
            //App.WindowDeactivate -= (_, w) => SaveEditorState();
            m_sqlManager = null;
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case ToggleTopMostMenuItem:
                        ToggleTopMost();
                        break;
                    case CenterFormMenuItem:
                        Utils.MoveFormToCenter(this);
                        break;
                    case NewFormMenuItem:
                        SqlEditorForm form = new SqlEditorForm(App);
                        form.Show();
                        break;
                    default:
                        break;
                }
            }

            switch (msg.Msg)
            {
                case WM_MOUSEMOVE:
                case WM_NCMOUSEMOVE:
                    int hitTest = msg.Msg == WM_NCMOUSEMOVE ? msg.WParam.ToInt32() : 0;
                    if (hitTest == HTCAPTION || msg.Msg == WM_MOUSEMOVE)
                    {
                        if (!isMouseOverForm)
                        {
                            this.Opacity = 0.95;
                            isMouseOverForm = true;
                        }
                    }
                    else if (isMouseOverForm)
                    {
                        if (!this.ContainsFocus)
                            this.Opacity = 0.65;
                        isMouseOverForm = false;
                    }
                    break;
            }
            base.WndProc(ref msg);
        }

        private void SqlEditorForm_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 5, MF_BYPOSITION, ToggleTopMostMenuItem, "Pin/Unpin this window");
            InsertMenu(MenuHandle, 6, MF_BYPOSITION, CenterFormMenuItem, "Center window");
            InsertMenu(MenuHandle, 7, MF_BYPOSITION, NewFormMenuItem, "New SQL Extractor window");
        }

        private void ToggleTopMost()
        {
            this.TopMost = !this.TopMost;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RefreshSserverTypeComboBox()
        {
            serverTypeComboBox.Items.AddRange(Directory.EnumerateDirectories(FileManager.SqlQueriesPath).Select(p => Path.GetFileName(p)).ToArray());
        }

        private void RefreshSavedQueriesComboBox()
        {
            SqlServerManager.ServerType serverType = (SqlServerManager.ServerType)Enum.Parse(typeof(SqlServerManager.ServerType), serverTypeComboBox.SelectedItem.ToString());
            if (!Enum.IsDefined(typeof(SqlServerManager.ServerType), serverType))
                return;

            switch (serverType)
            {
                case SqlServerManager.ServerType.SqlServer:
                    this.TopMost = false;
                    m_connDic = FileManager.GetSqlServerConnectionValues();
                    m_queriesDic = FileManager.GetSqlServerQueries();
                    this.TopMost = true;
                    break;
                case SqlServerManager.ServerType.Oracle:
                    this.TopMost = false;
                    m_connDic = FileManager.GetOracleConnectionValues();
                    m_queriesDic = FileManager.GetOracleQueries();
                    this.TopMost = true;
                    break;
                case SqlServerManager.ServerType.Excel:
                    this.TopMost = false;
                    m_connDic = null;
                    m_queriesDic = FileManager.GetExcelQueries();
                    this.TopMost = true;
                    break;
                default:
                    return;
            }
            ServerType = serverType;
            savedQueriesComboBox.Items.Clear();
            try
            {
                savedQueriesComboBox.Items.AddRange(m_queriesDic.Keys.Select(p => Path.GetFileName(p)).ToArray());
            }
            catch { }

            serverComboBox.Items.Clear();
            try
            {
                if (ServerType != SqlServerManager.ServerType.Excel)
                    serverComboBox.Items.AddRange(m_connDic.Keys.ToArray());
            }
            catch { }
        }

        private void validateBtn_Click(object sender, EventArgs e)
        {
            string err = null;

            if (new List<string> { sqlEditorScintilla.Text, SqlConn?.ConnectionString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                return;
            }

            err = SqlServerManager.CheckSqlQuerySyntaxOnline(sqlEditorScintilla.Text, SqlConn);

            switch (err)
            {
                case null:
                    MessageBox.Show("Error! Maybe no internet connection.");
                    break;
                case "":
                    MessageBox.Show("Syntax ok.");
                    break;
                default:
                    MessageBox.Show(err);
                    break;
            }
        }

        private void validateSelectionBtn_Click(object sender, EventArgs e)
        {
            string err = null;

            if (string.IsNullOrWhiteSpace(sqlEditorScintilla.SelectedText))
            {
                UtilsScintilla.SelectBlock(sqlEditorScintilla);
                return;
            }

            if (new List<string> { sqlEditorScintilla.SelectedText, SqlConn?.ConnectionString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                return;
            }

            err = SqlServerManager.CheckSqlQuerySyntaxOnline(sqlEditorScintilla.SelectedText, SqlConn);

            switch (err)
            {
                case null:
                    MessageBox.Show("Error! Maybe no internet connection.");
                    break;
                case "":
                    MessageBox.Show("Syntax OK");
                    break;
                default:
                    MessageBox.Show(err);
                    break;
            }
        }

        private void PasteFromClipboard()
        {
            string text = Clipboard.GetText(TextDataFormat.Text);
            if (string.IsNullOrWhiteSpace(text))
                return;

            UtilsScintilla.ReformatTextToSql(sqlEditorScintilla, text);
        }

        private void pasteRngBtn_Click(object sender, EventArgs e)
        {
            Excel.Range rng = App.ActiveWindow.RangeSelection;
            if (rng.Valid())
                sqlEditorScintilla.ReplaceSelection(UtilsExcel.FormatRangeToSqlPattern(rng));
        }

        private void pasteRngFilterBtn_Click(object sender, EventArgs e)
        {
            Excel.Range rng = App.ActiveWindow.RangeSelection;
            if (!rng.Valid())
                return;

            string rngText = UtilsExcel.GenerateSqlFilterFromExcelSelection(rng);
            if (!string.IsNullOrEmpty(rngText))
                sqlEditorScintilla.ReplaceSelection(rngText);
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            Query = sqlEditorScintilla.Text;
            Run(Query);
        }

        private void runSelectionBtn_Click(object sender, EventArgs e)
        {
            Query = sqlEditorScintilla.SelectedText;

            if (string.IsNullOrWhiteSpace(Query))
            {
                UtilsScintilla.SelectBlock(sqlEditorScintilla);
                return;
            }

            Run(Query);
        }

        private void Run_old(string query)
        {
            if (new List<string> { query, SqlConn?.ConnectionString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                return;
            }

            /*string err = null;
            err = SqlServerManager.CheckSqlQuerySyntaxOnline(query, SqlConn);

            DialogResult result;
            switch (err)
            {
                case null:
                    result = MessageBox.Show("Error! Maybe no internet connection.", "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (result == DialogResult.Abort)
                        return;
                    else if (result == DialogResult.Retry)
                    {
                        Run(query);
                        return;
                    }
                    break;
                case "":
                    break;
                default:
                    result = MessageBox.Show(err, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (result == DialogResult.Abort)
                        return;
                    else if(result == DialogResult.Retry)
                    {
                        Run(query);
                        return;
                    }
                    break;
            }*/

            Task<(SqlResult, bool)> runQueryWithResult = null;

            if (++RunningQueries == 1)
                Text = $"{FormTitle} [{RunningQueries}] running queries";
            else
                Text = Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");

            if (pasteToDataTableCheckBox.Checked)
            {
                runQueryWithResult = new Task<(SqlResult, bool)>(() => (SqlServerManager.GetDataFromServer(m_sqlManager, query, SqlConn, 0), true));

                runQueryWithResult.GetAwaiter().OnCompleted(() =>
                {
                    --RunningQueries;

                    if (this.IsDisposed || this.Disposing || this == null)
                        return;

                    string text;
                    this.Invoke(new Action(() =>
                    {
                        if (RunningQueries <= 0)
                            text = FormTitle;
                        else
                            text = Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");

                        this.Text = text;

                        SqlResult sqlResult = runQueryWithResult.Result.Item1;
                        if (sqlResult.HasErrors)
                        {
                            if (!sqlResult.Cancelled)
                            {
                                string msg = $"Query finished with errors:\n\n{sqlResult.Errors}\n\nQuery:\n\n{query}";
                                MessageBoxForm messageBox = new MessageBoxForm(msg, "Query finished", true);
                                messageBox.Show();
                            }
                            return;
                        }
                        DataTableForm form = new DataTableForm(sqlResult.DataTable, query, App);
                        form.Show();
                        form.Activate();
                    }));
                });
            }
            else
            {
                Excel.Range rng;
                Excel.Worksheet ws = null;
                string wsName;
                if (!pasteResultsToSelectionCheckBox.Checked)
                {
                    Excel.Workbook wb = App.ActiveWorkbook;
                    if (wb == null)
                        wb = App.Workbooks.Add();
                    ws = wb.Sheets.Add();
                    wsName = NewSheetName == m_sheetNameTextBoxPlaceholder ? DefaultSheetName : NewSheetName;
                    if (!string.IsNullOrEmpty(wsName))
                    {
                        ws.Rename(wsName);
                        wsName = ws.Name;
                    }
                    rng = ws.Cells[1, 1];
                }
                else
                {
                    rng = App.ActiveWindow.RangeSelection;
                    wsName = rng.Worksheet?.Name;
                }

                runQueryWithResult = new Task<(SqlResult, bool)>(() => SqlServerManager.GetDataFromServerToExcelRange(m_sqlManager, query, SqlConn, rng, PasteHeaders, 0));

                runQueryWithResult.GetAwaiter().OnCompleted(() =>
                {
                    if (this.IsDisposed || this.Disposing || this == null)
                        return;

                    --RunningQueries;

                    this.Invoke(new Action(() =>
                    {
                        string text;
                        if (RunningQueries <= 0)
                            text = FormTitle;
                        else
                            text = Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");

                        this.Text = text;

                        if (!runQueryWithResult.Result.Item2)
                        {
                            var result = MessageBox.Show($"Query for Worksheet [{wsName ?? "null"}] finished but Worksheet/Range unavailable/too small.\n\nPaste to DataTable?", "Query finished", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                SqlResult sqlResult = runQueryWithResult.Result.Item1;
                                DataTableForm form = new DataTableForm(sqlResult.DataTable, query, App);
                                form.Show();
                                form.Activate();
                            }
                            else if (result == DialogResult.No && ws.Exists())
                            {
                                App.DisplayAlerts = false;
                                ws.Delete();
                                App.DisplayAlerts = true;
                            }
                        }
                        else
                        {
                            string msg;
                            SqlResult sqlResult = runQueryWithResult.Result.Item1;
                            if (sqlResult.HasErrors)
                            {
                                msg = $"Query finished with errors:\n\n{sqlResult.Errors}\n\nQuery:\n\n{query}";
                                if (ws.Exists() && (ws.Parent as Excel.Workbook).Worksheets.Count > 1)
                                {
                                    App.DisplayAlerts = false;
                                    ws.Delete();
                                    App.DisplayAlerts = true;
                                }
                            }
                            else
                                msg = $"{query}\n\nFinished";
                            if (!sqlResult.Cancelled)
                            {
                                MessageBoxForm messageBox = new MessageBoxForm(msg, $"{wsName ?? string.Empty} query finished", true);
                                messageBox.Show();
                            }
                        }
                    }));
                });
            }

            runQueryWithResult.Start();
        }

        private void Run(string query)
        {
            if (new List<string> { query, SqlConn?.ConnectionString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                return;
            }

            if (++RunningQueries == 1)
                Text = $"{FormTitle} [{RunningQueries}] running queries";
            else
                Text = Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");

            //runQueryWithResult = null;
            if (pasteToDataTableCheckBox.Checked)
            {
                RunQueryToDataTable(query, SqlConn);
            }
            else
            {
                RunQueryToExcel(query, SqlConn);
            }
            SaveEditorState();
        }

        private void RunQueryToDataTable(string query, SqlConn sqlConn)
        {
            Task<(SqlResult, bool)> runQueryWithResult = new Task<(SqlResult, bool)>(() => (SqlServerManager.GetDataFromServer(m_sqlManager, query, sqlConn, 0), true));

            runQueryWithResult.GetAwaiter().OnCompleted(() =>
            {
                if (this.IsDisposed || this.Disposing || this == null)
                    return;

                --RunningQueries;

                this.Invoke(new Action(() =>
                {
                    UpdateRunningQueriesText();

                    SqlResult sqlResult = runQueryWithResult.Result.Item1;
                    if (sqlResult.HasErrors)
                    {
                        if (!sqlResult.Cancelled)
                        {
                            string msg = $"Query finished with errors:\n\n{sqlResult.Errors}\n\nQuery:\n\n{query}";
                            MessageBoxForm messageBox = new MessageBoxForm(msg, "Query finished", true);
                            messageBox.Show();
                        }
                        return;
                    }
                    DataTableForm form = new DataTableForm(sqlResult.DataTable, query, App);
                    form.Show();
                    form.Activate();
                }));
            });

            runQueryWithResult.Start();
        }

        private void RunQueryToExcel(string query, SqlConn sqlConn)
        {

            Excel.Range rng;
            Excel.Worksheet ws = null;
            string wsName;
            if (!pasteResultsToSelectionCheckBox.Checked)
            {
                Excel.Workbook wb = App.ActiveWorkbook;
                if (wb == null)
                    wb = App.Workbooks.Add();
                ws = wb.Sheets.Add();
                wsName = NewSheetName == m_sheetNameTextBoxPlaceholder ? DefaultSheetName : NewSheetName;
                if (!string.IsNullOrEmpty(wsName))
                {
                    ws.Rename(wsName);
                    wsName = ws.Name;
                }
                rng = ws.Cells[1, 1];
            }
            else
            {
                rng = App.ActiveWindow.RangeSelection;
                wsName = rng.Worksheet?.Name;
            }

            Task<(SqlResult, bool)> runQueryWithResult = new Task<(SqlResult, bool)>(() => SqlServerManager.GetDataFromServerToExcelRange(m_sqlManager, query, sqlConn, rng, PasteHeaders, 0));

            runQueryWithResult.GetAwaiter().OnCompleted(() =>
            {
                if (this.IsDisposed || this.Disposing || this == null)
                    return;

                --RunningQueries;

                this.Invoke(new Action(() =>
                {
                    string text;
                    if (RunningQueries <= 0)
                        text = FormTitle;
                    else
                        text = Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");

                    this.Text = text;

                    if (!runQueryWithResult.Result.Item2)
                    {
                        var result = MessageBox.Show($"Query for Worksheet [{wsName ?? "null"}] finished but Worksheet/Range unavailable/too small.\n\nPaste to DataTable?", "Query finished", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            SqlResult sqlResult = runQueryWithResult.Result.Item1;
                            DataTableForm form = new DataTableForm(sqlResult.DataTable, query, App);
                            form.Show();
                            form.Activate();
                        }
                        else if (result == DialogResult.No && ws.Exists())
                        {
                            App.DisplayAlerts = false;
                            ws.Delete();
                            App.DisplayAlerts = true;
                        }
                    }
                    else
                    {
                        string msg;
                        SqlResult sqlResult = runQueryWithResult.Result.Item1;
                        if (sqlResult.HasErrors)
                        {
                            msg = $"Query finished with errors:\n\n{sqlResult.Errors}\n\nQuery:\n\n{query}";
                            if (ws.Exists() && (ws.Parent as Excel.Workbook).Worksheets.Count > 1)
                            {
                                App.DisplayAlerts = false;
                                ws.Delete();
                                App.DisplayAlerts = true;
                            }
                        }
                        else
                            msg = $"{query}\n\nFinished";
                        if (!sqlResult.Cancelled)
                        {
                            MessageBoxForm messageBox = new MessageBoxForm(msg, $"{wsName ?? string.Empty} query finished", true);
                            messageBox.Show();
                        }
                    }
                }));
            });

            runQueryWithResult.Start();
        }

        private void UpdateRunningQueriesText()
        {
            Text = RunningQueries <= 0
                ? FormTitle
                : Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");
        }

        private void commentBtn_Click(object sender, EventArgs e)
        {
            UtilsScintilla.Comment(sqlEditorScintilla);
        }

        private void testConnBtn_Click(object sender, EventArgs e)
        {
            SqlConn sqlConn;
            bool result = m_connDic.TryGetValue(m_connDic.Keys.First(p => p == serverComboBox.SelectedItem.ToString()) ?? "", out sqlConn);
            if (result)
            {
                if (sqlConn.Test())
                    MessageBox.Show("Connection success!");
                else
                    MessageBox.Show("Connection failed!");
            }
            else
                MessageBox.Show("Connection failed!");
        }

        private void saveQueryBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "sql",
                Title = "Save query",
                ValidateNames = true,
                FileName = "Untitled.sql",
                Filter = "SQL queries | *.sql",
                OverwritePrompt = true
            };

            switch (ServerType)
            {
                case SqlServerManager.ServerType.SqlServer:
                    saveFileDialog.InitialDirectory = FileManager.SqlServerQueriesPath;
                    break;
                case SqlServerManager.ServerType.Oracle:
                    saveFileDialog.InitialDirectory = FileManager.OracleQueriesPath;
                    break;
                case SqlServerManager.ServerType.Excel:
                    saveFileDialog.InitialDirectory = FileManager.ExcelQueriesPath;
                    break;
                default:
                    break;
            }

            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                using (var sw = File.CreateText(filePath))
                {
                    sw.Write(sqlEditorScintilla.Text);
                }
                /*savedQueriesComboBox.Items.Clear();
                savedQueriesComboBox.Items.AddRange(m_queriesDic.Keys.Select(p => Path.GetFileName(p)).ToArray());*/
                RefreshSavedQueriesComboBox();
            }
        }

        private void serverTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSavedQueriesComboBox();
        }

        private void savedQueriesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(sqlEditorScintilla.Text) && !(sqlEditorScintilla.Text.Trim() == "SELECT * FROM"))
            {
                DialogResult result;
                result = MessageBox.Show("That will load query and erase current one.\nDo you want to paste it below?", "Load query warning",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Yes:
                        sqlEditorScintilla.Text = sqlEditorScintilla.Text.TrimEnd('\n', '\r', '\t', ' ');
                        int position = sqlEditorScintilla.Lines.Last().Position;
                        sqlEditorScintilla.AppendText($"{Environment.NewLine + Environment.NewLine}{new string('-', 50)}{Environment.NewLine + Environment.NewLine}{m_queriesDic[m_queriesDic.Keys.First(p => Path.GetFileName(p) == savedQueriesComboBox.SelectedItem.ToString())]}");
                        sqlEditorScintilla.GotoPosition(position);
                        break;
                    case DialogResult.No:
                        sqlEditorScintilla.Text = m_queriesDic[m_queriesDic.Keys.First(p => Path.GetFileName(p) == savedQueriesComboBox.SelectedItem.ToString())];
                        break;
                    case DialogResult.Cancel:
                    case DialogResult.None:
                    default:
                        return;
                }
            }
            else
                sqlEditorScintilla.Text = m_queriesDic[m_queriesDic.Keys.First(p => Path.GetFileName(p) == savedQueriesComboBox.SelectedItem.ToString())];
        }

        private void serverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var result = m_connDic.TryGetValue((sender as ComboBox).SelectedItem.ToString(), out SqlConn);
        }

        private void sqlEditorScintilla_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                UtilsScintilla.IndentAfterReturn(sqlEditorScintilla);
        }

        private void sqlEditorScintilla_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && (e.KeyCode == Keys.Divide || e.KeyCode == Keys.Oem2))
            {
                UtilsScintilla.Comment(sqlEditorScintilla);
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.R)
            {
                UtilsScintilla.ReformatTextToSql(sqlEditorScintilla);
                e.Handled = true; //e.SuppressKeyPress = true;
            }

            if (e.Alt)
            {
                if (e.KeyCode == Keys.Up)
                {
                    UtilsScintilla.MoveLineUp(sqlEditorScintilla);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    UtilsScintilla.MoveLineDown(sqlEditorScintilla);
                    e.Handled = true;
                }
            }
        }

        private void FetchFields(string tableName, SqlConn sqlConn)
        {
            m_fieldsListBoxAllItemsList.Clear();
            m_fieldsListBoxSelectedItemsList.Clear();
            fieldsListBox.Items.Clear();
            fieldsListBox.Items.Add("Fetching...");
            fieldsListBox.Update();

            bool tableNameIsQuery = tableName.Contains("select", StringComparison.OrdinalIgnoreCase);
            var sqlResult = SqlServerManager.GetDataFromServer(m_sqlManager, $"SELECT * FROM {(tableNameIsQuery ? "(" : "")}{tableName.Trim()}{(tableNameIsQuery ? ") FIELDS" : "")} WHERE 1=0", sqlConn, 40);
            fieldsListBox.Items.Clear(); // clear "Fetching..." from the list
            if (!sqlResult.HasErrors)
            {
                fieldsListBox.Items.AddRange(sqlResult.DataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).Distinct().ToArray());
                m_fieldsListBoxAllItemsList.AddRange(fieldsListBox.Items.Cast<string>().ToList());
            }

            objectsAndVariablesTabControl.SelectedTab = fieldsTabPage;
        }

        private void FetchTables(SqlConn sqlConn)
        {
            m_tablesListBoxAllItemsList.Clear();
            m_tablesListBoxSelectedItemsList.Clear();
            tablesListBox.Items.Clear();
            tablesListBox.Items.Add("Fetching...");
            tablesListBox.Update();

            string query;
            switch (sqlConn.Type)
            {
                case SqlServerManager.ServerType.SqlServer:
                    query = "CREATE TABLE #AllTables (Database_Schema_Object NVARCHAR(MAX)); DECLARE @sql NVARCHAR(MAX) = N''; DECLARE @dbName NVARCHAR(128); DECLARE dbCursor CURSOR FOR SELECT [name] FROM sys.databases WHERE state = 0 AND [name] NOT IN ('master', 'tempdb', 'model', 'msdb'); OPEN dbCursor; FETCH NEXT FROM dbCursor INTO @dbName; WHILE @@FETCH_STATUS = 0 BEGIN SET @sql = N'USE [' + @dbName + ']; INSERT INTO #AllTables SELECT ''' + @dbName + '.'' + SCHEMA_NAME(schema_id) + ''.'' + [name] FROM sys.tables t WHERE EXISTS (SELECT 1 FROM ' + QUOTENAME(@dbName) + '.sys.partitions p WHERE p.object_id = t.object_id AND p.rows > 0) UNION ALL SELECT ''' + @dbName + '.'' + SCHEMA_NAME(schema_id) + ''.'' + [name] FROM sys.views v;'; BEGIN TRY EXEC sp_executesql @sql; END TRY BEGIN CATCH PRINT 'Error accessing database ' + @dbName + ': ' + ERROR_MESSAGE(); END CATCH; FETCH NEXT FROM dbCursor INTO @dbName; END CLOSE dbCursor; DEALLOCATE dbCursor; SELECT * FROM #AllTables ORDER BY Database_Schema_Object; DROP TABLE #AllTables;";
                    break;
                case SqlServerManager.ServerType.Oracle:
                    query = "SELECT OWNER || '.' || OBJECT_NAME FROM (SELECT DISTINCT OWNER, OBJECT_NAME FROM ALL_OBJECTS WHERE OBJECT_TYPE IN ('VIEW', 'TABLE') AND STATUS = 'VALID' ORDER BY OBJECT_NAME)";
                    break;
                case SqlServerManager.ServerType.Excel:
                    tablesListBox.Items.Clear();
                    return;
                default:
                    tablesListBox.Items.Clear();
                    return;
            }

            var sqlResult = SqlServerManager.GetDataFromServer(m_sqlManager, query, sqlConn, 40);
            tablesListBox.Items.Clear(); // clear "Fetching..." from the list
            if (!sqlResult.HasErrors)
            {
                tablesListBox.Items.AddRange(sqlResult.DataTable.AsEnumerable().Select(row => row.Field<string>(0)).Distinct().ToArray() ?? new string[1]);
                m_tablesListBoxAllItemsList.AddRange(tablesListBox.Items.Cast<string>().ToList());
            }

            objectsAndVariablesTabControl.SelectedTab = tablesTabPage;
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {
            ListBox listBox = objectsAndVariablesTabControl.SelectedTab.FindAllChildrenByType<ListBox>().FirstOrDefault();

            SqlConn sqlConn;
            try
            {
                bool result = m_connDic.TryGetValue(m_connDic.Keys.FirstOrDefault(p => p.Contains(serverComboBox.SelectedItem.ToString())), out sqlConn);
                if (result)
                    result = sqlConn.Test();
                if (!result)
                    MessageBox.Show("Connection failed!");
            }
            catch
            {
                MessageBox.Show("Connection failed!");
                listBox.Items.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(sqlEditorScintilla.SelectedText))
            {
                FetchTables(sqlConn);
            }
            else
            {
                FetchFields(sqlEditorScintilla.SelectedText, sqlConn);
            }
        }

        private void TransferToQueryFromListbox()
        {
            ListBox listBox = objectsAndVariablesTabControl.SelectedTab.FindAllChildrenByType<ListBox>().FirstOrDefault();
            if (listBox == null)
                return;

            string text = string.Empty;
            if (objectsAndVariablesTabControl.SelectedTab == tablesTabPage)
            {
                text = string.Join(Environment.NewLine, listBox.SelectedItems.Cast<string>());
                sqlEditorScintilla.ReplaceSelection(text ?? "");
                return;
            }
            else
                foreach (var obj in listBox.SelectedItems)
                {
                    if (!obj.ToString().Contains(" "))
                        text += $", {obj.ToString()}";
                    else
                        text += $", [{obj.ToString()}]";
                }

            int lastWordRange = Math.Max(sqlEditorScintilla.WordStartPosition(sqlEditorScintilla.SelectionStart, true) - 10, 0);
            string lastText = sqlEditorScintilla.GetTextRange(lastWordRange, Math.Min(sqlEditorScintilla.SelectionStart, 10)).TrimEnd('\t', '\n', '\r', ' ');

            if (string.IsNullOrWhiteSpace(lastText) ||
                lastText.EndsWith("select", true, System.Globalization.CultureInfo.InvariantCulture) ||
                FileManager.SqlKeywords.Split(' ').Any(p => lastText.EndsWith(p, true, System.Globalization.CultureInfo.InvariantCulture)) || lastText.EndsWith("("))
            {
                if ((new char[] { ' ', '\t' }).ToList().Contains((char)sqlEditorScintilla.GetCharAt(sqlEditorScintilla.SelectionStart - 1)))
                    sqlEditorScintilla.ReplaceSelection(text?.TrimStart(',', ' ') ?? "");
                else
                    sqlEditorScintilla.ReplaceSelection(text?.TrimStart(',') ?? "");
            }
            else
                sqlEditorScintilla.ReplaceSelection(text ?? "");
        }

        private void transferToQueryBtn_Click(object sender, EventArgs e)
        {
            TransferToQueryFromListbox();
        }

        private void wrapIntoBlockBtn_Click(object sender, EventArgs e)
        {
            UtilsScintilla.WrapIntoSqlBlock(sqlEditorScintilla);
        }

        private void openInNotepadBtn_Click(object sender, EventArgs e)
        {
            if (sqlEditorScintilla.Text.Length > 1)
                FileManager.OpenStringWithNotepad(sqlEditorScintilla.Text);
        }

        private void sheetNameTextBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(sheetNameTextBox.Text))
                NewSheetName = sheetNameTextBox.Text;
            else
                NewSheetName = m_sheetNameTextBoxPlaceholder;
        }

        private void pasteResultsToSelectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (pasteResultsToSelectionCheckBox.Checked)
            {
                sheetNameTextBox.Enabled = false;
                fillSheetNameBtn.Enabled = false;
            }
            else
            {
                sheetNameTextBox.Enabled = true;
                fillSheetNameBtn.Enabled = true;
            }
        }

        private void pasteToDataTableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (pasteToDataTableCheckBox.Checked)
            {
                sheetNameTextBox.Enabled = false;
                fillSheetNameBtn.Enabled = false;
                pasteResultsToSelectionCheckBox.Enabled = false;
                headersCheckBox.Enabled = false;
            }
            else
            {
                if (pasteResultsToSelectionCheckBox.Checked)
                {
                    sheetNameTextBox.Enabled = false;
                    fillSheetNameBtn.Enabled = false;
                }
                else
                {
                    sheetNameTextBox.Enabled = true;
                    fillSheetNameBtn.Enabled = true;
                }
                pasteResultsToSelectionCheckBox.Enabled = true;
                headersCheckBox.Enabled = true;
            }
        }

        private void fillSheetNameBtn_Click(object sender, EventArgs e)
        {
            string word = sqlEditorScintilla.GetWordFromPosition(sqlEditorScintilla.CurrentPosition);
            if (!string.IsNullOrWhiteSpace(word))
            {
                sheetNameTextBox.Text = word;
                sheetNameTextBox.Focus();
            }
        }

        private void objectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If Ctrl is not pressed, clear the selected items list first
            if (!m_selectionChangedByCode && (Control.ModifierKeys & Keys.Control) == 0)
            {
                if (objectsAndVariablesTabControl.SelectedTab == tablesTabPage)
                    m_tablesListBoxSelectedItemsList.Clear();
                else
                    m_fieldsListBoxSelectedItemsList.Clear();
            }

            if (objectsAndVariablesTabControl.SelectedTab == tablesTabPage)
            {
                foreach (string item in (sender as ListBox).SelectedItems)
                    if (!m_tablesListBoxSelectedItemsList.Contains(item))
                        m_tablesListBoxSelectedItemsList.Add(item);
            }
            else
            {
                foreach (string item in (sender as ListBox).SelectedItems)
                    if (!m_fieldsListBoxSelectedItemsList.Contains(item))
                        m_fieldsListBoxSelectedItemsList.Add(item);
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                bool tables;
                ListBox listBox;
                if (objectsAndVariablesTabControl.SelectedTab == tablesTabPage)
                {
                    listBox = tablesListBox;
                    tables = true;
                }
                else
                {
                    listBox = fieldsListBox;
                    tables = false;
                }

                if ((tables && (m_tablesListBoxAllItemsList == null || m_tablesListBoxAllItemsList.Count < 1)) || (!tables && (m_fieldsListBoxAllItemsList == null || m_fieldsListBoxAllItemsList.Count < 1)))
                    return;

                // to ignore on changed event
                m_selectionChangedByCode = true;

                // Clear the ListBox
                listBox.Items.Clear();

                // Filter the items and add them to the ListBox
                List<string> filteredItems;
                if (tables)
                    filteredItems = m_tablesListBoxAllItemsList.Where(item => item.IndexOf(searchTablesTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0 || m_tablesListBoxSelectedItemsList.Contains(item)).ToList();
                else
                    filteredItems = m_fieldsListBoxAllItemsList.Where(item => item.IndexOf(searchFieldsTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0 || m_fieldsListBoxSelectedItemsList.Contains(item)).ToList();

                listBox.Items.AddRange(filteredItems.ToArray());
                listBox.Update();

                // Reselect the previously selected items
                for (int i = 0; i < (tables ? tablesListBox.Items.Count : fieldsListBox.Items.Count); i++)
                {
                    var item = listBox.Items[i].ToString();
                    if ((tables ? m_tablesListBoxSelectedItemsList : m_fieldsListBoxSelectedItemsList).Contains(item))
                        listBox.SetSelected(i, true);
                }

                m_selectionChangedByCode = false;
            }
        }

        private void clearEditorLabel_Click(object sender, EventArgs e)
        {
            sqlEditorScintilla.ClearAll();
        }

        private void sqlEditorScintilla_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string word = sqlEditorScintilla.GetWordFromPosition(sqlEditorScintilla.CurrentPosition);
            if (!string.IsNullOrWhiteSpace(word))
            {
                sheetNameTextBox.Text = word;
                sheetNameTextBox.Focus();
            }
        }

        private void objectsListBox_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = (sender as ListBox);
            if ((objectsAndVariablesTabControl.SelectedTab == tablesTabPage) && listBox.SelectedItems.Count == 1)
            {
                SqlConn sqlConn;
                try
                {
                    bool result = m_connDic.TryGetValue(m_connDic.Keys.FirstOrDefault(p => p.Contains(serverComboBox.SelectedItem.ToString())), out sqlConn);
                    if (result)
                        result = sqlConn.Test();
                    if (!result)
                        MessageBox.Show("Connection failed!");
                }
                catch
                {
                    MessageBox.Show("Connection failed!");
                    listBox.Items.Clear();
                    return;
                }
                objectsAndVariablesTabControl.SelectedTab = fieldsTabPage;
                FetchFields(listBox.SelectedItem.ToString(), sqlConn);
            }
            else if ((objectsAndVariablesTabControl.SelectedTab == fieldsTabPage) && listBox.SelectedItems.Count > 0)
                TransferToQueryFromListbox();
        }

        private void SqlEditorForm_Activated(object sender, EventArgs e)
        {
            this.Opacity = 0.95;
        }

        private void SqlEditorForm_Deactivate(object sender, EventArgs e)
        {
            this.Opacity = 0.65;
        }

        private void variablesDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void formatToSqlBtn_Click(object sender, EventArgs e)
        {
            UtilsScintilla.ReformatTextToSql(sqlEditorScintilla);
        }

        private void separateBtn_Click(object sender, EventArgs e)
        {
            sqlEditorScintilla.ReplaceSelection($"{Environment.NewLine}{m_querySeparator}{Environment.NewLine}");
        }

        private void objectsAndVariablesTabControl_TabIndexChanged(object sender, EventArgs e)
        {
            if (objectsAndVariablesTabControl.SelectedTab == variablesTabPage)
            {
                RefreshVariables();
            }
        }

        private void RefreshVariables()
        {
            // Remove rows without variable name
            foreach (var row in variablesDataGridView.Rows.Cast<DataGridViewRow>().Where(p => !p.IsNewRow && p.Visible && string.IsNullOrEmpty(p.Cells[1].Value?.ToString())).ToList())
                variablesDataGridView.Rows.Remove(row);

            List<string> currentVariables = variablesDataGridView.Rows.Cast<DataGridViewRow>().Select(p => p.Cells[1].Value?.ToString()).Distinct().ToList();
            List<string> keysToRemove = m_variablesD.Keys.Where(p => !currentVariables.Contains(p)).ToList();

            // Update variables dictionary
            foreach (var k in keysToRemove)
                m_variablesD.Remove(k);

            // Add new rows with detected variables
            foreach (var v in SuperSqlQuery.GetVariablesFromString(sqlEditorScintilla.Text))
            {
                if (!m_variablesD.Keys.Contains(v))
                    m_variablesD.Add(v, null);
            }

            foreach (var d in m_variablesD.Where(p => !currentVariables.Contains(p.Key)))
                variablesDataGridView.Rows.Add("List", d.Key, "Edit", d.Value?.Count ?? 0);
        }

        private void RefreshRunningQueriesDataGridView()
        {
            if (this.IsDisposed || this.Disposing || this == null)
                return;

            this.Invoke(new Action(() =>
            {
                if (objectsAndVariablesTabControl.SelectedTab == runningQueriesTabPage)
                    if (m_sqlManager.SqlElements.Count == runningQueriesDataGridView.RowCount)
                    {
                        for (int i = 0; i < m_sqlManager.SqlElements.Count; i++)
                        {
                            string time = $"{Math.Floor(DateTime.Now.Subtract((DateTime)m_sqlManager.SqlElements[i].m_startTime).TotalMinutes)} min";
                            if (runningQueriesDataGridView.Rows[i].Cells[2].Value.ToString() != time)
                                runningQueriesDataGridView.Rows[i].Cells[2].Value = time;
                        }
                    }
                    else
                    {
                        runningQueriesDataGridView.Rows.Clear();
                        foreach (SqlElement element in m_sqlManager.SqlElements)
                        {
                            runningQueriesDataGridView.Rows.Add("Cancel", element.Name ?? "Query name", $"{(DateTime.UtcNow - element.m_startTime).Value.Minutes} min", "Query");
                        }
                    }
            }));
        }

        private void runningQueriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && runningQueriesDataGridView.RowCount == m_sqlManager.SqlElements.Count)
            {
                if (m_sqlManager.SqlElements[e.RowIndex].TryToCancelQuery())
                {
                    m_sqlManager.CancelCmd(m_sqlManager.SqlElements[e.RowIndex].Cmd);
                }
            }
            else if (e.ColumnIndex == 3 && runningQueriesDataGridView.RowCount == m_sqlManager.SqlElements.Count)
            {
                string msg = FormatQueryDetailsMessage(m_sqlManager.SqlElements[e.RowIndex]);
                MessageBoxForm messageBox = new MessageBoxForm(msg, "Query", true);
                messageBox.Show();
            }
        }

        private string FormatQueryDetailsMessage(SqlElement sqlElement)
        {
            // Use string interpolation and better formatting for readability
            string message = $@"
                Server Type: {sqlElement.ServerType}
                Database: {sqlElement.DbName}
                Elapsed Time: {(DateTime.UtcNow - sqlElement.m_startTime).Value.Minutes} minutes

                {m_querySeparator}

                -- Query:

                {((string)sqlElement.Cmd.CommandText).RemoveLeadingTabsMultiline()}";

            return message.RemoveLeadingTabsMultiline(); // Remove any trailing newlines or whitespace
        }

        private void AutoSaveEditorState()
        {
            if (this.ContainsFocus)
            {
                SaveEditorState();
            }
        }

        private void SetupAutoSaveTimer(int interval)
        {
            m_autoSaveTimer = new Timer();
            m_autoSaveTimer.Interval = interval; // 10 seconds
            m_autoSaveTimer.Tick += (s, e) => AutoSaveEditorState();
            m_autoSaveTimer.Start();
        }

        private void ResetAutoSaveTimer()
        {
            if (m_autoSaveTimer != null)
            {
                m_autoSaveTimer.Stop();
                m_autoSaveTimer.Start();
            }
        }

        private void SaveEditorState()
        {
            // Reset the timer
            ResetAutoSaveTimer();
            if (this.IsDisposed || this.Disposing || this == null)
                return;

            this.Invoke(new Action(() =>
            {
                lock (saveLock)
                {
                    if (FileManager.EnsureDirectoryExists(FileManager.SqlEditorBackupPath))
                    {
                        string saveFile = Path.Combine(FileManager.SqlEditorBackupPath, m_backupName);

                        var save = new EditorState(sqlEditorScintilla.Text, m_sqlManager.SqlElements.Select(e => new SqlElementDto(e.Cmd.CommandText, e.ServerType, e.DbName)).ToList(), sqlEditorScintilla.WrapMode, pasteToDataTableCheckBox.Checked);

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            WriteIndented = true
                        };

                        string json = JsonSerializer.Serialize(save, typeof(EditorState), options);
                        if (json != null && saveFile != null)
                            File.WriteAllText(saveFile, json);
                    }
                }
            }));
        }

        private void LoadEditorState(string saveFile)
        {

            if (!File.Exists(saveFile))
                return;

            string json = File.ReadAllText(saveFile);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            EditorState save;
            try
            {
                save = JsonSerializer.Deserialize<EditorState>(json, options);
            }
            catch
            {
                MessageBox.Show("Failed to load editor state. The backup file might be corrupted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (save == null)
                return;

            sqlEditorScintilla.Text = save.Query ?? string.Empty;

            if (Enum.TryParse<ScintillaNET.WrapMode>(save.WrapMode.ToString(), out var wrapMode))
                sqlEditorScintilla.WrapMode = wrapMode;

            pasteToDataTableCheckBox.Checked = save.PasteToDataTable;

            // rerun queries if there were any
            List<SqlElementDto> sqlElementsDto = save.SqlElementsDto;
            if (sqlElementsDto != null && sqlElementsDto.Count > 0)
            {
                var result = MessageBox.Show($"There {(sqlElementsDto.Count == 1 ? "was" : $"were {sqlElementsDto.Count}")} quer{(sqlElementsDto.Count == 1 ? "y" : "ies")} from last session.\nDo you want to run them or copy? Click:\n-No for copy\n-Yes to try to run\n-Cancel", "Queries from last session", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string errorQueries = string.Empty;
                    var connDicSqlServer = FileManager.GetSqlServerConnectionValues();
                    var connDicOracle = FileManager.GetOracleConnectionValues();

                    foreach (var sqlElementDto in sqlElementsDto)
                    {

                        SqlConn sqlConn;
                        if (sqlElementDto.ServerType == SqlServerManager.ServerType.SqlServer)
                            connDicSqlServer.TryGetValue(sqlElementDto.DbName, out sqlConn);
                        else if (sqlElementDto.ServerType == SqlServerManager.ServerType.Oracle)
                            connDicOracle.TryGetValue(sqlElementDto.DbName, out sqlConn);
                        else
                            sqlConn = null;

                        if (sqlConn == null || !sqlConn.Test())
                        {
                            errorQueries += $"{m_querySeparator}{Environment.NewLine}{m_querySeparator}{Environment.NewLine}-- Server type: {sqlElementDto.ServerType.ToString()}{Environment.NewLine}-- DB: {sqlElementDto.DbName}{Environment.NewLine}{m_querySeparator}{Environment.NewLine}-- Query:{Environment.NewLine}{sqlElementDto.CommandText}{Environment.NewLine}{m_querySeparator}{Environment.NewLine}{Environment.NewLine}";
                            continue;
                        }

                        if (++RunningQueries == 1)
                            Text = $"{FormTitle} [{RunningQueries}] running queries";
                        else
                            Text = Regex.Replace(Text, @"\s\[\d+\]\srunning\squeries", $" [{RunningQueries}] running queries");

                        RunQueryToDataTable(sqlElementDto.CommandText, sqlConn.Clone());
                    }

                    if (!string.IsNullOrEmpty(errorQueries))
                    {
                        Clipboard.SetText(errorQueries);
                        MessageBox.Show("Some queries couldn't run so they got copied to clipboard!", "Some queries not run", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (result == DialogResult.No)
                {
                    string queries = string.Empty;
                    foreach (var sqlElementDto in sqlElementsDto)
                    {
                        queries += $"{m_querySeparator}{Environment.NewLine}{m_querySeparator}{Environment.NewLine}-- Server type: {sqlElementDto.ServerType.ToString()}{Environment.NewLine}-- DB: {sqlElementDto.DbName}{Environment.NewLine}{m_querySeparator}{Environment.NewLine}-- Query:{Environment.NewLine}{sqlElementDto.CommandText}{Environment.NewLine}{m_querySeparator}{Environment.NewLine}{Environment.NewLine}";
                    }
                    Clipboard.SetText(queries);
                }
            }
        }

        public class EditorState
        {
            public string Query { get; set; }
            public List<SqlElementDto> SqlElementsDto { get; set; }
            public ScintillaNET.WrapMode WrapMode { get; set; }
            public bool PasteToDataTable { get; set; }

            // Constructor with matching parameter names to properties
            [System.Text.Json.Serialization.JsonConstructor]
            public EditorState(string query, List<SqlElementDto> sqlElementsDto, ScintillaNET.WrapMode wrapMode, bool pasteToDataTable)
            {
                Query = query;
                SqlElementsDto = sqlElementsDto;
                WrapMode = wrapMode;
                PasteToDataTable = pasteToDataTable;
            }
        }


        public class SqlElementDto
        {
            public string CommandText { get; set; }
            public SqlServerManager.ServerType ServerType { get; set; }
            public string DbName { get; set; }

            public SqlElementDto(string commandText, SqlServerManager.ServerType serverType, string dbName)
            {
                CommandText = commandText;
                ServerType = serverType;
                DbName = dbName;
            }
        }

    }
}
