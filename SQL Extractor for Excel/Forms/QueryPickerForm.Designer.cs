using System;
using System.Windows.Forms;

namespace SQL_Extractor_for_Excel.Forms
{
    partial class QueryPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryPickerForm));
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.catalogTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.queriesTreeView = new System.Windows.Forms.TreeView();
            this.queryViewEditorScintilla = new ScintillaNET.Scintilla();
            this.buttonsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pasteQueryButton = new System.Windows.Forms.Button();
            this.replaceQueryButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.catalogTableLayoutPanel.SuspendLayout();
            this.buttonsFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.splitContainer, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.buttonsFlowLayoutPanel, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(592, 447);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 3);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.catalogTableLayoutPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.queryViewEditorScintilla);
            this.splitContainer.Size = new System.Drawing.Size(586, 406);
            this.splitContainer.SplitterDistance = 272;
            this.splitContainer.TabIndex = 6;
            // 
            // catalogTableLayoutPanel
            // 
            this.catalogTableLayoutPanel.ColumnCount = 1;
            this.catalogTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.catalogTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.catalogTableLayoutPanel.Controls.Add(this.searchTextBox, 0, 0);
            this.catalogTableLayoutPanel.Controls.Add(this.queriesTreeView, 0, 1);
            this.catalogTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.catalogTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.catalogTableLayoutPanel.Name = "catalogTableLayoutPanel";
            this.catalogTableLayoutPanel.RowCount = 2;
            this.catalogTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.catalogTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.catalogTableLayoutPanel.Size = new System.Drawing.Size(272, 406);
            this.catalogTableLayoutPanel.TabIndex = 0;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(3, 5);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(266, 20);
            this.searchTextBox.TabIndex = 4;
            // 
            // queriesTreeView
            // 
            this.queriesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.queriesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queriesTreeView.HideSelection = false;
            this.queriesTreeView.Indent = 15;
            this.queriesTreeView.Location = new System.Drawing.Point(3, 33);
            this.queriesTreeView.Name = "queriesTreeView";
            this.queriesTreeView.Size = new System.Drawing.Size(266, 370);
            this.queriesTreeView.TabIndex = 5;
            // 
            // queryViewEditorScintilla
            // 
            this.queryViewEditorScintilla.BorderStyle = ScintillaNET.BorderStyle.FixedSingle;
            this.queryViewEditorScintilla.CaretForeColor = System.Drawing.Color.White;
            this.queryViewEditorScintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryViewEditorScintilla.LexerName = "sql";
            this.queryViewEditorScintilla.Location = new System.Drawing.Point(0, 0);
            this.queryViewEditorScintilla.Name = "queryViewEditorScintilla";
            this.queryViewEditorScintilla.ReadOnly = true;
            this.queryViewEditorScintilla.Size = new System.Drawing.Size(310, 406);
            this.queryViewEditorScintilla.TabIndex = 3;
            this.queryViewEditorScintilla.UseTabs = true;
            this.queryViewEditorScintilla.WrapIndentMode = ScintillaNET.WrapIndentMode.Indent;
            this.queryViewEditorScintilla.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // buttonsFlowLayoutPanel
            // 
            this.buttonsFlowLayoutPanel.Controls.Add(this.pasteQueryButton);
            this.buttonsFlowLayoutPanel.Controls.Add(this.replaceQueryButton);
            this.buttonsFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.buttonsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonsFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonsFlowLayoutPanel.Location = new System.Drawing.Point(3, 415);
            this.buttonsFlowLayoutPanel.Name = "buttonsFlowLayoutPanel";
            this.buttonsFlowLayoutPanel.Size = new System.Drawing.Size(586, 29);
            this.buttonsFlowLayoutPanel.TabIndex = 7;
            // 
            // pasteQueryButton
            // 
            this.pasteQueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pasteQueryButton.AutoSize = true;
            this.pasteQueryButton.Location = new System.Drawing.Point(510, 3);
            this.pasteQueryButton.Name = "pasteQueryButton";
            this.pasteQueryButton.Size = new System.Drawing.Size(73, 23);
            this.pasteQueryButton.TabIndex = 2;
            this.pasteQueryButton.Text = "Paste query";
            this.pasteQueryButton.UseVisualStyleBackColor = true;
            this.pasteQueryButton.Click += new System.EventHandler(this.pasteQueryButton_Click);
            // 
            // replaceQueryButton
            // 
            this.replaceQueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceQueryButton.AutoSize = true;
            this.replaceQueryButton.Location = new System.Drawing.Point(418, 3);
            this.replaceQueryButton.Name = "replaceQueryButton";
            this.replaceQueryButton.Size = new System.Drawing.Size(86, 23);
            this.replaceQueryButton.TabIndex = 1;
            this.replaceQueryButton.Text = "Replace query";
            this.replaceQueryButton.UseVisualStyleBackColor = true;
            this.replaceQueryButton.Click += new System.EventHandler(this.replaceQueryButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.Location = new System.Drawing.Point(362, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(50, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // QueryPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 447);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QueryPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Query Picker Form";
            this.TopMost = true;
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.catalogTableLayoutPanel.ResumeLayout(false);
            this.catalogTableLayoutPanel.PerformLayout();
            this.buttonsFlowLayoutPanel.ResumeLayout(false);
            this.buttonsFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button replaceQueryButton;
        private System.Windows.Forms.Button pasteQueryButton;
        private ScintillaNET.Scintilla queryViewEditorScintilla;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.TreeView queriesTreeView;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel catalogTableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel buttonsFlowLayoutPanel;
    }
}