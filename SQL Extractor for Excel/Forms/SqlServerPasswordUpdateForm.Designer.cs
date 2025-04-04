namespace SQL_Extractor_for_Excel.Forms
{
    partial class SqlServerPasswordUpdateForm
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.TestBtn = new System.Windows.Forms.Button();
            this.ServerComboBox = new System.Windows.Forms.ComboBox();
            this.AcceptBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ShowOldPasswordLabel = new System.Windows.Forms.Label();
            this.ShowPasswordLabel = new System.Windows.Forms.Label();
            this.OldPasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.MainLabel = new System.Windows.Forms.Label();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.05128F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.46154F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.05128F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.4359F));
            this.tableLayoutPanel.Controls.Add(this.TestBtn, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.ServerComboBox, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.AcceptBtn, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.CancelBtn, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.ShowOldPasswordLabel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.ShowPasswordLabel, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.OldPasswordTextBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.PasswordTextBox, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.MainLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.ResultLabel, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(687, 88);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // TestBtn
            // 
            this.TestBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestBtn.Location = new System.Drawing.Point(3, 61);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(214, 24);
            this.TestBtn.TabIndex = 0;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // ServerComboBox
            // 
            this.ServerComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerComboBox.FormattingEnabled = true;
            this.ServerComboBox.Location = new System.Drawing.Point(315, 3);
            this.ServerComboBox.Name = "ServerComboBox";
            this.ServerComboBox.Size = new System.Drawing.Size(214, 21);
            this.ServerComboBox.TabIndex = 3;
            this.ServerComboBox.SelectedIndexChanged += new System.EventHandler(this.ServerComboBox_SelectedIndexChanged);
            // 
            // AcceptBtn
            // 
            this.AcceptBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AcceptBtn.Location = new System.Drawing.Point(535, 61);
            this.AcceptBtn.Name = "AcceptBtn";
            this.AcceptBtn.Size = new System.Drawing.Size(149, 24);
            this.AcceptBtn.TabIndex = 2;
            this.AcceptBtn.Text = "Accept";
            this.AcceptBtn.UseVisualStyleBackColor = true;
            this.AcceptBtn.Click += new System.EventHandler(this.AcceptBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelBtn.Location = new System.Drawing.Point(315, 61);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(214, 24);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ShowOldPasswordLabel
            // 
            this.ShowOldPasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ShowOldPasswordLabel.AutoSize = true;
            this.ShowOldPasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowOldPasswordLabel.Location = new System.Drawing.Point(223, 36);
            this.ShowOldPasswordLabel.Name = "ShowOldPasswordLabel";
            this.ShowOldPasswordLabel.Size = new System.Drawing.Size(38, 15);
            this.ShowOldPasswordLabel.TabIndex = 5;
            this.ShowOldPasswordLabel.Text = "Show";
            this.ShowOldPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ShowOldPasswordLabel.MouseEnter += new System.EventHandler(this.ShowOldPasswordLabel_MouseEnter);
            this.ShowOldPasswordLabel.MouseLeave += new System.EventHandler(this.ShowOldPasswordLabel_MouseLeave);
            // 
            // ShowPasswordLabel
            // 
            this.ShowPasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ShowPasswordLabel.AutoSize = true;
            this.ShowPasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowPasswordLabel.Location = new System.Drawing.Point(535, 36);
            this.ShowPasswordLabel.Name = "ShowPasswordLabel";
            this.ShowPasswordLabel.Size = new System.Drawing.Size(38, 15);
            this.ShowPasswordLabel.TabIndex = 6;
            this.ShowPasswordLabel.Text = "Show";
            this.ShowPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ShowPasswordLabel.MouseEnter += new System.EventHandler(this.ShowPasswordLabel_MouseEnter);
            this.ShowPasswordLabel.MouseLeave += new System.EventHandler(this.ShowPasswordLabel_MouseLeave);
            // 
            // OldPasswordTextBox
            // 
            this.OldPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.OldPasswordTextBox.Location = new System.Drawing.Point(3, 33);
            this.OldPasswordTextBox.Name = "OldPasswordTextBox";
            this.OldPasswordTextBox.ReadOnly = true;
            this.OldPasswordTextBox.Size = new System.Drawing.Size(214, 20);
            this.OldPasswordTextBox.TabIndex = 7;
            this.OldPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordTextBox.Location = new System.Drawing.Point(315, 33);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(214, 20);
            this.PasswordTextBox.TabIndex = 8;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // MainLabel
            // 
            this.MainLabel.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.MainLabel, 2);
            this.MainLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLabel.Location = new System.Drawing.Point(3, 0);
            this.MainLabel.Name = "MainLabel";
            this.MainLabel.Size = new System.Drawing.Size(306, 29);
            this.MainLabel.TabIndex = 9;
            this.MainLabel.Text = "Update password locally";
            this.MainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultLabel.Location = new System.Drawing.Point(535, 0);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(149, 29);
            this.ResultLabel.TabIndex = 10;
            this.ResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SqlServerPasswordUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 88);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "SqlServerPasswordUpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sql Server local password update";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.SqlServerPasswordUpdateForm_Activated);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.ComboBox ServerComboBox;
        private System.Windows.Forms.Button AcceptBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label ShowOldPasswordLabel;
        private System.Windows.Forms.Label ShowPasswordLabel;
        private System.Windows.Forms.TextBox OldPasswordTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label MainLabel;
        private System.Windows.Forms.Label ResultLabel;
    }
}