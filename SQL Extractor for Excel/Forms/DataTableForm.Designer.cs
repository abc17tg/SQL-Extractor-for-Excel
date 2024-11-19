namespace SQL_Extractor_for_Excel.Forms
{
    partial class DataTableForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTableForm));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.parametersTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.headersCheckBox = new System.Windows.Forms.CheckBox();
            this.pasteButton = new System.Windows.Forms.Button();
            this.dataTableDimentionsLabel = new System.Windows.Forms.Label();
            this.queryLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.queryRichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.parametersTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersTableLayoutPanel.SetColumnSpan(this.dataGridView, 5);
            this.dataGridView.Cursor = System.Windows.Forms.Cursors.Cross;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView.ImeMode = System.Windows.Forms.ImeMode.On;
            this.dataGridView.Location = new System.Drawing.Point(3, 33);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1025, 379);
            this.dataGridView.TabIndex = 0;
            // 
            // parametersTableLayoutPanel
            // 
            this.parametersTableLayoutPanel.AutoSize = true;
            this.parametersTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.parametersTableLayoutPanel.ColumnCount = 5;
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.parametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.parametersTableLayoutPanel.Controls.Add(this.headersCheckBox, 0, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.dataGridView, 0, 1);
            this.parametersTableLayoutPanel.Controls.Add(this.pasteButton, 2, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.dataTableDimentionsLabel, 1, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.queryLabel, 4, 0);
            this.parametersTableLayoutPanel.Controls.Add(this.saveButton, 3, 0);
            this.parametersTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.parametersTableLayoutPanel.MinimumSize = new System.Drawing.Size(0, 35);
            this.parametersTableLayoutPanel.Name = "parametersTableLayoutPanel";
            this.parametersTableLayoutPanel.RowCount = 2;
            this.parametersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.parametersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.parametersTableLayoutPanel.Size = new System.Drawing.Size(1031, 415);
            this.parametersTableLayoutPanel.TabIndex = 32;
            // 
            // headersCheckBox
            // 
            this.headersCheckBox.AutoSize = true;
            this.headersCheckBox.Checked = true;
            this.headersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.headersCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headersCheckBox.Location = new System.Drawing.Point(3, 3);
            this.headersCheckBox.Name = "headersCheckBox";
            this.headersCheckBox.Size = new System.Drawing.Size(66, 24);
            this.headersCheckBox.TabIndex = 28;
            this.headersCheckBox.Text = "Headers";
            this.headersCheckBox.UseVisualStyleBackColor = true;
            this.headersCheckBox.CheckedChanged += new System.EventHandler(this.headersCheckBox_CheckedChanged);
            // 
            // pasteButton
            // 
            this.pasteButton.AutoSize = true;
            this.pasteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pasteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pasteButton.Location = new System.Drawing.Point(140, 3);
            this.pasteButton.MinimumSize = new System.Drawing.Size(150, 25);
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Size = new System.Drawing.Size(708, 25);
            this.pasteButton.TabIndex = 30;
            this.pasteButton.Text = "Paste";
            this.pasteButton.UseVisualStyleBackColor = true;
            this.pasteButton.Click += new System.EventHandler(this.pasteButton_Click);
            // 
            // dataTableDimentionsLabel
            // 
            this.dataTableDimentionsLabel.AutoSize = true;
            this.dataTableDimentionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTableDimentionsLabel.Location = new System.Drawing.Point(75, 0);
            this.dataTableDimentionsLabel.Name = "dataTableDimentionsLabel";
            this.dataTableDimentionsLabel.Size = new System.Drawing.Size(59, 30);
            this.dataTableDimentionsLabel.TabIndex = 31;
            this.dataTableDimentionsLabel.Text = "Dimentions";
            this.dataTableDimentionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // queryLabel
            // 
            this.queryLabel.AutoSize = true;
            this.queryLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryLabel.Location = new System.Drawing.Point(974, 0);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(54, 30);
            this.queryLabel.TabIndex = 32;
            this.queryLabel.Text = "Query";
            this.queryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.queryLabel.Click += new System.EventHandler(this.queryLabel_Click);
            // 
            // saveButton
            // 
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveButton.Location = new System.Drawing.Point(854, 3);
            this.saveButton.MinimumSize = new System.Drawing.Size(0, 25);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(114, 25);
            this.saveButton.TabIndex = 33;
            this.saveButton.Text = "Save as txt";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // queryRichTextBox
            // 
            this.queryRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.queryRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryRichTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.queryRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.queryRichTextBox.Name = "queryRichTextBox";
            this.queryRichTextBox.ReadOnly = true;
            this.queryRichTextBox.Size = new System.Drawing.Size(1031, 415);
            this.queryRichTextBox.TabIndex = 34;
            this.queryRichTextBox.Text = "";
            this.queryRichTextBox.Visible = false;
            this.queryRichTextBox.DoubleClick += new System.EventHandler(this.queryRichTextBox_DoubleClick);
            // 
            // DataTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 415);
            this.Controls.Add(this.parametersTableLayoutPanel);
            this.Controls.Add(this.queryRichTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataTableForm";
            this.Text = "DataTable";
            this.Load += new System.EventHandler(this.DataTableForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.parametersTableLayoutPanel.ResumeLayout(false);
            this.parametersTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TableLayoutPanel parametersTableLayoutPanel;
        private System.Windows.Forms.CheckBox headersCheckBox;
        private System.Windows.Forms.Button pasteButton;
        private System.Windows.Forms.Label dataTableDimentionsLabel;
        private System.Windows.Forms.Label queryLabel;
        private System.Windows.Forms.RichTextBox queryRichTextBox;
        private System.Windows.Forms.Button saveButton;
    }
}