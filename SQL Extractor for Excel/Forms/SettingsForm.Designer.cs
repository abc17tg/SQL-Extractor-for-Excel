namespace SQL_Extractor_for_Excel.Forms
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer m_components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (m_components != null))
            {
                m_components.Dispose();
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
            this.m_splitMain = new System.Windows.Forms.SplitContainer();
            this.m_treeCategories = new System.Windows.Forms.TreeView();
            this.m_panelContent = new System.Windows.Forms.Panel();
            this.m_panelButtons = new System.Windows.Forms.Panel();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_btnApply = new System.Windows.Forms.Button();
            this.m_btnReset = new System.Windows.Forms.Button();
            this.m_lblAnnotation = new System.Windows.Forms.Label();

            // General category controls
            this.m_grpGeneral = new System.Windows.Forms.GroupBox();
            this.m_tblGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblKeepOnTop = new System.Windows.Forms.Label();
            this.m_chkKeepOnTopGlobal = new System.Windows.Forms.CheckBox();
            this.m_chkKeepOnTop = new System.Windows.Forms.CheckBox();

            // Query Editor category controls
            this.m_grpQueryEditor = new System.Windows.Forms.GroupBox();
            this.m_tblQueryEditor = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblTextWrap = new System.Windows.Forms.Label();
            this.m_chkTextWrapGlobal = new System.Windows.Forms.CheckBox();
            this.m_chkTextWrap = new System.Windows.Forms.CheckBox();

            // Syntax Highlighting category controls
            this.m_grpSyntax = new System.Windows.Forms.GroupBox();
            this.m_tblSyntax = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblKeywordColor = new System.Windows.Forms.Label();
            this.m_chkKeywordColorGlobal = new System.Windows.Forms.CheckBox();
            this.m_btnKeywordColor = new System.Windows.Forms.Button();
            this.m_lblStringColor = new System.Windows.Forms.Label();
            this.m_chkStringColorGlobal = new System.Windows.Forms.CheckBox();
            this.m_btnStringColor = new System.Windows.Forms.Button();
            this.m_lblCommentColor = new System.Windows.Forms.Label();
            this.m_chkCommentColorGlobal = new System.Windows.Forms.CheckBox();
            this.m_btnCommentColor = new System.Windows.Forms.Button();
            this.m_lblNumberColor = new System.Windows.Forms.Label();
            this.m_chkNumberColorGlobal = new System.Windows.Forms.CheckBox();
            this.m_btnNumberColor = new System.Windows.Forms.Button();
            this.m_lblDefaultForeground = new System.Windows.Forms.Label();
            this.m_chkDefaultForegroundGlobal = new System.Windows.Forms.CheckBox();
            this.m_btnDefaultForeground = new System.Windows.Forms.Button();
            this.m_lblEditorBackground = new System.Windows.Forms.Label();
            this.m_chkEditorBackgroundGlobal = new System.Windows.Forms.CheckBox();
            this.m_btnEditorBackground = new System.Windows.Forms.Button();

            // Export category controls
            this.m_grpExport = new System.Windows.Forms.GroupBox();
            this.m_tblExport = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblDefaultExport = new System.Windows.Forms.Label();
            this.m_chkDefaultExportGlobal = new System.Windows.Forms.CheckBox();
            this.m_cmbDefaultExport = new System.Windows.Forms.ComboBox();

            // Keywords category controls
            this.m_grpKeywords = new System.Windows.Forms.GroupBox();
            this.m_tblKeywords = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblMsSqlKeywords = new System.Windows.Forms.Label();
            this.m_chkMsSqlKeywordsGlobal = new System.Windows.Forms.CheckBox();
            this.m_txtMsSqlKeywords = new System.Windows.Forms.TextBox();
            this.m_lblOracleKeywords = new System.Windows.Forms.Label();
            this.m_chkOracleKeywordsGlobal = new System.Windows.Forms.CheckBox();
            this.m_txtOracleKeywords = new System.Windows.Forms.TextBox();
            this.m_btnResetKeywords = new System.Windows.Forms.Button();

            // Startup category controls
            this.m_grpStartup = new System.Windows.Forms.GroupBox();
            this.m_tblStartup = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblStartQuery = new System.Windows.Forms.Label();
            this.m_chkStartQueryGlobal = new System.Windows.Forms.CheckBox();
            this.m_txtStartQuery = new System.Windows.Forms.TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.m_splitMain)).BeginInit();
            this.m_splitMain.Panel1.SuspendLayout();
            this.m_splitMain.Panel2.SuspendLayout();
            this.m_splitMain.SuspendLayout();
            this.m_panelButtons.SuspendLayout();

            // General GroupBox
            this.m_grpGeneral.SuspendLayout();
            this.m_tblGeneral.SuspendLayout();

            // Query Editor GroupBox
            this.m_grpQueryEditor.SuspendLayout();
            this.m_tblQueryEditor.SuspendLayout();

            // Syntax GroupBox
            this.m_grpSyntax.SuspendLayout();
            this.m_tblSyntax.SuspendLayout();

            // Export GroupBox
            this.m_grpExport.SuspendLayout();
            this.m_tblExport.SuspendLayout();

            // Keywords GroupBox
            this.m_grpKeywords.SuspendLayout();
            this.m_tblKeywords.SuspendLayout();

            // Startup GroupBox
            this.m_grpStartup.SuspendLayout();
            this.m_tblStartup.SuspendLayout();

            this.SuspendLayout();
            //
            // m_splitMain
            //
            this.m_splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitMain.Location = new System.Drawing.Point(0, 0);
            this.m_splitMain.Name = "m_splitMain";
            //
            // m_splitMain.Panel1
            //
            this.m_splitMain.Panel1.Controls.Add(this.m_treeCategories);
            this.m_splitMain.Panel1MinSize = 180;
            //
            // m_splitMain.Panel2
            //
            this.m_splitMain.Panel2.Controls.Add(this.m_panelContent);
            this.m_splitMain.Panel2MinSize = 400;
            this.m_splitMain.Size = new System.Drawing.Size(900, 600);
            this.m_splitMain.SplitterDistance = 200;
            this.m_splitMain.TabIndex = 0;
            //
            // m_treeCategories
            //
            this.m_treeCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_treeCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_treeCategories.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_treeCategories.FullRowSelect = true;
            this.m_treeCategories.HideSelection = false;
            this.m_treeCategories.ItemHeight = 28;
            this.m_treeCategories.Location = new System.Drawing.Point(0, 0);
            this.m_treeCategories.Name = "m_treeCategories";
            this.m_treeCategories.ShowLines = false;
            this.m_treeCategories.Size = new System.Drawing.Size(200, 600);
            this.m_treeCategories.TabIndex = 0;
            //
            // m_panelContent
            //
            this.m_panelContent.AutoScroll = true;
            this.m_panelContent.BackColor = System.Drawing.Color.White;
            this.m_panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panelContent.Location = new System.Drawing.Point(0, 0);
            this.m_panelContent.Name = "m_panelContent";
            this.m_panelContent.Padding = new System.Windows.Forms.Padding(15);
            this.m_panelContent.Size = new System.Drawing.Size(696, 600);
            this.m_panelContent.TabIndex = 0;
            //
            // m_panelButtons
            //
            this.m_panelButtons.BackColor = System.Drawing.SystemColors.Control;
            this.m_panelButtons.Controls.Add(this.m_lblAnnotation);
            this.m_panelButtons.Controls.Add(this.m_btnReset);
            this.m_panelButtons.Controls.Add(this.m_btnOK);
            this.m_panelButtons.Controls.Add(this.m_btnCancel);
            this.m_panelButtons.Controls.Add(this.m_btnApply);
            this.m_panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_panelButtons.Location = new System.Drawing.Point(0, 600);
            this.m_panelButtons.Name = "m_panelButtons";
            this.m_panelButtons.Padding = new System.Windows.Forms.Padding(10);
            this.m_panelButtons.Size = new System.Drawing.Size(900, 60);
            this.m_panelButtons.TabIndex = 1;
            //
            // m_lblAnnotation
            //
            this.m_lblAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.m_lblAnnotation.AutoSize = true;
            this.m_lblAnnotation.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblAnnotation.ForeColor = System.Drawing.Color.Gray;
            this.m_lblAnnotation.Location = new System.Drawing.Point(140, 22);
            this.m_lblAnnotation.Name = "m_lblAnnotation";
            this.m_lblAnnotation.Size = new System.Drawing.Size(250, 13);
            this.m_lblAnnotation.TabIndex = 4;
            this.m_lblAnnotation.Text = "* Setting differs from global default";
            //
            // m_btnOK
            //
            this.m_btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnOK.Location = new System.Drawing.Point(638, 15);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(80, 30);
            this.m_btnOK.TabIndex = 0;
            this.m_btnOK.Text = "OK";
            this.m_btnOK.UseVisualStyleBackColor = true;
            //
            // m_btnCancel
            //
            this.m_btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnCancel.Location = new System.Drawing.Point(724, 15);
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(80, 30);
            this.m_btnCancel.TabIndex = 1;
            this.m_btnCancel.Text = "Cancel";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            //
            // m_btnApply
            //
            this.m_btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnApply.Location = new System.Drawing.Point(810, 15);
            this.m_btnApply.Name = "m_btnApply";
            this.m_btnApply.Size = new System.Drawing.Size(80, 30);
            this.m_btnApply.TabIndex = 2;
            this.m_btnApply.Text = "Apply";
            this.m_btnApply.UseVisualStyleBackColor = true;
            //
            // m_btnReset
            //
            this.m_btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnReset.Location = new System.Drawing.Point(12, 15);
            this.m_btnReset.Name = "m_btnReset";
            this.m_btnReset.Size = new System.Drawing.Size(120, 30);
            this.m_btnReset.TabIndex = 3;
            this.m_btnReset.Text = "Reset to Defaults";
            this.m_btnReset.UseVisualStyleBackColor = true;
            //
            // m_grpGeneral
            //
            this.m_grpGeneral.BackColor = System.Drawing.Color.White;
            this.m_grpGeneral.Controls.Add(this.m_tblGeneral);
            this.m_grpGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpGeneral.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_grpGeneral.Location = new System.Drawing.Point(0, 0);
            this.m_grpGeneral.Name = "m_grpGeneral";
            this.m_grpGeneral.Padding = new System.Windows.Forms.Padding(10);
            this.m_grpGeneral.Size = new System.Drawing.Size(660, 100);
            this.m_grpGeneral.TabIndex = 0;
            this.m_grpGeneral.TabStop = false;
            this.m_grpGeneral.Text = "General";
            //
            // m_tblGeneral
            //
            this.m_tblGeneral.ColumnCount = 3;
            this.m_tblGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.m_tblGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblGeneral.Controls.Add(this.m_lblKeepOnTop, 0, 0);
            this.m_tblGeneral.Controls.Add(this.m_chkKeepOnTopGlobal, 1, 0);
            this.m_tblGeneral.Controls.Add(this.m_chkKeepOnTop, 2, 0);
            this.m_tblGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tblGeneral.Location = new System.Drawing.Point(10, 24);
            this.m_tblGeneral.Name = "m_tblGeneral";
            this.m_tblGeneral.RowCount = 2;
            this.m_tblGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblGeneral.Size = new System.Drawing.Size(640, 66);
            this.m_tblGeneral.TabIndex = 0;
            //
            // m_lblKeepOnTop
            //
            this.m_lblKeepOnTop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblKeepOnTop.AutoSize = true;
            this.m_lblKeepOnTop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblKeepOnTop.Location = new System.Drawing.Point(3, 10);
            this.m_lblKeepOnTop.Name = "m_lblKeepOnTop";
            this.m_lblKeepOnTop.Size = new System.Drawing.Size(170, 15);
            this.m_lblKeepOnTop.TabIndex = 0;
            this.m_lblKeepOnTop.Text = "Pin SQL Editor as top-level window";
            //
            // m_chkKeepOnTopGlobal
            //
            this.m_chkKeepOnTopGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkKeepOnTopGlobal.AutoSize = true;
            this.m_chkKeepOnTopGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkKeepOnTopGlobal.Location = new System.Drawing.Point(206, 9);
            this.m_chkKeepOnTopGlobal.Name = "m_chkKeepOnTopGlobal";
            this.m_chkKeepOnTopGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkKeepOnTopGlobal.TabIndex = 1;
            this.m_chkKeepOnTopGlobal.Text = "Apply globally";
            this.m_chkKeepOnTopGlobal.UseVisualStyleBackColor = true;
            //
            // m_chkKeepOnTop
            //
            this.m_chkKeepOnTop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkKeepOnTop.AutoSize = true;
            this.m_chkKeepOnTop.Checked = true;
            this.m_chkKeepOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkKeepOnTop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkKeepOnTop.Location = new System.Drawing.Point(332, 8);
            this.m_chkKeepOnTop.Name = "m_chkKeepOnTop";
            this.m_chkKeepOnTop.Size = new System.Drawing.Size(45, 19);
            this.m_chkKeepOnTop.TabIndex = 2;
            this.m_chkKeepOnTop.Text = "On";
            this.m_chkKeepOnTop.UseVisualStyleBackColor = true;
            //
            // m_grpQueryEditor
            //
            this.m_grpQueryEditor.BackColor = System.Drawing.Color.White;
            this.m_grpQueryEditor.Controls.Add(this.m_tblQueryEditor);
            this.m_grpQueryEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpQueryEditor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_grpQueryEditor.Location = new System.Drawing.Point(0, 0);
            this.m_grpQueryEditor.Name = "m_grpQueryEditor";
            this.m_grpQueryEditor.Padding = new System.Windows.Forms.Padding(10);
            this.m_grpQueryEditor.Size = new System.Drawing.Size(660, 100);
            this.m_grpQueryEditor.TabIndex = 0;
            this.m_grpQueryEditor.TabStop = false;
            this.m_grpQueryEditor.Text = "Query Editor";
            //
            // m_tblQueryEditor
            //
            this.m_tblQueryEditor.ColumnCount = 3;
            this.m_tblQueryEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.m_tblQueryEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblQueryEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblQueryEditor.Controls.Add(this.m_lblTextWrap, 0, 0);
            this.m_tblQueryEditor.Controls.Add(this.m_chkTextWrapGlobal, 1, 0);
            this.m_tblQueryEditor.Controls.Add(this.m_chkTextWrap, 2, 0);
            this.m_tblQueryEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tblQueryEditor.Location = new System.Drawing.Point(10, 24);
            this.m_tblQueryEditor.Name = "m_tblQueryEditor";
            this.m_tblQueryEditor.RowCount = 2;
            this.m_tblQueryEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblQueryEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblQueryEditor.Size = new System.Drawing.Size(640, 66);
            this.m_tblQueryEditor.TabIndex = 0;
            //
            // m_lblTextWrap
            //
            this.m_lblTextWrap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblTextWrap.AutoSize = true;
            this.m_lblTextWrap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblTextWrap.Location = new System.Drawing.Point(3, 10);
            this.m_lblTextWrap.Name = "m_lblTextWrap";
            this.m_lblTextWrap.Size = new System.Drawing.Size(112, 15);
            this.m_lblTextWrap.TabIndex = 0;
            this.m_lblTextWrap.Text = "Enable text wrapping";
            //
            // m_chkTextWrapGlobal
            //
            this.m_chkTextWrapGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkTextWrapGlobal.AutoSize = true;
            this.m_chkTextWrapGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkTextWrapGlobal.Location = new System.Drawing.Point(206, 9);
            this.m_chkTextWrapGlobal.Name = "m_chkTextWrapGlobal";
            this.m_chkTextWrapGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkTextWrapGlobal.TabIndex = 1;
            this.m_chkTextWrapGlobal.Text = "Apply globally";
            this.m_chkTextWrapGlobal.UseVisualStyleBackColor = true;
            //
            // m_chkTextWrap
            //
            this.m_chkTextWrap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkTextWrap.AutoSize = true;
            this.m_chkTextWrap.Checked = true;
            this.m_chkTextWrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkTextWrap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkTextWrap.Location = new System.Drawing.Point(332, 8);
            this.m_chkTextWrap.Name = "m_chkTextWrap";
            this.m_chkTextWrap.Size = new System.Drawing.Size(45, 19);
            this.m_chkTextWrap.TabIndex = 2;
            this.m_chkTextWrap.Text = "On";
            this.m_chkTextWrap.UseVisualStyleBackColor = true;
            //
            // m_grpSyntax
            //
            this.m_grpSyntax.BackColor = System.Drawing.Color.White;
            this.m_grpSyntax.Controls.Add(this.m_tblSyntax);
            this.m_grpSyntax.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpSyntax.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_grpSyntax.Location = new System.Drawing.Point(0, 0);
            this.m_grpSyntax.Name = "m_grpSyntax";
            this.m_grpSyntax.Padding = new System.Windows.Forms.Padding(10);
            this.m_grpSyntax.Size = new System.Drawing.Size(660, 260);
            this.m_grpSyntax.TabIndex = 0;
            this.m_grpSyntax.TabStop = false;
            this.m_grpSyntax.Text = "Syntax Highlighting Colors";
            //
            // m_tblSyntax
            //
            this.m_tblSyntax.ColumnCount = 3;
            this.m_tblSyntax.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.m_tblSyntax.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblSyntax.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblSyntax.Controls.Add(this.m_lblKeywordColor, 0, 0);
            this.m_tblSyntax.Controls.Add(this.m_chkKeywordColorGlobal, 1, 0);
            this.m_tblSyntax.Controls.Add(this.m_btnKeywordColor, 2, 0);
            this.m_tblSyntax.Controls.Add(this.m_lblStringColor, 0, 1);
            this.m_tblSyntax.Controls.Add(this.m_chkStringColorGlobal, 1, 1);
            this.m_tblSyntax.Controls.Add(this.m_btnStringColor, 2, 1);
            this.m_tblSyntax.Controls.Add(this.m_lblCommentColor, 0, 2);
            this.m_tblSyntax.Controls.Add(this.m_chkCommentColorGlobal, 1, 2);
            this.m_tblSyntax.Controls.Add(this.m_btnCommentColor, 2, 2);
            this.m_tblSyntax.Controls.Add(this.m_lblNumberColor, 0, 3);
            this.m_tblSyntax.Controls.Add(this.m_chkNumberColorGlobal, 1, 3);
            this.m_tblSyntax.Controls.Add(this.m_btnNumberColor, 2, 3);
            this.m_tblSyntax.Controls.Add(this.m_lblDefaultForeground, 0, 4);
            this.m_tblSyntax.Controls.Add(this.m_chkDefaultForegroundGlobal, 1, 4);
            this.m_tblSyntax.Controls.Add(this.m_btnDefaultForeground, 2, 4);
            this.m_tblSyntax.Controls.Add(this.m_lblEditorBackground, 0, 5);
            this.m_tblSyntax.Controls.Add(this.m_chkEditorBackgroundGlobal, 1, 5);
            this.m_tblSyntax.Controls.Add(this.m_btnEditorBackground, 2, 5);
            this.m_tblSyntax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tblSyntax.Location = new System.Drawing.Point(10, 24);
            this.m_tblSyntax.Name = "m_tblSyntax";
            this.m_tblSyntax.RowCount = 6;
            this.m_tblSyntax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblSyntax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblSyntax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblSyntax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblSyntax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblSyntax.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblSyntax.Size = new System.Drawing.Size(640, 226);
            this.m_tblSyntax.TabIndex = 0;
            //
            // m_lblKeywordColor
            //
            this.m_lblKeywordColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblKeywordColor.AutoSize = true;
            this.m_lblKeywordColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblKeywordColor.Location = new System.Drawing.Point(3, 10);
            this.m_lblKeywordColor.Name = "m_lblKeywordColor";
            this.m_lblKeywordColor.Size = new System.Drawing.Size(80, 15);
            this.m_lblKeywordColor.TabIndex = 0;
            this.m_lblKeywordColor.Text = "Keyword color";
            //
            // m_chkKeywordColorGlobal
            //
            this.m_chkKeywordColorGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkKeywordColorGlobal.AutoSize = true;
            this.m_chkKeywordColorGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkKeywordColorGlobal.Location = new System.Drawing.Point(206, 9);
            this.m_chkKeywordColorGlobal.Name = "m_chkKeywordColorGlobal";
            this.m_chkKeywordColorGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkKeywordColorGlobal.TabIndex = 1;
            this.m_chkKeywordColorGlobal.Text = "Apply globally";
            this.m_chkKeywordColorGlobal.UseVisualStyleBackColor = true;
            //
            // m_btnKeywordColor
            //
            this.m_btnKeywordColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_btnKeywordColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(156)))), ((int)(((byte)(214)))));
            this.m_btnKeywordColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnKeywordColor.ForeColor = System.Drawing.Color.White;
            this.m_btnKeywordColor.Location = new System.Drawing.Point(332, 5);
            this.m_btnKeywordColor.Name = "m_btnKeywordColor";
            this.m_btnKeywordColor.Size = new System.Drawing.Size(80, 25);
            this.m_btnKeywordColor.TabIndex = 2;
            this.m_btnKeywordColor.Text = "Blue";
            this.m_btnKeywordColor.UseVisualStyleBackColor = false;
            //
            // m_lblStringColor
            //
            this.m_lblStringColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblStringColor.AutoSize = true;
            this.m_lblStringColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblStringColor.Location = new System.Drawing.Point(3, 45);
            this.m_lblStringColor.Name = "m_lblStringColor";
            this.m_lblStringColor.Size = new System.Drawing.Size(70, 15);
            this.m_lblStringColor.TabIndex = 3;
            this.m_lblStringColor.Text = "String color";
            //
            // m_chkStringColorGlobal
            //
            this.m_chkStringColorGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkStringColorGlobal.AutoSize = true;
            this.m_chkStringColorGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkStringColorGlobal.Location = new System.Drawing.Point(206, 44);
            this.m_chkStringColorGlobal.Name = "m_chkStringColorGlobal";
            this.m_chkStringColorGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkStringColorGlobal.TabIndex = 4;
            this.m_chkStringColorGlobal.Text = "Apply globally";
            this.m_chkStringColorGlobal.UseVisualStyleBackColor = true;
            //
            // m_btnStringColor
            //
            this.m_btnStringColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_btnStringColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(220)))), ((int)(((byte)(168)))));
            this.m_btnStringColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnStringColor.ForeColor = System.Drawing.Color.Black;
            this.m_btnStringColor.Location = new System.Drawing.Point(332, 40);
            this.m_btnStringColor.Name = "m_btnStringColor";
            this.m_btnStringColor.Size = new System.Drawing.Size(80, 25);
            this.m_btnStringColor.TabIndex = 5;
            this.m_btnStringColor.Text = "Green";
            this.m_btnStringColor.UseVisualStyleBackColor = false;
            //
            // m_lblCommentColor
            //
            this.m_lblCommentColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblCommentColor.AutoSize = true;
            this.m_lblCommentColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblCommentColor.Location = new System.Drawing.Point(3, 80);
            this.m_lblCommentColor.Name = "m_lblCommentColor";
            this.m_lblCommentColor.Size = new System.Drawing.Size(87, 15);
            this.m_lblCommentColor.TabIndex = 6;
            this.m_lblCommentColor.Text = "Comment color";
            //
            // m_chkCommentColorGlobal
            //
            this.m_chkCommentColorGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkCommentColorGlobal.AutoSize = true;
            this.m_chkCommentColorGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkCommentColorGlobal.Location = new System.Drawing.Point(206, 79);
            this.m_chkCommentColorGlobal.Name = "m_chkCommentColorGlobal";
            this.m_chkCommentColorGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkCommentColorGlobal.TabIndex = 7;
            this.m_chkCommentColorGlobal.Text = "Apply globally";
            this.m_chkCommentColorGlobal.UseVisualStyleBackColor = true;
            //
            // m_btnCommentColor
            //
            this.m_btnCommentColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_btnCommentColor.BackColor = System.Drawing.Color.Gray;
            this.m_btnCommentColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnCommentColor.ForeColor = System.Drawing.Color.White;
            this.m_btnCommentColor.Location = new System.Drawing.Point(332, 75);
            this.m_btnCommentColor.Name = "m_btnCommentColor";
            this.m_btnCommentColor.Size = new System.Drawing.Size(80, 25);
            this.m_btnCommentColor.TabIndex = 8;
            this.m_btnCommentColor.Text = "Gray";
            this.m_btnCommentColor.UseVisualStyleBackColor = false;
            //
            // m_lblNumberColor
            //
            this.m_lblNumberColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblNumberColor.AutoSize = true;
            this.m_lblNumberColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblNumberColor.Location = new System.Drawing.Point(3, 115);
            this.m_lblNumberColor.Name = "m_lblNumberColor";
            this.m_lblNumberColor.Size = new System.Drawing.Size(78, 15);
            this.m_lblNumberColor.TabIndex = 9;
            this.m_lblNumberColor.Text = "Number color";
            //
            // m_chkNumberColorGlobal
            //
            this.m_chkNumberColorGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkNumberColorGlobal.AutoSize = true;
            this.m_chkNumberColorGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkNumberColorGlobal.Location = new System.Drawing.Point(206, 114);
            this.m_chkNumberColorGlobal.Name = "m_chkNumberColorGlobal";
            this.m_chkNumberColorGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkNumberColorGlobal.TabIndex = 10;
            this.m_chkNumberColorGlobal.Text = "Apply globally";
            this.m_chkNumberColorGlobal.UseVisualStyleBackColor = true;
            //
            // m_btnNumberColor
            //
            this.m_btnNumberColor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_btnNumberColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(157)))), ((int)(((byte)(133)))));
            this.m_btnNumberColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnNumberColor.ForeColor = System.Drawing.Color.Black;
            this.m_btnNumberColor.Location = new System.Drawing.Point(332, 110);
            this.m_btnNumberColor.Name = "m_btnNumberColor";
            this.m_btnNumberColor.Size = new System.Drawing.Size(80, 25);
            this.m_btnNumberColor.TabIndex = 11;
            this.m_btnNumberColor.Text = "Orange";
            this.m_btnNumberColor.UseVisualStyleBackColor = false;
            //
            // m_lblDefaultForeground
            //
            this.m_lblDefaultForeground.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblDefaultForeground.AutoSize = true;
            this.m_lblDefaultForeground.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblDefaultForeground.Location = new System.Drawing.Point(3, 150);
            this.m_lblDefaultForeground.Name = "m_lblDefaultForeground";
            this.m_lblDefaultForeground.Size = new System.Drawing.Size(115, 15);
            this.m_lblDefaultForeground.TabIndex = 12;
            this.m_lblDefaultForeground.Text = "Default text color";
            //
            // m_chkDefaultForegroundGlobal
            //
            this.m_chkDefaultForegroundGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkDefaultForegroundGlobal.AutoSize = true;
            this.m_chkDefaultForegroundGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkDefaultForegroundGlobal.Location = new System.Drawing.Point(206, 149);
            this.m_chkDefaultForegroundGlobal.Name = "m_chkDefaultForegroundGlobal";
            this.m_chkDefaultForegroundGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkDefaultForegroundGlobal.TabIndex = 13;
            this.m_chkDefaultForegroundGlobal.Text = "Apply globally";
            this.m_chkDefaultForegroundGlobal.UseVisualStyleBackColor = true;
            //
            // m_btnDefaultForeground
            //
            this.m_btnDefaultForeground.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_btnDefaultForeground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.m_btnDefaultForeground.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnDefaultForeground.ForeColor = System.Drawing.Color.Black;
            this.m_btnDefaultForeground.Location = new System.Drawing.Point(332, 145);
            this.m_btnDefaultForeground.Name = "m_btnDefaultForeground";
            this.m_btnDefaultForeground.Size = new System.Drawing.Size(80, 25);
            this.m_btnDefaultForeground.TabIndex = 14;
            this.m_btnDefaultForeground.Text = "White";
            this.m_btnDefaultForeground.UseVisualStyleBackColor = false;
            //
            // m_lblEditorBackground
            //
            this.m_lblEditorBackground.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblEditorBackground.AutoSize = true;
            this.m_lblEditorBackground.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblEditorBackground.Location = new System.Drawing.Point(3, 185);
            this.m_lblEditorBackground.Name = "m_lblEditorBackground";
            this.m_lblEditorBackground.Size = new System.Drawing.Size(116, 15);
            this.m_lblEditorBackground.TabIndex = 15;
            this.m_lblEditorBackground.Text = "Editor background";
            //
            // m_chkEditorBackgroundGlobal
            //
            this.m_chkEditorBackgroundGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkEditorBackgroundGlobal.AutoSize = true;
            this.m_chkEditorBackgroundGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkEditorBackgroundGlobal.Location = new System.Drawing.Point(206, 184);
            this.m_chkEditorBackgroundGlobal.Name = "m_chkEditorBackgroundGlobal";
            this.m_chkEditorBackgroundGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkEditorBackgroundGlobal.TabIndex = 16;
            this.m_chkEditorBackgroundGlobal.Text = "Apply globally";
            this.m_chkEditorBackgroundGlobal.UseVisualStyleBackColor = true;
            //
            // m_btnEditorBackground
            //
            this.m_btnEditorBackground.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_btnEditorBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.m_btnEditorBackground.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnEditorBackground.ForeColor = System.Drawing.Color.White;
            this.m_btnEditorBackground.Location = new System.Drawing.Point(332, 180);
            this.m_btnEditorBackground.Name = "m_btnEditorBackground";
            this.m_btnEditorBackground.Size = new System.Drawing.Size(80, 25);
            this.m_btnEditorBackground.TabIndex = 17;
            this.m_btnEditorBackground.Text = "Dark";
            this.m_btnEditorBackground.UseVisualStyleBackColor = false;
            //
            // m_grpExport
            //
            this.m_grpExport.BackColor = System.Drawing.Color.White;
            this.m_grpExport.Controls.Add(this.m_tblExport);
            this.m_grpExport.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_grpExport.Location = new System.Drawing.Point(0, 0);
            this.m_grpExport.Name = "m_grpExport";
            this.m_grpExport.Padding = new System.Windows.Forms.Padding(10);
            this.m_grpExport.Size = new System.Drawing.Size(660, 100);
            this.m_grpExport.TabIndex = 0;
            this.m_grpExport.TabStop = false;
            this.m_grpExport.Text = "Export Options";
            //
            // m_tblExport
            //
            this.m_tblExport.ColumnCount = 3;
            this.m_tblExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.m_tblExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblExport.Controls.Add(this.m_lblDefaultExport, 0, 0);
            this.m_tblExport.Controls.Add(this.m_chkDefaultExportGlobal, 1, 0);
            this.m_tblExport.Controls.Add(this.m_cmbDefaultExport, 2, 0);
            this.m_tblExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tblExport.Location = new System.Drawing.Point(10, 24);
            this.m_tblExport.Name = "m_tblExport";
            this.m_tblExport.RowCount = 2;
            this.m_tblExport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.m_tblExport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblExport.Size = new System.Drawing.Size(640, 66);
            this.m_tblExport.TabIndex = 0;
            //
            // m_lblDefaultExport
            //
            this.m_lblDefaultExport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblDefaultExport.AutoSize = true;
            this.m_lblDefaultExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblDefaultExport.Location = new System.Drawing.Point(3, 10);
            this.m_lblDefaultExport.Name = "m_lblDefaultExport";
            this.m_lblDefaultExport.Size = new System.Drawing.Size(103, 15);
            this.m_lblDefaultExport.TabIndex = 0;
            this.m_lblDefaultExport.Text = "Default export type";
            //
            // m_chkDefaultExportGlobal
            //
            this.m_chkDefaultExportGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkDefaultExportGlobal.AutoSize = true;
            this.m_chkDefaultExportGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkDefaultExportGlobal.Location = new System.Drawing.Point(206, 9);
            this.m_chkDefaultExportGlobal.Name = "m_chkDefaultExportGlobal";
            this.m_chkDefaultExportGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkDefaultExportGlobal.TabIndex = 1;
            this.m_chkDefaultExportGlobal.Text = "Apply globally";
            this.m_chkDefaultExportGlobal.UseVisualStyleBackColor = true;
            //
            // m_cmbDefaultExport
            //
            this.m_cmbDefaultExport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_cmbDefaultExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cmbDefaultExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_cmbDefaultExport.FormattingEnabled = true;
            this.m_cmbDefaultExport.Items.AddRange(new object[] {
            "DataTableForm",
            "NewWorksheet",
            "SelectedCell"});
            this.m_cmbDefaultExport.Location = new System.Drawing.Point(332, 5);
            this.m_cmbDefaultExport.Name = "m_cmbDefaultExport";
            this.m_cmbDefaultExport.Size = new System.Drawing.Size(150, 23);
            this.m_cmbDefaultExport.TabIndex = 2;
            //
            // m_grpKeywords
            //
            this.m_grpKeywords.BackColor = System.Drawing.Color.White;
            this.m_grpKeywords.Controls.Add(this.m_tblKeywords);
            this.m_grpKeywords.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpKeywords.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_grpKeywords.Location = new System.Drawing.Point(0, 0);
            this.m_grpKeywords.Name = "m_grpKeywords";
            this.m_grpKeywords.Padding = new System.Windows.Forms.Padding(10);
            this.m_grpKeywords.Size = new System.Drawing.Size(660, 250);
            this.m_grpKeywords.TabIndex = 0;
            this.m_grpKeywords.TabStop = false;
            this.m_grpKeywords.Text = "SQL Keywords (Space-Separated)";
            //
            // m_tblKeywords
            //
            this.m_tblKeywords.ColumnCount = 3;
            this.m_tblKeywords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblKeywords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblKeywords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblKeywords.Controls.Add(this.m_lblMsSqlKeywords, 0, 0);
            this.m_tblKeywords.Controls.Add(this.m_chkMsSqlKeywordsGlobal, 1, 0);
            this.m_tblKeywords.Controls.Add(this.m_txtMsSqlKeywords, 0, 1);
            this.m_tblKeywords.Controls.Add(this.m_lblOracleKeywords, 0, 2);
            this.m_tblKeywords.Controls.Add(this.m_chkOracleKeywordsGlobal, 1, 2);
            this.m_tblKeywords.Controls.Add(this.m_txtOracleKeywords, 0, 3);
            this.m_tblKeywords.Controls.Add(this.m_btnResetKeywords, 2, 4);
            this.m_tblKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tblKeywords.Location = new System.Drawing.Point(10, 24);
            this.m_tblKeywords.Name = "m_tblKeywords";
            this.m_tblKeywords.RowCount = 5;
            this.m_tblKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.m_tblKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.m_tblKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.m_tblKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.m_tblKeywords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.m_tblKeywords.Size = new System.Drawing.Size(640, 216);
            this.m_tblKeywords.TabIndex = 0;
            //
            // m_lblMsSqlKeywords
            //
            this.m_lblMsSqlKeywords.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblMsSqlKeywords.AutoSize = true;
            this.m_lblMsSqlKeywords.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblMsSqlKeywords.Location = new System.Drawing.Point(3, 5);
            this.m_lblMsSqlKeywords.Name = "m_lblMsSqlKeywords";
            this.m_lblMsSqlKeywords.Size = new System.Drawing.Size(98, 15);
            this.m_lblMsSqlKeywords.TabIndex = 0;
            this.m_lblMsSqlKeywords.Text = "MS SQL Server";
            //
            // m_chkMsSqlKeywordsGlobal
            //
            this.m_chkMsSqlKeywordsGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkMsSqlKeywordsGlobal.AutoSize = true;
            this.m_chkMsSqlKeywordsGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkMsSqlKeywordsGlobal.Location = new System.Drawing.Point(126, 4);
            this.m_chkMsSqlKeywordsGlobal.Name = "m_chkMsSqlKeywordsGlobal";
            this.m_chkMsSqlKeywordsGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkMsSqlKeywordsGlobal.TabIndex = 1;
            this.m_chkMsSqlKeywordsGlobal.Text = "Apply globally";
            this.m_chkMsSqlKeywordsGlobal.UseVisualStyleBackColor = true;
            //
            // m_txtMsSqlKeywords
            //
            this.m_txtMsSqlKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tblKeywords.SetColumnSpan(this.m_txtMsSqlKeywords, 3);
            this.m_txtMsSqlKeywords.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_txtMsSqlKeywords.Location = new System.Drawing.Point(3, 28);
            this.m_txtMsSqlKeywords.Multiline = true;
            this.m_txtMsSqlKeywords.Name = "m_txtMsSqlKeywords";
            this.m_txtMsSqlKeywords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_txtMsSqlKeywords.Size = new System.Drawing.Size(634, 64);
            this.m_txtMsSqlKeywords.TabIndex = 2;
            //
            // m_lblOracleKeywords
            //
            this.m_lblOracleKeywords.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblOracleKeywords.AutoSize = true;
            this.m_lblOracleKeywords.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblOracleKeywords.Location = new System.Drawing.Point(3, 98);
            this.m_lblOracleKeywords.Name = "m_lblOracleKeywords";
            this.m_lblOracleKeywords.Size = new System.Drawing.Size(45, 15);
            this.m_lblOracleKeywords.TabIndex = 3;
            this.m_lblOracleKeywords.Text = "Oracle";
            //
            // m_chkOracleKeywordsGlobal
            //
            this.m_chkOracleKeywordsGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkOracleKeywordsGlobal.AutoSize = true;
            this.m_chkOracleKeywordsGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkOracleKeywordsGlobal.Location = new System.Drawing.Point(126, 97);
            this.m_chkOracleKeywordsGlobal.Name = "m_chkOracleKeywordsGlobal";
            this.m_chkOracleKeywordsGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkOracleKeywordsGlobal.TabIndex = 4;
            this.m_chkOracleKeywordsGlobal.Text = "Apply globally";
            this.m_chkOracleKeywordsGlobal.UseVisualStyleBackColor = true;
            //
            // m_txtOracleKeywords
            //
            this.m_txtOracleKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tblKeywords.SetColumnSpan(this.m_txtOracleKeywords, 3);
            this.m_txtOracleKeywords.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_txtOracleKeywords.Location = new System.Drawing.Point(3, 121);
            this.m_txtOracleKeywords.Multiline = true;
            this.m_txtOracleKeywords.Name = "m_txtOracleKeywords";
            this.m_txtOracleKeywords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_txtOracleKeywords.Size = new System.Drawing.Size(634, 64);
            this.m_txtOracleKeywords.TabIndex = 5;
            //
            // m_btnResetKeywords
            //
            this.m_btnResetKeywords.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.m_btnResetKeywords.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_btnResetKeywords.Location = new System.Drawing.Point(545, 188);
            this.m_btnResetKeywords.Name = "m_btnResetKeywords";
            this.m_btnResetKeywords.Size = new System.Drawing.Size(92, 24);
            this.m_btnResetKeywords.TabIndex = 6;
            this.m_btnResetKeywords.Text = "Reset to Default";
            this.m_btnResetKeywords.UseVisualStyleBackColor = true;
            //
            // m_grpStartup
            //
            this.m_grpStartup.BackColor = System.Drawing.Color.White;
            this.m_grpStartup.Controls.Add(this.m_tblStartup);
            this.m_grpStartup.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpStartup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_grpStartup.Location = new System.Drawing.Point(0, 0);
            this.m_grpStartup.Name = "m_grpStartup";
            this.m_grpStartup.Padding = new System.Windows.Forms.Padding(10);
            this.m_grpStartup.Size = new System.Drawing.Size(660, 200);
            this.m_grpStartup.TabIndex = 0;
            this.m_grpStartup.TabStop = false;
            this.m_grpStartup.Text = "Startup";
            //
            // m_tblStartup
            //
            this.m_tblStartup.ColumnCount = 3;
            this.m_tblStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.m_tblStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.m_tblStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblStartup.Controls.Add(this.m_lblStartQuery, 0, 0);
            this.m_tblStartup.Controls.Add(this.m_chkStartQueryGlobal, 1, 0);
            this.m_tblStartup.Controls.Add(this.m_txtStartQuery, 0, 1);
            this.m_tblStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tblStartup.Location = new System.Drawing.Point(10, 24);
            this.m_tblStartup.Name = "m_tblStartup";
            this.m_tblStartup.RowCount = 2;
            this.m_tblStartup.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.m_tblStartup.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.m_tblStartup.Size = new System.Drawing.Size(640, 166);
            this.m_tblStartup.TabIndex = 0;
            //
            // m_lblStartQuery
            //
            this.m_lblStartQuery.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_lblStartQuery.AutoSize = true;
            this.m_lblStartQuery.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblStartQuery.Location = new System.Drawing.Point(3, 5);
            this.m_lblStartQuery.Name = "m_lblStartQuery";
            this.m_lblStartQuery.Size = new System.Drawing.Size(152, 15);
            this.m_lblStartQuery.TabIndex = 0;
            this.m_lblStartQuery.Text = "Start SQL query on launch";
            //
            // m_chkStartQueryGlobal
            //
            this.m_chkStartQueryGlobal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_chkStartQueryGlobal.AutoSize = true;
            this.m_chkStartQueryGlobal.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_chkStartQueryGlobal.Location = new System.Drawing.Point(206, 4);
            this.m_chkStartQueryGlobal.Name = "m_chkStartQueryGlobal";
            this.m_chkStartQueryGlobal.Size = new System.Drawing.Size(97, 17);
            this.m_chkStartQueryGlobal.TabIndex = 1;
            this.m_chkStartQueryGlobal.Text = "Apply globally";
            this.m_chkStartQueryGlobal.UseVisualStyleBackColor = true;
            //
            // m_txtStartQuery
            //
            this.m_txtStartQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tblStartup.SetColumnSpan(this.m_txtStartQuery, 3);
            this.m_txtStartQuery.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_txtStartQuery.Location = new System.Drawing.Point(3, 28);
            this.m_txtStartQuery.Multiline = true;
            this.m_txtStartQuery.Name = "m_txtStartQuery";
            this.m_txtStartQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_txtStartQuery.Size = new System.Drawing.Size(634, 135);
            this.m_txtStartQuery.TabIndex = 2;
            //
            // SettingsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 660);
            this.Controls.Add(this.m_splitMain);
            this.Controls.Add(this.m_panelButtons);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL Extractor for Excel – Settings";
            this.m_splitMain.Panel1.ResumeLayout(false);
            this.m_splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_splitMain)).EndInit();
            this.m_splitMain.ResumeLayout(false);
            this.m_panelButtons.ResumeLayout(false);
            this.m_panelButtons.PerformLayout();

            this.m_grpGeneral.ResumeLayout(false);
            this.m_tblGeneral.ResumeLayout(false);
            this.m_tblGeneral.PerformLayout();

            this.m_grpQueryEditor.ResumeLayout(false);
            this.m_tblQueryEditor.ResumeLayout(false);
            this.m_tblQueryEditor.PerformLayout();

            this.m_grpSyntax.ResumeLayout(false);
            this.m_tblSyntax.ResumeLayout(false);
            this.m_tblSyntax.PerformLayout();

            this.m_grpExport.ResumeLayout(false);
            this.m_tblExport.ResumeLayout(false);
            this.m_tblExport.PerformLayout();

            this.m_grpKeywords.ResumeLayout(false);
            this.m_tblKeywords.ResumeLayout(false);
            this.m_tblKeywords.PerformLayout();

            this.m_grpStartup.ResumeLayout(false);
            this.m_tblStartup.ResumeLayout(false);
            this.m_tblStartup.PerformLayout();

            this.ResumeLayout(false);

        }

        #endregion

        // Main layout controls
        private System.Windows.Forms.SplitContainer m_splitMain;
        private System.Windows.Forms.TreeView m_treeCategories;
        private System.Windows.Forms.Panel m_panelContent;
        private System.Windows.Forms.Panel m_panelButtons;
        private System.Windows.Forms.Button m_btnOK;
        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.Button m_btnApply;
        private System.Windows.Forms.Button m_btnReset;
        private System.Windows.Forms.Label m_lblAnnotation;

        // General category
        private System.Windows.Forms.GroupBox m_grpGeneral;
        private System.Windows.Forms.TableLayoutPanel m_tblGeneral;
        private System.Windows.Forms.Label m_lblKeepOnTop;
        private System.Windows.Forms.CheckBox m_chkKeepOnTopGlobal;
        private System.Windows.Forms.CheckBox m_chkKeepOnTop;

        // Query Editor category
        private System.Windows.Forms.GroupBox m_grpQueryEditor;
        private System.Windows.Forms.TableLayoutPanel m_tblQueryEditor;
        private System.Windows.Forms.Label m_lblTextWrap;
        private System.Windows.Forms.CheckBox m_chkTextWrapGlobal;
        private System.Windows.Forms.CheckBox m_chkTextWrap;

        // Syntax Highlighting category
        private System.Windows.Forms.GroupBox m_grpSyntax;
        private System.Windows.Forms.TableLayoutPanel m_tblSyntax;
        private System.Windows.Forms.Label m_lblKeywordColor;
        private System.Windows.Forms.CheckBox m_chkKeywordColorGlobal;
        private System.Windows.Forms.Button m_btnKeywordColor;
        private System.Windows.Forms.Label m_lblStringColor;
        private System.Windows.Forms.CheckBox m_chkStringColorGlobal;
        private System.Windows.Forms.Button m_btnStringColor;
        private System.Windows.Forms.Label m_lblCommentColor;
        private System.Windows.Forms.CheckBox m_chkCommentColorGlobal;
        private System.Windows.Forms.Button m_btnCommentColor;
        private System.Windows.Forms.Label m_lblNumberColor;
        private System.Windows.Forms.CheckBox m_chkNumberColorGlobal;
        private System.Windows.Forms.Button m_btnNumberColor;
        private System.Windows.Forms.Label m_lblDefaultForeground;
        private System.Windows.Forms.CheckBox m_chkDefaultForegroundGlobal;
        private System.Windows.Forms.Button m_btnDefaultForeground;
        private System.Windows.Forms.Label m_lblEditorBackground;
        private System.Windows.Forms.CheckBox m_chkEditorBackgroundGlobal;
        private System.Windows.Forms.Button m_btnEditorBackground;

        // Export category
        private System.Windows.Forms.GroupBox m_grpExport;
        private System.Windows.Forms.TableLayoutPanel m_tblExport;
        private System.Windows.Forms.Label m_lblDefaultExport;
        private System.Windows.Forms.CheckBox m_chkDefaultExportGlobal;
        private System.Windows.Forms.ComboBox m_cmbDefaultExport;

        // Keywords category
        private System.Windows.Forms.GroupBox m_grpKeywords;
        private System.Windows.Forms.TableLayoutPanel m_tblKeywords;
        private System.Windows.Forms.Label m_lblMsSqlKeywords;
        private System.Windows.Forms.CheckBox m_chkMsSqlKeywordsGlobal;
        private System.Windows.Forms.TextBox m_txtMsSqlKeywords;
        private System.Windows.Forms.Label m_lblOracleKeywords;
        private System.Windows.Forms.CheckBox m_chkOracleKeywordsGlobal;
        private System.Windows.Forms.TextBox m_txtOracleKeywords;
        private System.Windows.Forms.Button m_btnResetKeywords;

        // Startup category
        private System.Windows.Forms.GroupBox m_grpStartup;
        private System.Windows.Forms.TableLayoutPanel m_tblStartup;
        private System.Windows.Forms.Label m_lblStartQuery;
        private System.Windows.Forms.CheckBox m_chkStartQueryGlobal;
        private System.Windows.Forms.TextBox m_txtStartQuery;
    }
}
