using System;
using System.Windows.Forms;

namespace SQL_Extractor_for_Excel
{
    partial class SqlEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlEditorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.sqlEditorScintilla = new ScintillaNET.Scintilla();
            this.transferTablesToQueryBtn = new System.Windows.Forms.Button();
            this.fetchTablesBtn = new System.Windows.Forms.Button();
            this.tablesListBox = new System.Windows.Forms.ListBox();
            this.searchTablesTextBox = new System.Windows.Forms.TextBox();
            this.pasteResultsToSelectionCheckBox = new System.Windows.Forms.CheckBox();
            this.sheetNameTextBox = new System.Windows.Forms.TextBox();
            this.fillSheetNameBtn = new System.Windows.Forms.Button();
            this.headersCheckBox = new System.Windows.Forms.CheckBox();
            this.clearEditorLabel = new System.Windows.Forms.Label();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.formatToSqlBtn = new System.Windows.Forms.Button();
            this.separateBtn = new System.Windows.Forms.Button();
            this.validateSelectionBtn = new System.Windows.Forms.Button();
            this.pasteRngBtn = new System.Windows.Forms.Button();
            this.commentBtn = new System.Windows.Forms.Button();
            this.serverTypeComboBox = new System.Windows.Forms.ComboBox();
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.testConnBtn = new System.Windows.Forms.Button();
            this.openInNotepadBtn = new System.Windows.Forms.Button();
            this.runSelectionBtn = new System.Windows.Forms.Button();
            this.validateBtn = new System.Windows.Forms.Button();
            this.pasteRngFilterBtn = new System.Windows.Forms.Button();
            this.wrapIntoBlockBtn = new System.Windows.Forms.Button();
            this.savedQueriesComboBox = new System.Windows.Forms.ComboBox();
            this.saveQueryBtn = new System.Windows.Forms.Button();
            this.runBtn = new System.Windows.Forms.Button();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.upperTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.parametersTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.worksheetTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pasteToDataTableCheckBox = new System.Windows.Forms.CheckBox();
            this.objectsAndVariablesTabControl = new System.Windows.Forms.TabControl();
            this.tablesTabPage = new System.Windows.Forms.TabPage();
            this.tablesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tablesButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.fieldsTabPage = new System.Windows.Forms.TabPage();
            this.fieldsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.searchFieldsTextBox = new System.Windows.Forms.TextBox();
            this.fieldsListBox = new System.Windows.Forms.ListBox();
            this.fieldsButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.transferFieldsToQueryBtn = new System.Windows.Forms.Button();
            this.fetchFieldsBtn = new System.Windows.Forms.Button();
            this.variablesTabPage = new System.Windows.Forms.TabPage();
            this.variablesDataGridView = new System.Windows.Forms.DataGridView();
            this.variableValuesCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VariableValuesColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.VariableInstancesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runningQueriesTabPage = new System.Windows.Forms.TabPage();
            this.runningQueriesDataGridView = new System.Windows.Forms.DataGridView();
            this.CancelQueryColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.QueryNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QueryColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mainTableLayoutPanel.SuspendLayout();
            this.buttonsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.upperTableLayoutPanel.SuspendLayout();
            this.parametersTableLayoutPanel.SuspendLayout();
            this.worksheetTableLayoutPanel.SuspendLayout();
            this.objectsAndVariablesTabControl.SuspendLayout();
            this.tablesTabPage.SuspendLayout();
            this.tablesTableLayoutPanel.SuspendLayout();
            this.tablesButtonsTableLayoutPanel.SuspendLayout();
            this.fieldsTabPage.SuspendLayout();
            this.fieldsTableLayoutPanel.SuspendLayout();
            this.fieldsButtonsTableLayoutPanel.SuspendLayout();
            this.variablesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.variablesDataGridView)).BeginInit();
            this.runningQueriesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runningQueriesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // sqlEditorScintilla
            // 
            this.sqlEditorScintilla.AllowDrop = true;
            this.sqlEditorScintilla.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
            this.sqlEditorScintilla.CaretForeColor = System.Drawing.Color.White;
            this.sqlEditorScintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqlEditorScintilla.LexerName = "sql";
            this.sqlEditorScintilla.Location = new System.Drawing.Point(3, 48);
            this.sqlEditorScintilla.Name = "sqlEditorScintilla";
            this.sqlEditorScintilla.Size = new System.Drawing.Size(734, 616);
            this.sqlEditorScintilla.TabIndex = 9;
            this.sqlEditorScintilla.Text = "SELECT * FROM";
            this.sqlEditorScintilla.UseTabs = true;
            this.sqlEditorScintilla.WrapIndentMode = ScintillaNET.WrapIndentMode.Indent;
            this.sqlEditorScintilla.WrapMode = ScintillaNET.WrapMode.Word;
            this.sqlEditorScintilla.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sqlEditorScintilla_KeyUp);
            this.sqlEditorScintilla.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sqlEditorScintilla_MouseDoubleClick);
            // 
            // transferTablesToQueryBtn
            // 
            this.transferTablesToQueryBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transferTablesToQueryBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transferTablesToQueryBtn.Location = new System.Drawing.Point(3, 3);
            this.transferTablesToQueryBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.transferTablesToQueryBtn.Name = "transferTablesToQueryBtn";
            this.transferTablesToQueryBtn.Size = new System.Drawing.Size(49, 26);
            this.transferTablesToQueryBtn.TabIndex = 19;
            this.transferTablesToQueryBtn.Text = "←";
            this.toolTip.SetToolTip(this.transferTablesToQueryBtn, "Paste to the selection selected tables from the list");
            this.transferTablesToQueryBtn.UseVisualStyleBackColor = true;
            this.transferTablesToQueryBtn.Click += new System.EventHandler(this.transferToQueryBtn_Click);
            // 
            // fetchTablesBtn
            // 
            this.fetchTablesBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fetchTablesBtn.Location = new System.Drawing.Point(58, 3);
            this.fetchTablesBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.fetchTablesBtn.Name = "fetchTablesBtn";
            this.fetchTablesBtn.Size = new System.Drawing.Size(162, 26);
            this.fetchTablesBtn.TabIndex = 18;
            this.fetchTablesBtn.Text = "Fetch";
            this.toolTip.SetToolTip(this.fetchTablesBtn, "Fetch all tables and views from the given database (selection must be empty)");
            this.fetchTablesBtn.UseVisualStyleBackColor = true;
            this.fetchTablesBtn.Click += new System.EventHandler(this.fetchBtn_Click);
            // 
            // tablesListBox
            // 
            this.tablesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.tablesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablesListBox.FormattingEnabled = true;
            this.tablesListBox.HorizontalScrollbar = true;
            this.tablesListBox.ItemHeight = 12;
            this.tablesListBox.Location = new System.Drawing.Point(3, 30);
            this.tablesListBox.Name = "tablesListBox";
            this.tablesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.tablesListBox.Size = new System.Drawing.Size(217, 573);
            this.tablesListBox.TabIndex = 20;
            this.tablesListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tablesListBox_DrawItem);
            this.tablesListBox.SelectedIndexChanged += new System.EventHandler(this.objectsListBox_SelectedIndexChanged);
            this.tablesListBox.DoubleClick += new System.EventHandler(this.objectsListBox_DoubleClick);
            // 
            // searchTablesTextBox
            // 
            this.searchTablesTextBox.AcceptsReturn = true;
            this.searchTablesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchTablesTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTablesTextBox.Location = new System.Drawing.Point(3, 3);
            this.searchTablesTextBox.Name = "searchTablesTextBox";
            this.searchTablesTextBox.Size = new System.Drawing.Size(217, 21);
            this.searchTablesTextBox.TabIndex = 24;
            this.searchTablesTextBox.Text = "Search";
            this.toolTip.SetToolTip(this.searchTablesTextBox, "Search tables (you can use Regex) (selections on the list are preserved)");
            this.searchTablesTextBox.WordWrap = false;
            this.searchTablesTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyDown);
            // 
            // pasteResultsToSelectionCheckBox
            // 
            this.pasteResultsToSelectionCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pasteResultsToSelectionCheckBox.Location = new System.Drawing.Point(324, 3);
            this.pasteResultsToSelectionCheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.pasteResultsToSelectionCheckBox.Name = "pasteResultsToSelectionCheckBox";
            this.pasteResultsToSelectionCheckBox.Size = new System.Drawing.Size(180, 33);
            this.pasteResultsToSelectionCheckBox.TabIndex = 25;
            this.pasteResultsToSelectionCheckBox.Text = "Paste result to selection";
            this.toolTip.SetToolTip(this.pasteResultsToSelectionCheckBox, "When checked it will paste the result to cell that was selected when the query wa" +
        "s run");
            this.pasteResultsToSelectionCheckBox.UseVisualStyleBackColor = true;
            this.pasteResultsToSelectionCheckBox.CheckedChanged += new System.EventHandler(this.pasteResultsToSelectionCheckBox_CheckedChanged);
            // 
            // sheetNameTextBox
            // 
            this.sheetNameTextBox.AllowDrop = true;
            this.sheetNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sheetNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sheetNameTextBox.Location = new System.Drawing.Point(3, 9);
            this.sheetNameTextBox.Name = "sheetNameTextBox";
            this.sheetNameTextBox.Size = new System.Drawing.Size(154, 21);
            this.sheetNameTextBox.TabIndex = 26;
            this.sheetNameTextBox.Text = "Worksheet name";
            this.toolTip.SetToolTip(this.sheetNameTextBox, "Name of the worksheet that will be created after the query is run");
            this.sheetNameTextBox.Leave += new System.EventHandler(this.sheetNameTextBox_Leave);
            // 
            // fillSheetNameBtn
            // 
            this.fillSheetNameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fillSheetNameBtn.Location = new System.Drawing.Point(163, 8);
            this.fillSheetNameBtn.Name = "fillSheetNameBtn";
            this.fillSheetNameBtn.Size = new System.Drawing.Size(27, 23);
            this.fillSheetNameBtn.TabIndex = 27;
            this.fillSheetNameBtn.Text = "▲";
            this.toolTip.SetToolTip(this.fillSheetNameBtn, "Get selected text as future worksheet name");
            this.fillSheetNameBtn.UseVisualStyleBackColor = true;
            this.fillSheetNameBtn.Click += new System.EventHandler(this.fillSheetNameBtn_Click);
            // 
            // headersCheckBox
            // 
            this.headersCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.headersCheckBox.Checked = true;
            this.headersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.headersCheckBox.Location = new System.Drawing.Point(5, 3);
            this.headersCheckBox.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.headersCheckBox.Name = "headersCheckBox";
            this.headersCheckBox.Size = new System.Drawing.Size(113, 33);
            this.headersCheckBox.TabIndex = 28;
            this.headersCheckBox.Text = "Headers";
            this.toolTip.SetToolTip(this.headersCheckBox, "Preserve headers on paste to Excel");
            this.headersCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearEditorLabel
            // 
            this.clearEditorLabel.AutoSize = true;
            this.clearEditorLabel.BackColor = System.Drawing.Color.White;
            this.clearEditorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clearEditorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearEditorLabel.ForeColor = System.Drawing.Color.IndianRed;
            this.clearEditorLabel.Location = new System.Drawing.Point(703, 0);
            this.clearEditorLabel.Name = "clearEditorLabel";
            this.clearEditorLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.clearEditorLabel.Size = new System.Drawing.Size(28, 39);
            this.clearEditorLabel.TabIndex = 29;
            this.clearEditorLabel.Text = "❌";
            this.clearEditorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.clearEditorLabel, "Clear content of editor");
            this.clearEditorLabel.Click += new System.EventHandler(this.clearEditorLabel_Click);
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTableLayoutPanel.Controls.Add(this.buttonsTableLayoutPanel, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.mainSplitContainer, 0, 0);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(985, 738);
            this.mainTableLayoutPanel.TabIndex = 30;
            // 
            // buttonsTableLayoutPanel
            // 
            this.buttonsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonsTableLayoutPanel.ColumnCount = 9;
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.buttonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.buttonsTableLayoutPanel.Controls.Add(this.formatToSqlBtn, 3, 1);
            this.buttonsTableLayoutPanel.Controls.Add(this.separateBtn, 3, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.validateSelectionBtn, 0, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.pasteRngBtn, 1, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.commentBtn, 2, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.serverTypeComboBox, 4, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.serverComboBox, 5, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.testConnBtn, 6, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.openInNotepadBtn, 7, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.runSelectionBtn, 8, 0);
            this.buttonsTableLayoutPanel.Controls.Add(this.validateBtn, 0, 1);
            this.buttonsTableLayoutPanel.Controls.Add(this.pasteRngFilterBtn, 1, 1);
            this.buttonsTableLayoutPanel.Controls.Add(this.wrapIntoBlockBtn, 2, 1);
            this.buttonsTableLayoutPanel.Controls.Add(this.savedQueriesComboBox, 4, 1);
            this.buttonsTableLayoutPanel.Controls.Add(this.saveQueryBtn, 7, 1);
            this.buttonsTableLayoutPanel.Controls.Add(this.runBtn, 8, 1);
            this.buttonsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.buttonsTableLayoutPanel.Location = new System.Drawing.Point(0, 673);
            this.buttonsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonsTableLayoutPanel.MaximumSize = new System.Drawing.Size(0, 62);
            this.buttonsTableLayoutPanel.Name = "buttonsTableLayoutPanel";
            this.buttonsTableLayoutPanel.RowCount = 2;
            this.buttonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonsTableLayoutPanel.Size = new System.Drawing.Size(985, 62);
            this.buttonsTableLayoutPanel.TabIndex = 24;
            // 
            // formatToSqlBtn
            // 
            this.formatToSqlBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.formatToSqlBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formatToSqlBtn.Location = new System.Drawing.Point(268, 34);
            this.formatToSqlBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.formatToSqlBtn.Name = "formatToSqlBtn";
            this.formatToSqlBtn.Size = new System.Drawing.Size(62, 25);
            this.formatToSqlBtn.TabIndex = 25;
            this.formatToSqlBtn.Text = "( \' \', \' \', ... )";
            this.toolTip.SetToolTip(this.formatToSqlBtn, "(Ctrl + Q) Format the selection as a filter list (use on formatted text to change" +
        " from text values to number values)");
            this.formatToSqlBtn.UseVisualStyleBackColor = true;
            this.formatToSqlBtn.Click += new System.EventHandler(this.formatToSqlBtn_Click);
            // 
            // separateBtn
            // 
            this.separateBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.separateBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.separateBtn.Location = new System.Drawing.Point(268, 3);
            this.separateBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.separateBtn.Name = "separateBtn";
            this.separateBtn.Size = new System.Drawing.Size(62, 25);
            this.separateBtn.TabIndex = 24;
            this.separateBtn.Text = "- - - - -";
            this.toolTip.SetToolTip(this.separateBtn, "(Ctrl + -) Paste query separator into the selection");
            this.separateBtn.UseVisualStyleBackColor = true;
            this.separateBtn.Click += new System.EventHandler(this.separateBtn_Click);
            // 
            // validateSelectionBtn
            // 
            this.validateSelectionBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.validateSelectionBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.validateSelectionBtn.Location = new System.Drawing.Point(3, 3);
            this.validateSelectionBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.validateSelectionBtn.Name = "validateSelectionBtn";
            this.validateSelectionBtn.Size = new System.Drawing.Size(102, 25);
            this.validateSelectionBtn.TabIndex = 3;
            this.validateSelectionBtn.Text = "Validate selection";
            this.toolTip.SetToolTip(this.validateSelectionBtn, "Validates editor\'s selection");
            this.validateSelectionBtn.UseVisualStyleBackColor = true;
            this.validateSelectionBtn.Click += new System.EventHandler(this.validateSelectionBtn_Click);
            // 
            // pasteRngBtn
            // 
            this.pasteRngBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pasteRngBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pasteRngBtn.Location = new System.Drawing.Point(111, 3);
            this.pasteRngBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.pasteRngBtn.Name = "pasteRngBtn";
            this.pasteRngBtn.Size = new System.Drawing.Size(102, 25);
            this.pasteRngBtn.TabIndex = 5;
            this.pasteRngBtn.Text = "Paste range";
            this.toolTip.SetToolTip(this.pasteRngBtn, "(Ctrl + Shift + V) Pastes to the editor\'s selection the selected cells from the a" +
        "ctive workbook as a filter list (no header) or as headers separated by comma");
            this.pasteRngBtn.UseVisualStyleBackColor = true;
            this.pasteRngBtn.Click += new System.EventHandler(this.pasteRngBtn_Click);
            // 
            // commentBtn
            // 
            this.commentBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.commentBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commentBtn.Location = new System.Drawing.Point(219, 3);
            this.commentBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.commentBtn.Name = "commentBtn";
            this.commentBtn.Size = new System.Drawing.Size(43, 25);
            this.commentBtn.TabIndex = 4;
            this.commentBtn.Text = "- - ...";
            this.toolTip.SetToolTip(this.commentBtn, "(Ctrl + Shift + /) Comment/Uncomment selected lines");
            this.commentBtn.UseVisualStyleBackColor = true;
            this.commentBtn.Click += new System.EventHandler(this.commentBtn_Click);
            // 
            // serverTypeComboBox
            // 
            this.serverTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.serverTypeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverTypeComboBox.DropDownHeight = 210;
            this.serverTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverTypeComboBox.FormattingEnabled = true;
            this.serverTypeComboBox.IntegralHeight = false;
            this.serverTypeComboBox.ItemHeight = 15;
            this.serverTypeComboBox.Location = new System.Drawing.Point(336, 3);
            this.serverTypeComboBox.MaxDropDownItems = 15;
            this.serverTypeComboBox.Name = "serverTypeComboBox";
            this.serverTypeComboBox.Size = new System.Drawing.Size(151, 23);
            this.serverTypeComboBox.TabIndex = 8;
            this.toolTip.SetToolTip(this.serverTypeComboBox, "Choose server type (Right click to add server connection)");
            this.serverTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.serverTypeComboBox_SelectedIndexChanged);
            // 
            // serverComboBox
            // 
            this.serverComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.serverComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverComboBox.DropDownHeight = 210;
            this.serverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverComboBox.FormattingEnabled = true;
            this.serverComboBox.IntegralHeight = false;
            this.serverComboBox.ItemHeight = 15;
            this.serverComboBox.Location = new System.Drawing.Point(493, 3);
            this.serverComboBox.MaxDropDownItems = 15;
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(161, 23);
            this.serverComboBox.TabIndex = 16;
            this.toolTip.SetToolTip(this.serverComboBox, "Choose server connection (Right click to add server connection)");
            this.serverComboBox.SelectedIndexChanged += new System.EventHandler(this.serverComboBox_SelectedIndexChanged);
            // 
            // testConnBtn
            // 
            this.testConnBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testConnBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testConnBtn.Location = new System.Drawing.Point(660, 3);
            this.testConnBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.testConnBtn.Name = "testConnBtn";
            this.testConnBtn.Size = new System.Drawing.Size(102, 25);
            this.testConnBtn.TabIndex = 7;
            this.testConnBtn.Text = "Test connection";
            this.toolTip.SetToolTip(this.testConnBtn, "Test connection with database");
            this.testConnBtn.UseVisualStyleBackColor = true;
            this.testConnBtn.Click += new System.EventHandler(this.testConnBtn_Click);
            // 
            // openInNotepadBtn
            // 
            this.openInNotepadBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.openInNotepadBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openInNotepadBtn.Location = new System.Drawing.Point(768, 3);
            this.openInNotepadBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.openInNotepadBtn.Name = "openInNotepadBtn";
            this.openInNotepadBtn.Size = new System.Drawing.Size(102, 25);
            this.openInNotepadBtn.TabIndex = 23;
            this.openInNotepadBtn.Text = "Notepad";
            this.toolTip.SetToolTip(this.openInNotepadBtn, "Open editor\'s text in the Notepad");
            this.openInNotepadBtn.UseVisualStyleBackColor = true;
            this.openInNotepadBtn.Click += new System.EventHandler(this.openInNotepadBtn_Click);
            // 
            // runSelectionBtn
            // 
            this.runSelectionBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.runSelectionBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runSelectionBtn.Location = new System.Drawing.Point(876, 3);
            this.runSelectionBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.runSelectionBtn.Name = "runSelectionBtn";
            this.runSelectionBtn.Size = new System.Drawing.Size(106, 25);
            this.runSelectionBtn.TabIndex = 2;
            this.runSelectionBtn.Text = "Run selection";
            this.toolTip.SetToolTip(this.runSelectionBtn, "(Ctrl + Shift + R) Run selection (without selection it selects block)");
            this.runSelectionBtn.UseVisualStyleBackColor = true;
            this.runSelectionBtn.Click += new System.EventHandler(this.runSelectionBtn_Click);
            // 
            // validateBtn
            // 
            this.validateBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.validateBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.validateBtn.Location = new System.Drawing.Point(3, 34);
            this.validateBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.validateBtn.Name = "validateBtn";
            this.validateBtn.Size = new System.Drawing.Size(102, 25);
            this.validateBtn.TabIndex = 11;
            this.validateBtn.Text = "Validate";
            this.toolTip.SetToolTip(this.validateBtn, "Validates editor\'s text as one query");
            this.validateBtn.UseVisualStyleBackColor = true;
            this.validateBtn.Click += new System.EventHandler(this.validateBtn_Click);
            // 
            // pasteRngFilterBtn
            // 
            this.pasteRngFilterBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pasteRngFilterBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pasteRngFilterBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasteRngFilterBtn.Location = new System.Drawing.Point(111, 34);
            this.pasteRngFilterBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.pasteRngFilterBtn.Name = "pasteRngFilterBtn";
            this.pasteRngFilterBtn.Size = new System.Drawing.Size(102, 25);
            this.pasteRngFilterBtn.TabIndex = 13;
            this.pasteRngFilterBtn.Text = "Range as filter";
            this.toolTip.SetToolTip(this.pasteRngFilterBtn, resources.GetString("pasteRngFilterBtn.ToolTip"));
            this.pasteRngFilterBtn.UseVisualStyleBackColor = true;
            this.pasteRngFilterBtn.Click += new System.EventHandler(this.pasteRngFilterBtn_Click);
            // 
            // wrapIntoBlockBtn
            // 
            this.wrapIntoBlockBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.wrapIntoBlockBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wrapIntoBlockBtn.Location = new System.Drawing.Point(219, 34);
            this.wrapIntoBlockBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.wrapIntoBlockBtn.Name = "wrapIntoBlockBtn";
            this.wrapIntoBlockBtn.Size = new System.Drawing.Size(43, 25);
            this.wrapIntoBlockBtn.TabIndex = 22;
            this.wrapIntoBlockBtn.Text = "( ... )";
            this.toolTip.SetToolTip(this.wrapIntoBlockBtn, "(Ctrl + Shift + B) Wrap the selection into ( ... ) block");
            this.wrapIntoBlockBtn.UseVisualStyleBackColor = true;
            this.wrapIntoBlockBtn.Click += new System.EventHandler(this.wrapIntoBlockBtn_Click);
            // 
            // savedQueriesComboBox
            // 
            this.savedQueriesComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.buttonsTableLayoutPanel.SetColumnSpan(this.savedQueriesComboBox, 3);
            this.savedQueriesComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savedQueriesComboBox.DropDownHeight = 310;
            this.savedQueriesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savedQueriesComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savedQueriesComboBox.FormattingEnabled = true;
            this.savedQueriesComboBox.IntegralHeight = false;
            this.savedQueriesComboBox.ItemHeight = 15;
            this.savedQueriesComboBox.Location = new System.Drawing.Point(336, 34);
            this.savedQueriesComboBox.MaxDropDownItems = 25;
            this.savedQueriesComboBox.Name = "savedQueriesComboBox";
            this.savedQueriesComboBox.Size = new System.Drawing.Size(426, 23);
            this.savedQueriesComboBox.TabIndex = 15;
            this.toolTip.SetToolTip(this.savedQueriesComboBox, "Choose saved query (Right click to launch query picker)");
            this.savedQueriesComboBox.SelectedIndexChanged += new System.EventHandler(this.savedQueriesComboBox_SelectedIndexChanged);
            this.savedQueriesComboBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.savedQueriesComboBox_MouseDown);
            // 
            // saveQueryBtn
            // 
            this.saveQueryBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveQueryBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveQueryBtn.Location = new System.Drawing.Point(768, 34);
            this.saveQueryBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.saveQueryBtn.Name = "saveQueryBtn";
            this.saveQueryBtn.Size = new System.Drawing.Size(102, 25);
            this.saveQueryBtn.TabIndex = 14;
            this.saveQueryBtn.Text = "Save query";
            this.toolTip.SetToolTip(this.saveQueryBtn, "Save editor\'s text as a query");
            this.saveQueryBtn.UseVisualStyleBackColor = true;
            this.saveQueryBtn.Click += new System.EventHandler(this.saveQueryBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.runBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runBtn.Location = new System.Drawing.Point(876, 34);
            this.runBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(106, 25);
            this.runBtn.TabIndex = 10;
            this.runBtn.Text = "Run";
            this.toolTip.SetToolTip(this.runBtn, "(Ctrl + R) Run editor\'s text as one query");
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.upperTableLayoutPanel);
            this.mainSplitContainer.Panel1MinSize = 250;
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.objectsAndVariablesTabControl);
            this.mainSplitContainer.Panel2MinSize = 40;
            this.mainSplitContainer.Size = new System.Drawing.Size(979, 667);
            this.mainSplitContainer.SplitterDistance = 740;
            this.mainSplitContainer.SplitterWidth = 8;
            this.mainSplitContainer.TabIndex = 33;
            this.mainSplitContainer.TabStop = false;
            // 
            // upperTableLayoutPanel
            // 
            this.upperTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upperTableLayoutPanel.ColumnCount = 1;
            this.upperTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.upperTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.upperTableLayoutPanel.Controls.Add(this.parametersTableLayoutPanel, 0, 0);
            this.upperTableLayoutPanel.Controls.Add(this.sqlEditorScintilla, 0, 1);
            this.upperTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upperTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.upperTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.upperTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.upperTableLayoutPanel.Name = "upperTableLayoutPanel";
            this.upperTableLayoutPanel.RowCount = 2;
            this.upperTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.upperTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.upperTableLayoutPanel.Size = new System.Drawing.Size(740, 667);
            this.upperTableLayoutPanel.TabIndex = 32;
            // 
            // parametersTableLayoutPanel
            // 
            this.parametersTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.parametersTableLayoutPanel.ColumnCount = 5;
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.34695F));
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.55102F));
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.55102F));
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.55102F));
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.parametersTableLayoutPanel.Controls.Add(this.headersCheckBox, 0, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.worksheetTableLayoutPanel, 1, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.pasteResultsToSelectionCheckBox, 2, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.pasteToDataTableCheckBox, 3, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.clearEditorLabel, 4, 0);
            this.parametersTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.parametersTableLayoutPanel.MinimumSize = new System.Drawing.Size(0, 35);
            this.parametersTableLayoutPanel.Name = "parametersTableLayoutPanel";
            this.parametersTableLayoutPanel.RowCount = 2;
            this.parametersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.parametersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.parametersTableLayoutPanel.Size = new System.Drawing.Size(734, 39);
            this.parametersTableLayoutPanel.TabIndex = 31;
            // 
            // worksheetTableLayoutPanel
            // 
            this.worksheetTableLayoutPanel.AutoSize = true;
            this.worksheetTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.worksheetTableLayoutPanel.ColumnCount = 2;
            this.worksheetTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.worksheetTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.worksheetTableLayoutPanel.Controls.Add(this.sheetNameTextBox, 0, 0);
            this.worksheetTableLayoutPanel.Controls.Add(this.fillSheetNameBtn, 1, 0);
            this.worksheetTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.worksheetTableLayoutPanel.Location = new System.Drawing.Point(121, 0);
            this.worksheetTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.worksheetTableLayoutPanel.Name = "worksheetTableLayoutPanel";
            this.worksheetTableLayoutPanel.RowCount = 1;
            this.worksheetTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.worksheetTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.worksheetTableLayoutPanel.Size = new System.Drawing.Size(193, 39);
            this.worksheetTableLayoutPanel.TabIndex = 28;
            // 
            // pasteToDataTableCheckBox
            // 
            this.pasteToDataTableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pasteToDataTableCheckBox.Checked = true;
            this.pasteToDataTableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pasteToDataTableCheckBox.Location = new System.Drawing.Point(517, 3);
            this.pasteToDataTableCheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.pasteToDataTableCheckBox.Name = "pasteToDataTableCheckBox";
            this.pasteToDataTableCheckBox.Size = new System.Drawing.Size(180, 33);
            this.pasteToDataTableCheckBox.TabIndex = 33;
            this.pasteToDataTableCheckBox.Text = "Paste result to data table";
            this.toolTip.SetToolTip(this.pasteToDataTableCheckBox, "When checked it will paste the result into extra window with a visible table that" +
        " can be later pasted to Excel or discarded (recommended)");
            this.pasteToDataTableCheckBox.UseVisualStyleBackColor = true;
            this.pasteToDataTableCheckBox.CheckedChanged += new System.EventHandler(this.pasteToDataTableCheckBox_CheckedChanged);
            // 
            // objectsAndVariablesTabControl
            // 
            this.objectsAndVariablesTabControl.Controls.Add(this.tablesTabPage);
            this.objectsAndVariablesTabControl.Controls.Add(this.fieldsTabPage);
            this.objectsAndVariablesTabControl.Controls.Add(this.variablesTabPage);
            this.objectsAndVariablesTabControl.Controls.Add(this.runningQueriesTabPage);
            this.objectsAndVariablesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsAndVariablesTabControl.Location = new System.Drawing.Point(0, 0);
            this.objectsAndVariablesTabControl.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.objectsAndVariablesTabControl.Name = "objectsAndVariablesTabControl";
            this.objectsAndVariablesTabControl.SelectedIndex = 0;
            this.objectsAndVariablesTabControl.Size = new System.Drawing.Size(231, 667);
            this.objectsAndVariablesTabControl.TabIndex = 31;
            this.objectsAndVariablesTabControl.SelectedIndexChanged += new System.EventHandler(this.objectsAndVariablesTabControl_TabIndexChanged);
            this.objectsAndVariablesTabControl.TabIndexChanged += new System.EventHandler(this.objectsAndVariablesTabControl_TabIndexChanged);
            // 
            // tablesTabPage
            // 
            this.tablesTabPage.Controls.Add(this.tablesTableLayoutPanel);
            this.tablesTabPage.Location = new System.Drawing.Point(4, 22);
            this.tablesTabPage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tablesTabPage.Name = "tablesTabPage";
            this.tablesTabPage.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tablesTabPage.Size = new System.Drawing.Size(223, 641);
            this.tablesTabPage.TabIndex = 0;
            this.tablesTabPage.Text = "Tables";
            this.tablesTabPage.UseVisualStyleBackColor = true;
            // 
            // tablesTableLayoutPanel
            // 
            this.tablesTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tablesTableLayoutPanel.ColumnCount = 1;
            this.tablesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablesTableLayoutPanel.Controls.Add(this.searchTablesTextBox, 0, 0);
            this.tablesTableLayoutPanel.Controls.Add(this.tablesListBox, 0, 1);
            this.tablesTableLayoutPanel.Controls.Add(this.tablesButtonsTableLayoutPanel, 0, 2);
            this.tablesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesTableLayoutPanel.Location = new System.Drawing.Point(0, 3);
            this.tablesTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tablesTableLayoutPanel.Name = "tablesTableLayoutPanel";
            this.tablesTableLayoutPanel.RowCount = 3;
            this.tablesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablesTableLayoutPanel.Size = new System.Drawing.Size(223, 638);
            this.tablesTableLayoutPanel.TabIndex = 32;
            // 
            // tablesButtonsTableLayoutPanel
            // 
            this.tablesButtonsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tablesButtonsTableLayoutPanel.ColumnCount = 2;
            this.tablesButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tablesButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tablesButtonsTableLayoutPanel.Controls.Add(this.transferTablesToQueryBtn, 0, 0);
            this.tablesButtonsTableLayoutPanel.Controls.Add(this.fetchTablesBtn, 1, 0);
            this.tablesButtonsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesButtonsTableLayoutPanel.Location = new System.Drawing.Point(0, 606);
            this.tablesButtonsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tablesButtonsTableLayoutPanel.Name = "tablesButtonsTableLayoutPanel";
            this.tablesButtonsTableLayoutPanel.RowCount = 1;
            this.tablesButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablesButtonsTableLayoutPanel.Size = new System.Drawing.Size(223, 32);
            this.tablesButtonsTableLayoutPanel.TabIndex = 31;
            // 
            // fieldsTabPage
            // 
            this.fieldsTabPage.Controls.Add(this.fieldsTableLayoutPanel);
            this.fieldsTabPage.Location = new System.Drawing.Point(4, 22);
            this.fieldsTabPage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.fieldsTabPage.Name = "fieldsTabPage";
            this.fieldsTabPage.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.fieldsTabPage.Size = new System.Drawing.Size(223, 641);
            this.fieldsTabPage.TabIndex = 2;
            this.fieldsTabPage.Text = "Fields";
            this.fieldsTabPage.UseVisualStyleBackColor = true;
            // 
            // fieldsTableLayoutPanel
            // 
            this.fieldsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fieldsTableLayoutPanel.ColumnCount = 1;
            this.fieldsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fieldsTableLayoutPanel.Controls.Add(this.searchFieldsTextBox, 0, 0);
            this.fieldsTableLayoutPanel.Controls.Add(this.fieldsListBox, 0, 1);
            this.fieldsTableLayoutPanel.Controls.Add(this.fieldsButtonsTableLayoutPanel, 0, 2);
            this.fieldsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldsTableLayoutPanel.Location = new System.Drawing.Point(0, 3);
            this.fieldsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.fieldsTableLayoutPanel.Name = "fieldsTableLayoutPanel";
            this.fieldsTableLayoutPanel.RowCount = 3;
            this.fieldsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fieldsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fieldsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fieldsTableLayoutPanel.Size = new System.Drawing.Size(223, 638);
            this.fieldsTableLayoutPanel.TabIndex = 33;
            // 
            // searchFieldsTextBox
            // 
            this.searchFieldsTextBox.AcceptsReturn = true;
            this.searchFieldsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchFieldsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchFieldsTextBox.Location = new System.Drawing.Point(3, 3);
            this.searchFieldsTextBox.Name = "searchFieldsTextBox";
            this.searchFieldsTextBox.Size = new System.Drawing.Size(217, 21);
            this.searchFieldsTextBox.TabIndex = 24;
            this.searchFieldsTextBox.Text = "Search";
            this.toolTip.SetToolTip(this.searchFieldsTextBox, "Search fields (you can use Regex) (selections on the list are preserved)");
            this.searchFieldsTextBox.WordWrap = false;
            this.searchFieldsTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyDown);
            // 
            // fieldsListBox
            // 
            this.fieldsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fieldsListBox.FormattingEnabled = true;
            this.fieldsListBox.HorizontalScrollbar = true;
            this.fieldsListBox.ItemHeight = 12;
            this.fieldsListBox.Location = new System.Drawing.Point(3, 30);
            this.fieldsListBox.Name = "fieldsListBox";
            this.fieldsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fieldsListBox.Size = new System.Drawing.Size(217, 573);
            this.fieldsListBox.TabIndex = 20;
            this.fieldsListBox.SelectedIndexChanged += new System.EventHandler(this.objectsListBox_SelectedIndexChanged);
            this.fieldsListBox.DoubleClick += new System.EventHandler(this.objectsListBox_DoubleClick);
            // 
            // fieldsButtonsTableLayoutPanel
            // 
            this.fieldsButtonsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fieldsButtonsTableLayoutPanel.ColumnCount = 2;
            this.fieldsButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.fieldsButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.fieldsButtonsTableLayoutPanel.Controls.Add(this.transferFieldsToQueryBtn, 0, 0);
            this.fieldsButtonsTableLayoutPanel.Controls.Add(this.fetchFieldsBtn, 1, 0);
            this.fieldsButtonsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldsButtonsTableLayoutPanel.Location = new System.Drawing.Point(0, 606);
            this.fieldsButtonsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.fieldsButtonsTableLayoutPanel.Name = "fieldsButtonsTableLayoutPanel";
            this.fieldsButtonsTableLayoutPanel.RowCount = 1;
            this.fieldsButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fieldsButtonsTableLayoutPanel.Size = new System.Drawing.Size(223, 32);
            this.fieldsButtonsTableLayoutPanel.TabIndex = 31;
            // 
            // transferFieldsToQueryBtn
            // 
            this.transferFieldsToQueryBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transferFieldsToQueryBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transferFieldsToQueryBtn.Location = new System.Drawing.Point(3, 3);
            this.transferFieldsToQueryBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.transferFieldsToQueryBtn.Name = "transferFieldsToQueryBtn";
            this.transferFieldsToQueryBtn.Size = new System.Drawing.Size(49, 26);
            this.transferFieldsToQueryBtn.TabIndex = 19;
            this.transferFieldsToQueryBtn.Text = "←";
            this.toolTip.SetToolTip(this.transferFieldsToQueryBtn, "Paste to the selection selected fields from the list separated by comma");
            this.transferFieldsToQueryBtn.UseVisualStyleBackColor = true;
            this.transferFieldsToQueryBtn.Click += new System.EventHandler(this.transferToQueryBtn_Click);
            // 
            // fetchFieldsBtn
            // 
            this.fetchFieldsBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fetchFieldsBtn.Location = new System.Drawing.Point(58, 3);
            this.fetchFieldsBtn.MinimumSize = new System.Drawing.Size(0, 25);
            this.fetchFieldsBtn.Name = "fetchFieldsBtn";
            this.fetchFieldsBtn.Size = new System.Drawing.Size(162, 26);
            this.fetchFieldsBtn.TabIndex = 18;
            this.fetchFieldsBtn.Text = "Fetch";
            this.toolTip.SetToolTip(this.fetchFieldsBtn, "Fetch all fields from the given table (selection must include table name)");
            this.fetchFieldsBtn.UseVisualStyleBackColor = true;
            this.fetchFieldsBtn.Click += new System.EventHandler(this.fetchBtn_Click);
            // 
            // variablesTabPage
            // 
            this.variablesTabPage.Controls.Add(this.variablesDataGridView);
            this.variablesTabPage.Location = new System.Drawing.Point(4, 22);
            this.variablesTabPage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.variablesTabPage.Name = "variablesTabPage";
            this.variablesTabPage.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.variablesTabPage.Size = new System.Drawing.Size(223, 641);
            this.variablesTabPage.TabIndex = 1;
            this.variablesTabPage.Text = "Variables";
            this.variablesTabPage.UseVisualStyleBackColor = true;
            // 
            // variablesDataGridView
            // 
            this.variablesDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.variablesDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.variablesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.variablesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.variablesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.variablesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.variableValuesCountColumn,
            this.VariableNameColumn,
            this.VariableValuesColumn,
            this.VariableInstancesColumn});
            this.variablesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variablesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.variablesDataGridView.Location = new System.Drawing.Point(0, 3);
            this.variablesDataGridView.MultiSelect = false;
            this.variablesDataGridView.Name = "variablesDataGridView";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.variablesDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.variablesDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.variablesDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.variablesDataGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.variablesDataGridView.Size = new System.Drawing.Size(223, 638);
            this.variablesDataGridView.TabIndex = 0;
            this.variablesDataGridView.TabStop = false;
            this.variablesDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.variablesDataGridView_CellContentClick);
            // 
            // variableValuesCountColumn
            // 
            this.variableValuesCountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = "0";
            this.variableValuesCountColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.variableValuesCountColumn.HeaderText = "Count";
            this.variableValuesCountColumn.Name = "variableValuesCountColumn";
            this.variableValuesCountColumn.ReadOnly = true;
            this.variableValuesCountColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.variableValuesCountColumn.ToolTipText = "Count of values";
            this.variableValuesCountColumn.Width = 60;
            // 
            // VariableNameColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.VariableNameColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.VariableNameColumn.HeaderText = "Variable";
            this.VariableNameColumn.MaxInputLength = 40;
            this.VariableNameColumn.Name = "VariableNameColumn";
            this.VariableNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.VariableNameColumn.ToolTipText = "Choose unique variable name";
            // 
            // VariableValuesColumn
            // 
            this.VariableValuesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "Edit";
            this.VariableValuesColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.VariableValuesColumn.HeaderText = "Values";
            this.VariableValuesColumn.Name = "VariableValuesColumn";
            this.VariableValuesColumn.Text = "Values";
            this.VariableValuesColumn.ToolTipText = "Click to set values";
            this.VariableValuesColumn.Width = 45;
            // 
            // VariableInstancesColumn
            // 
            this.VariableInstancesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = "0";
            this.VariableInstancesColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.VariableInstancesColumn.HeaderText = "i";
            this.VariableInstancesColumn.MaxInputLength = 3;
            this.VariableInstancesColumn.MinimumWidth = 15;
            this.VariableInstancesColumn.Name = "VariableInstancesColumn";
            this.VariableInstancesColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.VariableInstancesColumn.ToolTipText = "Instances that will divide query";
            this.VariableInstancesColumn.Width = 34;
            // 
            // runningQueriesTabPage
            // 
            this.runningQueriesTabPage.Controls.Add(this.runningQueriesDataGridView);
            this.runningQueriesTabPage.Location = new System.Drawing.Point(4, 22);
            this.runningQueriesTabPage.Name = "runningQueriesTabPage";
            this.runningQueriesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.runningQueriesTabPage.Size = new System.Drawing.Size(223, 641);
            this.runningQueriesTabPage.TabIndex = 3;
            this.runningQueriesTabPage.Text = "Running";
            this.runningQueriesTabPage.UseVisualStyleBackColor = true;
            // 
            // runningQueriesDataGridView
            // 
            this.runningQueriesDataGridView.AllowUserToAddRows = false;
            this.runningQueriesDataGridView.AllowUserToDeleteRows = false;
            this.runningQueriesDataGridView.AllowUserToResizeRows = false;
            this.runningQueriesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.runningQueriesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.runningQueriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.runningQueriesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CancelQueryColumn,
            this.QueryNameColumn,
            this.TimeColumn,
            this.QueryColumn});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.runningQueriesDataGridView.DefaultCellStyle = dataGridViewCellStyle10;
            this.runningQueriesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runningQueriesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.runningQueriesDataGridView.Location = new System.Drawing.Point(3, 3);
            this.runningQueriesDataGridView.Name = "runningQueriesDataGridView";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.runningQueriesDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.runningQueriesDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.runningQueriesDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.runningQueriesDataGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.runningQueriesDataGridView.Size = new System.Drawing.Size(217, 635);
            this.runningQueriesDataGridView.TabIndex = 0;
            this.runningQueriesDataGridView.TabStop = false;
            this.runningQueriesDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.runningQueriesDataGridView_CellClick);
            // 
            // CancelQueryColumn
            // 
            this.CancelQueryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.CancelQueryColumn.HeaderText = "Cancel";
            this.CancelQueryColumn.MinimumWidth = 25;
            this.CancelQueryColumn.Name = "CancelQueryColumn";
            this.CancelQueryColumn.Width = 46;
            // 
            // QueryNameColumn
            // 
            this.QueryNameColumn.HeaderText = "Query name";
            this.QueryNameColumn.MinimumWidth = 60;
            this.QueryNameColumn.Name = "QueryNameColumn";
            this.QueryNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TimeColumn
            // 
            this.TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.TimeColumn.HeaderText = "Time";
            this.TimeColumn.MinimumWidth = 40;
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TimeColumn.Width = 40;
            // 
            // QueryColumn
            // 
            this.QueryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.QueryColumn.HeaderText = "Query";
            this.QueryColumn.MinimumWidth = 30;
            this.QueryColumn.Name = "QueryColumn";
            this.QueryColumn.Width = 41;
            // 
            // toolTip
            // 
            this.toolTip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            // 
            // SqlEditorForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(985, 738);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SqlEditorForm";
            this.Opacity = 0.95D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL Extractor for Excel";
            this.Activated += new System.EventHandler(this.SqlEditorForm_Activated);
            this.Deactivate += new System.EventHandler(this.SqlEditorForm_Deactivate);
            this.Load += new System.EventHandler(this.SqlEditorForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.buttonsTableLayoutPanel.ResumeLayout(false);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.upperTableLayoutPanel.ResumeLayout(false);
            this.parametersTableLayoutPanel.ResumeLayout(false);
            this.parametersTableLayoutPanel.PerformLayout();
            this.worksheetTableLayoutPanel.ResumeLayout(false);
            this.worksheetTableLayoutPanel.PerformLayout();
            this.objectsAndVariablesTabControl.ResumeLayout(false);
            this.tablesTabPage.ResumeLayout(false);
            this.tablesTableLayoutPanel.ResumeLayout(false);
            this.tablesTableLayoutPanel.PerformLayout();
            this.tablesButtonsTableLayoutPanel.ResumeLayout(false);
            this.fieldsTabPage.ResumeLayout(false);
            this.fieldsTableLayoutPanel.ResumeLayout(false);
            this.fieldsTableLayoutPanel.PerformLayout();
            this.fieldsButtonsTableLayoutPanel.ResumeLayout(false);
            this.variablesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.variablesDataGridView)).EndInit();
            this.runningQueriesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.runningQueriesDataGridView)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        private ScintillaNET.Scintilla sqlEditorScintilla;
        private System.Windows.Forms.Button transferTablesToQueryBtn;
        private System.Windows.Forms.Button fetchTablesBtn;
        private System.Windows.Forms.ListBox tablesListBox;
        private TextBox searchTablesTextBox;
        private CheckBox pasteResultsToSelectionCheckBox;
        private TextBox sheetNameTextBox;
        private Button fillSheetNameBtn;
        private CheckBox headersCheckBox;
        private Label clearEditorLabel;
        private TableLayoutPanel mainTableLayoutPanel;
        private TableLayoutPanel parametersTableLayoutPanel;
        private CheckBox pasteToDataTableCheckBox;
        private TableLayoutPanel upperTableLayoutPanel;
        private TableLayoutPanel tablesButtonsTableLayoutPanel;
        private TableLayoutPanel tablesTableLayoutPanel;
        private TableLayoutPanel worksheetTableLayoutPanel;
        private TableLayoutPanel buttonsTableLayoutPanel;
        private Button validateSelectionBtn;
        private Button pasteRngBtn;
        private Button commentBtn;
        private ComboBox serverTypeComboBox;
        private ComboBox serverComboBox;
        private Button testConnBtn;
        private Button openInNotepadBtn;
        private Button runSelectionBtn;
        private Button validateBtn;
        private Button pasteRngFilterBtn;
        private Button wrapIntoBlockBtn;
        private ComboBox savedQueriesComboBox;
        private Button saveQueryBtn;
        private Button runBtn;
        private TabControl objectsAndVariablesTabControl;
        private TabPage tablesTabPage;
        private TabPage variablesTabPage;
        private TabPage fieldsTabPage;
        private TableLayoutPanel fieldsTableLayoutPanel;
        private TextBox searchFieldsTextBox;
        private ListBox fieldsListBox;
        private TableLayoutPanel fieldsButtonsTableLayoutPanel;
        private Button transferFieldsToQueryBtn;
        private Button fetchFieldsBtn;
        private TabPage runningQueriesTabPage;
        private DataGridView runningQueriesDataGridView;
        private DataGridView variablesDataGridView;
        private Button formatToSqlBtn;
        private Button separateBtn;
        private DataGridViewButtonColumn CancelQueryColumn;
        private DataGridViewTextBoxColumn QueryNameColumn;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewButtonColumn QueryColumn;
        private DataGridViewTextBoxColumn variableValuesCountColumn;
        private DataGridViewTextBoxColumn VariableNameColumn;
        private DataGridViewButtonColumn VariableValuesColumn;
        private DataGridViewTextBoxColumn VariableInstancesColumn;
        private SplitContainer mainSplitContainer;
        private ToolTip toolTip;
    }
}