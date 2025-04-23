using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ScintillaNET;
using SQL_Extractor_for_Excel.Forms;
using SQL_Extractor_for_Excel.Scripts;
using static SQL_Extractor_for_Excel.Forms.QueryPickerForm;
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
        public SqlServerManager.ServerType? ServerType;
        public SqlConn SqlConn;
        public static string DefaultSheetName = "Sql Query";
        public static string NewSheetName;
        public bool PasteHeaders => headersCheckBox.Checked;
        public bool PasteToSelection => pasteResultsToSelectionCheckBox.Checked;

        private readonly string m_guid;
        private readonly string m_backupName;
        private readonly object saveLock = new object();
        private DataTable m_shortcutsDT;
        private SqlServerManager m_sqlManager;
        private Timer m_timer;
        private Timer m_autoSaveTimer;
        private List<string> m_tablesListBoxAllItemsList = new List<string>();
        private List<string> m_tablesListBoxSelectedItemsList = new List<string>();
        private List<string> m_fieldsListBoxAllItemsList = new List<string>();
        private List<string> m_fieldsListBoxSelectedItemsList = new List<string>();

        private string m_sqlKeywords;
        private string m_fieldsKeywords;
        private string m_tablesKeywords;
        private List<string> m_sqlKeywordList = new List<string>();
        private List<string> m_fieldsKeywordList => m_fieldsListBoxAllItemsList;
        private List<string> m_tablesKeywordList => m_tablesListBoxAllItemsList;
        private int m_autoCStartPosition = -1; // Position in text where AC was triggered/started
        private char m_currentTriggerChar = '\0';// Store the character that triggered the list (if '.' or '#')
        private string m_autoCSelectedWord = string.Empty;
        private bool m_autoCWasActiveOnBackspace = false;

        private Dictionary<string, SqlConn> m_connDic;
        private Dictionary<string, string> m_queriesDic;
        private bool m_selectionChangedByCode = false;
        private static readonly string m_sheetNameTextBoxPlaceholder = "Worksheet name";
        private static readonly string m_listBoxFetchingText = "Fetching ...";
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


        public SqlEditorForm(Excel.Application app, string saveFile = null, string startQuery = null)
        {
            //ScintillaFix.CopyNativeFolderIfNotExistOrDifferentFixForScintillaBug();
            InitializeComponent();
            m_sqlKeywords = FileManager.SqlKeywords.ToUpper();
            m_sqlKeywordList = m_sqlKeywords.Split(' ').ToList();

            m_guid = Guid.NewGuid().ToString();
            m_backupName = $"{Text}_{m_guid}.json";

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"{Text} v{Utils.GetVersionString(version)}";
            FormTitle = Text;

            m_sqlManager = new SqlServerManager();
            App = app;
            TopMost = true;

            serverTypeComboBox.Items.AddRange(Directory.EnumerateDirectories(FileManager.SqlQueriesPath).Select(p => Path.GetFileName(p)).ToArray());

            UtilsScintilla.SetupSqlEditor(sqlEditorScintilla);

            if (!string.IsNullOrWhiteSpace(startQuery))
                sqlEditorScintilla.Text = startQuery;

            serverTypeComboBox.ContextMenuStrip = new ContextMenuStrip();
            serverTypeComboBox.ContextMenuStrip.Items.Add("Add Server Connection").Click += (sender, e) => { SqlServerManager.AddSqlConnection(); RefreshSavedQueriesComboBox(serverComboBox.SelectedItem?.ToString()); };

            serverComboBox.ContextMenuStrip = new ContextMenuStrip();
            serverComboBox.ContextMenuStrip.Items.Add("Add Server Connection").Click += (sender, e) => { SqlServerManager.AddSqlConnection(); RefreshSavedQueriesComboBox(serverComboBox.SelectedItem?.ToString()); };
            serverComboBox.ContextMenuStrip.Items.Add("Update local password to server").Click += (sender, e) =>
            { PasswordUpdate(); RefreshSavedQueriesComboBox(serverComboBox.SelectedItem?.ToString()); };

            if (!string.IsNullOrEmpty(saveFile))
                LoadEditorState(saveFile);

            sqlEditorScintilla.LostFocus += (o, s) => SaveEditorState();
            //app.WindowDeactivate += (_, w) => SaveEditorState();
            SetupAutoSaveTimer(30000);

            SheetNameTextBoxEnterLeaveSetup();

            SetupTimer();

            m_sqlManager.CommandFinished += RefreshRunningQueriesDataGridView;
            m_sqlManager.CommandFinished += SaveEditorState;

            SetupSqlEditorScintillaContextMenu();
            UtilsScintilla.InitializeScintillaAutocomplete(sqlEditorScintilla);
            sqlEditorScintilla.CharAdded += sqlEditorScintilla_CharAdded;
            sqlEditorScintilla.KeyDown += sqlEditorScintilla_KeyDown;
            sqlEditorScintilla.KeyUp += sqlEditorScintilla_KeyUp2;
            sqlEditorScintilla.AutoCSelectionChange += sqlEditorScintilla_AutoCSelectionChange;
            sqlEditorScintilla.AutoCCompleted += sqlEditorScintilla_AutoCCompleted; // To reset trigger char
            sqlEditorScintilla.AutoCCancelled += sqlEditorScintilla_AutoCCancelled; // To reset trigger char

            try
            {
                string filePath = Path.Combine(FileManager.ResourcesPath, "SqlEditorScintillaShortcuts.txt");
                m_shortcutsDT = Utils.ReadTabDelimitedFile(filePath);
            }
            catch (Exception) { }
        }

        private void DisplayEditorShortcutsForm()
        {
            try
            {
                if (m_shortcutsDT != null)
                {
                    DataTableForm dataTableForm = new DataTableForm(m_shortcutsDT, "No query", App);
                    dataTableForm.TopMost = true;
                    dataTableForm.Show();
                    Utils.MoveFormToCursor(dataTableForm);
                    var DGV = dataTableForm.FindAllChildrenByType<DataGridView>()?.FirstOrDefault();
                    DGV.AutoResizeColumns();
                }
                else
                {
                    MessageBox.Show($"Problem displaing shortcuts table, check if file exists:\n{Path.Combine(FileManager.ResourcesPath, "SqlEditorScintillaShortcuts.txt")}");
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Problem displaing shortcuts table, check if file exists:\n{Path.Combine(FileManager.ResourcesPath, "SqlEditorScintillaShortcuts.txt")}");
            }
        }

        private void sqlEditorScintilla_AutoCSelectionChange(object sender, AutoCSelectionChangeEventArgs e)
        {
            m_autoCSelectedWord = e.Text ?? string.Empty;
        }

        private void sqlEditorScintilla_AutoCCancelled(object sender, EventArgs e)
        {
            if (!m_autoCWasActiveOnBackspace)
                ResetAutoCState();
        }

        private void sqlEditorScintilla_AutoCCompleted(object sender, AutoCSelectionEventArgs e)
        {
            // Capture state *before* cancelling, as AutoCCancelled will clear it
            var originalTrigger = m_currentTriggerChar;
            var originalStartPos = m_autoCStartPosition;
            // Perform deletion *after* cancelling if triggered by . or #
            if (originalTrigger == '.' || originalTrigger == '#')
            {
                var currentPos = sqlEditorScintilla.CurrentPosition;
                int triggerPos = originalStartPos - 1; // Position of the trigger char itself

                // Basic check: Ensure range is valid and trigger char is still there
                if (triggerPos >= 0 && currentPos >= originalStartPos && triggerPos < sqlEditorScintilla.TextLength)
                {
                    if ((char)sqlEditorScintilla.GetCharAt(triggerPos) == originalTrigger)
                    {
                        sqlEditorScintilla.SetSelection(triggerPos, currentPos);
                        sqlEditorScintilla.ReplaceSelection(m_autoCSelectedWord);
                    }
                }
            }
            else
            {
                var currentPos = sqlEditorScintilla.CurrentPosition;
                int triggerPos = originalStartPos;

                // Basic check: Ensure range is valid and trigger char is still there
                if (triggerPos >= 0 && currentPos >= originalStartPos && triggerPos < sqlEditorScintilla.TextLength)
                {
                    sqlEditorScintilla.SetSelection(triggerPos, currentPos);
                    sqlEditorScintilla.ReplaceSelection(m_autoCSelectedWord);
                }
            }

            sqlEditorScintilla.AutoCCancel(); // This triggers AutoCCancelled handler
            ResetAutoCState();
        }

        private void sqlEditorScintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (sqlEditorScintilla.AutoCActive)
            {
                if (e.KeyCode == Keys.Tab)
                {
                    /*sqlEditorScintilla.AutoCComplete();
                    e.Handled = true;
                    e.SuppressKeyPress = true;*/

                    // Capture state *before* cancelling, as AutoCCancelled will clear it
                    var originalTrigger = m_currentTriggerChar;
                    var originalStartPos = m_autoCStartPosition;


                    // Perform deletion *after* cancelling if triggered by . or #
                    if (originalTrigger == '.' || originalTrigger == '#')
                    {
                        var currentPos = sqlEditorScintilla.CurrentPosition;
                        int triggerPos = originalStartPos - 1; // Position of the trigger char itself

                        // Basic check: Ensure range is valid and trigger char is still there
                        if (triggerPos >= 0 && currentPos >= originalStartPos && triggerPos < sqlEditorScintilla.TextLength)
                        {
                            if ((char)sqlEditorScintilla.GetCharAt(triggerPos) == originalTrigger)
                            {
                                sqlEditorScintilla.SetSelection(triggerPos, currentPos);
                                sqlEditorScintilla.ReplaceSelection(m_autoCSelectedWord);
                            }
                        }
                    }
                    else
                    {
                        var currentPos = sqlEditorScintilla.CurrentPosition;
                        int triggerPos = originalStartPos;

                        // Basic check: Ensure range is valid and trigger char is still there
                        if (triggerPos >= 0 && currentPos >= originalStartPos && triggerPos < sqlEditorScintilla.TextLength)
                        {
                            sqlEditorScintilla.SetSelection(triggerPos, currentPos);
                            sqlEditorScintilla.ReplaceSelection(m_autoCSelectedWord);
                        }
                    }

                    sqlEditorScintilla.AutoCCancel(); // This triggers AutoCCancelled handler
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Return)
                {
                    sqlEditorScintilla.AutoCCancel();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    // Capture state *before* cancelling, as AutoCCancelled will clear it
                    var originalTrigger = m_currentTriggerChar;
                    var originalStartPos = m_autoCStartPosition;

                    sqlEditorScintilla.AutoCCancel(); // This triggers AutoCCancelled handler

                    // Perform deletion *after* cancelling if triggered by . or #
                    if (originalTrigger == '.' || originalTrigger == '#')
                    {
                        var currentPos = sqlEditorScintilla.CurrentPosition;
                        int triggerPos = originalStartPos - 1; // Position of the trigger char itself

                        // Basic check: Ensure range is valid and trigger char is still there
                        if (triggerPos >= 0 && currentPos >= originalStartPos && triggerPos < sqlEditorScintilla.TextLength)
                        {
                            if ((char)sqlEditorScintilla.GetCharAt(triggerPos) == originalTrigger)
                            {
                                sqlEditorScintilla.SetSelection(triggerPos, currentPos);
                                sqlEditorScintilla.ReplaceSelection(string.Empty);
                            }
                        }

                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (sqlEditorScintilla.AutoCActive)
                        m_autoCWasActiveOnBackspace = true;
                }
            }
        }

        private void sqlEditorScintilla_KeyUp2(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && m_autoCWasActiveOnBackspace)
            {
                var currentPos = sqlEditorScintilla.CurrentPosition;
                // If a list is active, filter it based on text typed since AC started
                // Ensure start position is valid before trying to get text range
                if (m_autoCStartPosition < 0 || currentPos < m_autoCStartPosition)
                {
                    sqlEditorScintilla.AutoCCancel();
                    m_autoCWasActiveOnBackspace = false;
                    return;
                }

                string filterText = sqlEditorScintilla.GetTextRange(m_autoCStartPosition, currentPos - m_autoCStartPosition);

                List<string> sourceList = GetCurrentSourceList();
                sourceList.SortStrings(filterText);
                if (sourceList != null)
                {
                    ShowFilteredList(sourceList, filterText);
                }
                else
                {
                    sqlEditorScintilla.AutoCCancel();
                }
            }
            else
                m_autoCWasActiveOnBackspace = false;
        }

        private void sqlEditorScintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            if (sqlEditorScintilla.SelectedText.Length > 0)
            {
                // If user starts selecting text, cancel any active list
                if (sqlEditorScintilla.AutoCActive)
                    sqlEditorScintilla.AutoCCancel();
                return;
            }

            var currentPos = sqlEditorScintilla.CurrentPosition;

            // --- Handle Trigger Characters ---
            if (!sqlEditorScintilla.AutoCActive)
            {
                if (e.Char == '.')
                {
                    m_currentTriggerChar = '.';
                    // Start position is *after* the trigger character
                    m_autoCStartPosition = currentPos;
                    // Show full list initially, allow manual filtering for subsequent chars
                    ShowFilteredList(m_fieldsKeywordList, ""); // Assumes m_fieldsKeywordList is populated
                    return;
                }
                else if (e.Char == '#')
                {
                    m_currentTriggerChar = '#';
                    m_autoCStartPosition = currentPos;
                    ShowFilteredList(m_tablesKeywordList, ""); // Assumes m_tablesKeywordList is populated
                    return;
                }
            }

            // --- Handle Filtering or SQL Keyword Trigger ---
            if (sqlEditorScintilla.AutoCActive)
            {
                // If a list is active, filter it based on text typed since AC started
                // Ensure start position is valid before trying to get text range
                if (m_autoCStartPosition < 0 || currentPos < m_autoCStartPosition)
                {
                    sqlEditorScintilla.AutoCCancel();
                    return;
                }

                string filterText = sqlEditorScintilla.GetTextRange(m_autoCStartPosition, currentPos - m_autoCStartPosition);

                List<string> sourceList = GetCurrentSourceList();
                sourceList.SortStrings(filterText);
                if (sourceList != null)
                {
                    ShowFilteredList(sourceList, filterText);
                }
                else
                {
                    sqlEditorScintilla.AutoCCancel();
                }
            }
            else
            {
                // List is NOT active, check if we should trigger SQL keywords
                if (char.IsLetterOrDigit((char)e.Char) || e.Char == '_')
                {
                    var wordStartPos = sqlEditorScintilla.WordStartPosition(currentPos, true);
                    // Check if immediately preceded by a trigger char (we shouldn't trigger SQL keywords then)
                    if (wordStartPos > 0)
                    {
                        char precedingChar = (char)sqlEditorScintilla.GetCharAt(wordStartPos - 1);
                        if (precedingChar == '.' || precedingChar == '#')
                        {
                            return; // Don't trigger SQL keywords right after . or #
                        }
                    }

                    string currentWord = sqlEditorScintilla.GetTextRange(wordStartPos, currentPos - wordStartPos);

                    if (currentWord.Length >= 1) // Or set minimum length (e.g., 2)
                    {
                        m_currentTriggerChar = '\0'; // Indicate SQL keywords context
                        m_autoCStartPosition = wordStartPos; // Start pos is the beginning of the keyword
                        var tempList = m_sqlKeywordList;
                        tempList.SortStrings(currentWord);
                        ShowFilteredList(tempList, currentWord);
                    }
                }
            }
        }

        private void ResetAutoCState()
        {
            m_currentTriggerChar = '\0';
            m_autoCStartPosition = -1;
        }

        private List<string> GetCurrentSourceList()
        {
            switch (m_currentTriggerChar)
            {
                case '.': return m_fieldsKeywordList;
                case '#': return m_tablesKeywordList;
                case '\0': // SQL Keywords context
                           // Ensure start position is valid for SQL context
                    return (m_autoCStartPosition != -1) ? m_sqlKeywordList : null;
                default: return null;
            }
        }

        // Central method to filter and show/update the AC list
        private void ShowFilteredList(List<string> sourceList, string filterText)
        {
            if (sourceList == null)
            {
                if (sqlEditorScintilla.AutoCActive)
                    sqlEditorScintilla.AutoCCancel();
                return;
            }

            // Perform case-insensitive "Contains" filtering
            var filteredList = sourceList
                .Where(item => item.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .ToList(); // Use ToList to materialize the results

            if (filteredList.Any())
            {
                string listString = string.Join(sqlEditorScintilla.AutoCSeparator.ToString(), filteredList);
                int lengthEntered = filterText.Length;
                sqlEditorScintilla.AutoCShow(0, listString);
            }
            else
            {
                // No items match
                if (sqlEditorScintilla.AutoCActive)
                {
                    sqlEditorScintilla.AutoCCancel(); // Explicitly cancel if filter yields nothing
                }
            }
        }

        private void PasswordUpdate()
        {
            SqlServerManager.ServerType serverType = (SqlServerManager.ServerType)Enum.Parse(typeof(SqlServerManager.ServerType), serverTypeComboBox.SelectedItem?.ToString());
            if (!Enum.IsDefined(typeof(SqlServerManager.ServerType), serverType) || string.IsNullOrWhiteSpace(serverComboBox.SelectedItem?.ToString()))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                return;
            }

            Form.ActiveForm.TopMost = false;
            SqlServerPasswordUpdateForm sqlServerPasswordUpdateForm = new SqlServerPasswordUpdateForm(serverType, serverComboBox.SelectedItem?.ToString());
            var result = sqlServerPasswordUpdateForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                switch (serverType)
                {
                    case SqlServerManager.ServerType.SqlServer:
                        this.TopMost = false;
                        m_connDic = FileManager.GetSqlServerConnectionValues();
                        this.TopMost = true;
                        break;
                    case SqlServerManager.ServerType.Oracle:
                        this.TopMost = false;
                        m_connDic = FileManager.GetOracleConnectionValues();
                        this.TopMost = true;
                        break;
                    default:
                        return;
                }
                RefreshSavedQueriesComboBox(serverComboBox.SelectedItem?.ToString());
            }
            Form.ActiveForm.TopMost = true;
        }

        private void SheetNameTextBoxEnterLeaveSetup()
        {
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
        }

        private void SetupSqlEditorScintillaContextMenu()
        {
            ContextMenu cm = sqlEditorScintilla.ContextMenu ?? new ContextMenu();

            MenuItem formatSqlBySqlFluffCMI = new MenuItem("Format by SQLFluff", (o, e) => FormatSelectionUsingSqlFluff());
            MenuItem fetchCMI = new MenuItem("Fetch", (o, e) => Fetch());
            MenuItem runSelectionCMI = new MenuItem("Run selected", (o, e) => RunSelection());
            MenuItem runBlockCMI = new MenuItem("Run block (block identifier '-----')", (o, e) => { UtilsScintilla.SelectBlock(sqlEditorScintilla); RunSelection(); });
            MenuItem shortcutsLookupCMI = new MenuItem("Keyboard shortcuts", (o, e) => DisplayEditorShortcutsForm());
            cm.MenuItems.Add(formatSqlBySqlFluffCMI);
            cm.MenuItems.Add(fetchCMI);
            cm.MenuItems.Add(runSelectionCMI);
            cm.MenuItems.Add(runBlockCMI);
            cm.MenuItems.Add(shortcutsLookupCMI);
            sqlEditorScintilla.ContextMenu = cm;
        }

        private void SetupTimer()
        {
            m_timer = new Timer();
            m_timer.Interval = 500;
            m_timer.Tick += (t, v) => RefreshRunningQueriesDataGridView();
            m_timer.Start();
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

            SaveEditorState();
        }

        private void ToggleTopMost()
        {
            this.TopMost = !this.TopMost;
        }

        private void LaunchQueryPickerForm()
        {
            using (var form = new QueryPickerForm(FileManager.SqlQueriesPath, ServerType))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string text = form.SelectedText;
                    PasteType pasteType = form.SelectedPasteType;

                    switch (pasteType)
                    {
                        case PasteType.PasteBelow:
                            sqlEditorScintilla.Text = sqlEditorScintilla.Text.TrimEnd('\n', '\r', '\t', ' ');
                            int position = sqlEditorScintilla.Lines.Last().Position;
                            // Append the new query with separator
                            sqlEditorScintilla.AppendText($"{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}{text}{Environment.NewLine}");
                            sqlEditorScintilla.GotoPosition(position); // Move cursor
                            break;

                        case PasteType.Replace:
                            sqlEditorScintilla.Text = text;
                            sqlEditorScintilla.GotoPosition(0); // Move cursor to start
                            break;

                        case PasteType.PasteIntoSelection:
                            sqlEditorScintilla.ReplaceSelection(text);
                            break;

                        case PasteType.OpenInNewWindow:
                            SqlEditorForm newForm = new SqlEditorForm(App, startQuery: text);
                            newForm.Show();
                            break;

                        case PasteType.Cancel:
                        default:
                            break;
                    }
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RefreshServerTypeComboBox(string prevSel = null)
        {
            serverTypeComboBox.Items.AddRange(Directory.EnumerateDirectories(FileManager.SqlQueriesPath).Select(p => Path.GetFileName(p)).ToArray());
            int prevSelIndex = serverTypeComboBox.FindStringExact(prevSel ?? string.Empty);
            if (prevSelIndex > 0)
                serverTypeComboBox.SelectedIndex = prevSelIndex;
        }

        private void RefreshSavedQueriesComboBox(string prevSel = null)
        {
            SqlServerManager.ServerType serverType = (SqlServerManager.ServerType)Enum.Parse(typeof(SqlServerManager.ServerType), serverTypeComboBox.SelectedItem?.ToString());
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

            int prevSelIndex = serverComboBox.FindStringExact(prevSel ?? string.Empty);
            if (prevSelIndex > 0)
                serverComboBox.SelectedIndex = prevSelIndex;
        }

        private void FormatSelectionUsingSqlFluff()
        {
            string formattedText = null;
            try
            {
                sqlEditorScintilla.Enabled = false;
                sqlEditorScintilla.ReadOnly = true;
                this.UseWaitCursor = true;
                switch (ServerType)
                {
                    case SqlServerManager.ServerType.Oracle:
                        formattedText = SqlFormatter.Format(sqlEditorScintilla.SelectedText, SqlFormatter.SqlDialect.Oracle) ?? "Formatting failed";
                        break;
                    case SqlServerManager.ServerType.SqlServer:
                        formattedText = SqlFormatter.Format(sqlEditorScintilla.SelectedText, SqlFormatter.SqlDialect.TSql) ?? "Formatting failed";
                        break;
                    default:
                        formattedText = SqlFormatter.Format(sqlEditorScintilla.SelectedText) ?? "Formatting failed";
                        break;
                }
            }
            catch (Exception ex)
            {
                formattedText = $"Formatting failed: {ex.Message}";
            }
            finally
            {
                sqlEditorScintilla.Enabled = true;
                sqlEditorScintilla.ReadOnly = false;
                sqlEditorScintilla.ReplaceSelection(formattedText ?? $"Formatting failed: no error message");
                this.UseWaitCursor = false;
            }
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
                sqlEditorScintilla.Focus();
                return;
            }

            if (new List<string> { sqlEditorScintilla.SelectedText, SqlConn?.ConnectionString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                sqlEditorScintilla.Focus();
                return;
            }

            switch (SqlConn.Type)
            {
                case SqlServerManager.ServerType.SqlServer:
                    err = SqlServerManager.CheckSqlQuerySyntaxOnline(sqlEditorScintilla.SelectedText, SqlConn, sqlEditorScintilla.LineFromPosition(sqlEditorScintilla.SelectionStart));
                    break;
                case SqlServerManager.ServerType.Oracle:
                    err = SqlServerManager.CheckSqlQuerySyntaxOnline(sqlEditorScintilla.SelectedText, SqlConn, sqlEditorScintilla.SelectionStart);
                    break;
                default:
                    err = SqlServerManager.CheckSqlQuerySyntaxOnline(sqlEditorScintilla.SelectedText, SqlConn, sqlEditorScintilla.SelectionStart);
                    break;
            }

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
            sqlEditorScintilla.Focus();
        }

        private void pasteRngBtn_Click(object sender, EventArgs e)
        {
            Excel.Range rng = App.ActiveWindow.RangeSelection;
            if (rng.Valid())
            {
                string indentation = UtilsScintilla.GetIndentationLevel(sqlEditorScintilla);
                sqlEditorScintilla.ReplaceSelection(string.Join($"\n{indentation}", UtilsExcel.FormatRangeToSqlPattern(rng).Split('\n')));
            }
            sqlEditorScintilla.Focus();
        }

        private void PasteRngAsFilter(Excel.Range rng)
        {
            // Get DataTable on UI thread (COM requires this)
            DataTable dataTable = rng.GetDataTable(true);

            // Generate SQL on background thread
            var generateSqlFilterResult = new Task<string>(() => Utils.GenerateSqlFilter(dataTable));

            generateSqlFilterResult.GetAwaiter().OnCompleted(() =>
            {
                if (this.IsDisposed || this.Disposing || this == null || sqlEditorScintilla == null)
                    return;

                this.Invoke(new Action(() =>
                {
                    try
                    {
                        string rngText = generateSqlFilterResult.Result;
                        if (!string.IsNullOrEmpty(rngText))
                        {
                            // Get the indentation level from the current caret or selection start position
                            using (new ScintillaPauseUpdatesBlock(sqlEditorScintilla))
                            {
                                int position = sqlEditorScintilla.SelectionStart;
                                int lineNumber = sqlEditorScintilla.LineFromPosition(position);
                                int indentation = sqlEditorScintilla.Lines[lineNumber].Indentation;
                                rngText = rngText.UnifyLineEndings();
                                UseWaitCursor = false;
                                sqlEditorScintilla.ReadOnly = false;
                                sqlEditorScintilla.Enabled = true;
                                sqlEditorScintilla.ReplaceSelection(rngText);
                                int endPosition = position + rngText.Length + 1;
                                int lastLine = sqlEditorScintilla.LineFromPosition(endPosition);

                                for (int i = sqlEditorScintilla.LineFromPosition(position) + 1; i <= lastLine; i++)
                                {
                                    sqlEditorScintilla.Lines[i].Indentation = indentation + sqlEditorScintilla.Lines[i].Indentation;
                                    endPosition = sqlEditorScintilla.Lines[i].EndPosition;
                                }

                                sqlEditorScintilla.SetSelection(position, endPosition);

                                // wrap it with ( ... ) when not wrapped
                                using (var reader = new StringReader(rngText))
                                {
                                    string line;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        if (line.Length == 0 || (line[0] != '(' && line[0] != ')' && line[0] != '\t'))
                                        {
                                            UtilsScintilla.WrapIntoSqlBlock(sqlEditorScintilla);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Set editor to not read-only as scintilla can't update text when not not read-only
                        UseWaitCursor = false;
                        sqlEditorScintilla.ReadOnly = false;
                        sqlEditorScintilla.Enabled = true;
                        sqlEditorScintilla.ReplaceSelection($"(Error occurred while creating SQL Filter:\n{ex.Message.ToString()})");
                    }
                }));
            });

            // Set editor to read-only
            sqlEditorScintilla.ReadOnly = true;
            sqlEditorScintilla.Enabled = false;
            UseWaitCursor = true;
            generateSqlFilterResult.Start();
        }

        private void pasteRngFilterBtn_Click(object sender, EventArgs e)
        {
            PasteRngAsFilter();
        }

        private void PasteRngAsFilter()
        {
            Excel.Range rng = App.ActiveWindow.RangeSelection;

            if (!rng.Valid() || rng.Areas.Cast<Excel.Range>().Sum(p => p.Rows.Count) < 2)
            {
                sqlEditorScintilla.ReplaceSelection(string.Empty);
                return;
            }

            PasteRngAsFilter(rng);
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            RunAll();
        }

        private void RunAll()
        {
            Query = sqlEditorScintilla.Text;
            Run(Query);
        }

        private void runSelectionBtn_Click(object sender, EventArgs e)
        {
            RunSelection();
        }

        private void RunSelection()
        {
            Query = sqlEditorScintilla.SelectedText;

            if (string.IsNullOrWhiteSpace(Query))
            {
                UtilsScintilla.SelectBlock(sqlEditorScintilla);
                sqlEditorScintilla.Focus();
                return;
            }

            Run(Query);
        }

        private void Run(string query)
        {
            if (new List<string> { query, SqlConn?.ConnectionString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Run error");
                sqlEditorScintilla.Focus();
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
                    string formName = $"DataTable [ET: {Math.Floor((DateTime.Now.Subtract((DateTime)sqlResult.SqlElement.m_startTime).TotalMinutes))} min]";
                    DataTableForm form = new DataTableForm(sqlResult, query, App, formName, SqlElement.FormatQueryDetailsMessage(sqlResult.SqlElement));
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
                            string formName = $"DataTable [ET: {Math.Floor((DateTime.Now.Subtract((DateTime)sqlResult.SqlElement.m_startTime).TotalMinutes))} min]";
                            DataTableForm form = new DataTableForm(sqlResult, query, App, formName, SqlElement.FormatQueryDetailsMessage(sqlResult.SqlElement));
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
            sqlEditorScintilla.Focus();
        }

        private void testConnBtn_Click(object sender, EventArgs e)
        {
            SqlConn sqlConn;
            if (new List<string> { m_connDic?.ToString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Test connection error");
                return;
            }

            bool result = m_connDic.TryGetValue(m_connDic?.Keys?.FirstOrDefault(p => p == serverComboBox.SelectedItem?.ToString()) ?? "", out sqlConn);
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
                RefreshSavedQueriesComboBox(serverComboBox.SelectedItem?.ToString());
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
                        sqlEditorScintilla.AppendText($"{Environment.NewLine + Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine + Environment.NewLine}{m_queriesDic[m_queriesDic.Keys.First(p => Path.GetFileName(p) == savedQueriesComboBox.SelectedItem?.ToString())]}");
                        sqlEditorScintilla.GotoPosition(position);
                        break;
                    case DialogResult.No:
                        sqlEditorScintilla.Text = m_queriesDic[m_queriesDic.Keys.First(p => Path.GetFileName(p) == savedQueriesComboBox.SelectedItem?.ToString())];
                        break;
                    case DialogResult.Cancel:
                    case DialogResult.None:
                    default:
                        return;
                }
            }
            else
                sqlEditorScintilla.Text = m_queriesDic[m_queriesDic.Keys.First(p => Path.GetFileName(p) == savedQueriesComboBox.SelectedItem?.ToString())];
        }

        private void savedQueriesComboBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                LaunchQueryPickerForm();
        }

        private void serverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var result = m_connDic.TryGetValue((sender as ComboBox).SelectedItem?.ToString(), out SqlConn);
        }

        private void sqlEditorScintilla_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.Shift)
                {
                    if (e.KeyCode == Keys.V)
                    {
                        Excel.Range rng = App.ActiveWindow.RangeSelection;
                        if (rng.Valid())
                        {
                            string indentation = UtilsScintilla.GetIndentationLevel(sqlEditorScintilla);
                            sqlEditorScintilla.ReplaceSelection(string.Join($"\n{indentation}", UtilsExcel.FormatRangeToSqlPattern(rng).Split('\n')));
                        }
                        sqlEditorScintilla.Focus();
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.R)
                    {
                        Query = sqlEditorScintilla.SelectedText;

                        if (string.IsNullOrWhiteSpace(Query))
                        {
                            UtilsScintilla.SelectBlock(sqlEditorScintilla);
                            sqlEditorScintilla.Focus();
                        }
                        else
                        {
                            Run(Query);
                        }
                        e.SuppressKeyPress = true;
                    }

                    if (e.KeyCode == Keys.F)
                    {
                        Excel.Range rng = App.ActiveWindow.RangeSelection;

                        if (!rng.Valid() || rng.Areas.Cast<Excel.Range>().Sum(p => p.Rows.Count) < 2)
                        {
                            sqlEditorScintilla.ReplaceSelection(string.Empty);
                            e.SuppressKeyPress = true;
                            return;
                        }

                        PasteRngAsFilter(rng);
                        e.SuppressKeyPress = true;
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.R)
                    {
                        Query = sqlEditorScintilla.Text;
                        Run(Query);
                        e.SuppressKeyPress = true;
                    }
                }
            }
        }

        private void FetchFields(string tableName, SqlConn sqlConn)
        {
            m_fieldsListBoxAllItemsList.Clear();
            m_fieldsListBoxSelectedItemsList.Clear();
            m_fieldsKeywords = string.Empty;
            fieldsListBox.Items.Clear();
            fieldsListBox.Items.Add(m_listBoxFetchingText);
            fieldsListBox.Update();

            bool tableNameIsQuery = tableName.Contains("select", StringComparison.OrdinalIgnoreCase);
            var sqlResult = SqlServerManager.GetDataFromServer(m_sqlManager, $"SELECT * FROM {(tableNameIsQuery ? "(" : "")}{tableName.Trim()}{(tableNameIsQuery ? ") FIELDS" : "")} WHERE 1=0", sqlConn, 40);
            fieldsListBox.Items.Clear(); // clear "Fetching..." from the list
            if (!sqlResult.HasErrors)
            {
                fieldsListBox.Items.AddRange(sqlResult.DataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).Distinct().ToArray());
                m_fieldsListBoxAllItemsList.AddRange(fieldsListBox.Items.Cast<string>());
                m_fieldsKeywords = string.Join(" ", m_fieldsListBoxAllItemsList) ?? string.Empty;
            }

            objectsAndVariablesTabControl.SelectedTab = fieldsTabPage;
        }

        private void FetchTables(SqlConn sqlConn)
        {
            m_tablesListBoxAllItemsList.Clear();
            m_tablesListBoxSelectedItemsList.Clear();
            m_tablesKeywords = string.Empty;
            tablesListBox.Items.Clear();
            tablesListBox.Items.Add(m_listBoxFetchingText);
            tablesListBox.Update();

            string query = SqlServerManager.GetFetchTablesQuery(sqlConn.Type);
            if (string.IsNullOrEmpty(query))
            {
                tablesListBox.Items.Clear();
                return;
            }

            var sqlResult = SqlServerManager.GetDataFromServer(m_sqlManager, query, sqlConn, 40);
            tablesListBox.Items.Clear(); // clear "Fetching..." from the list
            if (!sqlResult.HasErrors)
            {
                tablesListBox.Items.AddRange(sqlResult.DataTable.AsEnumerable()
                    .Select(row => row.Field<string>(0))
                    .Distinct()
                    .ToArray() ?? new string[1]);
                m_tablesListBoxAllItemsList.AddRange(tablesListBox.Items.Cast<string>());
                m_tablesKeywords = string.Join(" ", m_tablesListBoxAllItemsList) ?? string.Empty;
            }

            objectsAndVariablesTabControl.SelectedTab = tablesTabPage;
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {
            Fetch();
        }

        private void Fetch()
        {
            ListBox listBox = objectsAndVariablesTabControl.SelectedTab.FindAllChildrenByType<ListBox>().FirstOrDefault();

            if (new List<string> { m_connDic?.ToString(), serverComboBox.SelectedItem?.ToString(), serverTypeComboBox.SelectedItem?.ToString() }.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                MessageBox.Show("Missing server selections or query", "Fetch error");
                return;
            }

            SqlConn sqlConn;
            try
            {
                bool result = m_connDic.TryGetValue(m_connDic?.Keys?.FirstOrDefault(p => p.Contains(serverComboBox.SelectedItem?.ToString())), out sqlConn);
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
            sqlEditorScintilla.Focus();
        }

        private void wrapIntoBlockBtn_Click(object sender, EventArgs e)
        {
            UtilsScintilla.WrapIntoSqlBlock(sqlEditorScintilla);
            sqlEditorScintilla.Focus();
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
                TextBox textBox = sender as TextBox;

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
                Regex regx;
                try
                {
                    regx = new Regex(textBox.Text, RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    regx = new Regex(Regex.Escape(textBox.Text), RegexOptions.IgnoreCase);
                }
                if (tables)
                    filteredItems = m_tablesListBoxAllItemsList.Where(item => regx.IsMatch(item) || m_tablesListBoxSelectedItemsList.Contains(item)).ToList();
                else
                    filteredItems = m_fieldsListBoxAllItemsList.Where(item => regx.IsMatch(item) || m_fieldsListBoxSelectedItemsList.Contains(item)).ToList();

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
                    bool result = m_connDic.TryGetValue(m_connDic.Keys.FirstOrDefault(p => p.Contains(serverComboBox.SelectedItem?.ToString())), out sqlConn);
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
                FetchFields(listBox.SelectedItem?.ToString(), sqlConn);
            }
            else if ((objectsAndVariablesTabControl.SelectedTab == fieldsTabPage) && listBox.SelectedItems.Count > 0)
                TransferToQueryFromListbox();
        }

        private void tablesListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBox listBox = (sender as ListBox);

            // Get the item text
            string itemText = listBox.Items[e.Index].ToString();

            // Split the item into left and right parts
            string[] parts = itemText.Split(new char[] { '.' }, 2);
            string leftPart = parts.Length > 0 ? " (" + parts[0] + ")" : string.Empty;
            string rightPart = parts.Length > 1 ? parts[1] : string.Empty;

            if (itemText == m_listBoxFetchingText)
            {
                rightPart = m_listBoxFetchingText;
                leftPart = string.Empty;
            }

            // Calculate alignment using monospaced font
            float dotPositionX = TextRenderer.MeasureText(rightPart, e.Font).Width;

            // Draw background
            e.DrawBackground();

            using (Brush leftBrush = rightPart.Length > 0 ? new SolidBrush(Color.DarkSlateGray) : new SolidBrush(Color.Black))
            using (Brush rightBrush = new SolidBrush(Color.Black))
            {
                // Draw the left part (gray)
                e.Graphics.DrawString(rightPart, e.Font, rightBrush, e.Bounds.Left, e.Bounds.Top);

                // Draw the right part (black)
                e.Graphics.DrawString(leftPart, e.Font, leftBrush, dotPositionX, e.Bounds.Top);
            }

            // Draw focus rectangle if needed
            e.DrawFocusRectangle();
        }

        private void SqlEditorForm_Activated(object sender, EventArgs e)
        {
            this.Opacity = 0.95;
            Utils.EnsureWindowIsVisible(this);
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
            UtilsScintilla.ReformatTextToSqlFilter(sqlEditorScintilla);
            sqlEditorScintilla.Focus();
        }

        private void separateBtn_Click(object sender, EventArgs e)
        {
            sqlEditorScintilla.ReplaceSelection($"{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}");
            sqlEditorScintilla.Focus();
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
            foreach (var row in variablesDataGridView.Rows.Cast<DataGridViewRow>().Where(p => !p.IsNewRow && p.Visible && string.IsNullOrEmpty(p.Cells[1].Value?.ToString())))
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
                            runningQueriesDataGridView.Rows.Add("Cancel", element.Name ?? "Query name", $"{Math.Floor((DateTime.Now.Subtract((DateTime)element.m_startTime).TotalMinutes))} min", "Query");
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
                string msg = SqlElement.FormatQueryDetailsMessage(m_sqlManager.SqlElements[e.RowIndex]);
                MessageBoxForm messageBox = new MessageBoxForm(msg, "Query", true);
                messageBox.Show();
            }
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
                            errorQueries += $"{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}-- Server type: {sqlElementDto.ServerType.ToString()}{Environment.NewLine}-- DB: {sqlElementDto.DbName}{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}-- Query:{Environment.NewLine}{sqlElementDto.CommandText}{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}{Environment.NewLine}";
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
                        queries += $"{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}-- Server type: {sqlElementDto.ServerType.ToString()}{Environment.NewLine}-- DB: {sqlElementDto.DbName}{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}-- Query:{Environment.NewLine}{sqlElementDto.CommandText}{Environment.NewLine}{UtilsScintilla.ScintillaSqlQuerySeparator}{Environment.NewLine}{Environment.NewLine}";
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
