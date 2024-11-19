namespace SQL_Extractor_for_Excel.Forms
{
    partial class FileDropForm
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
            this.dropFileField = new System.Windows.Forms.Panel();
            this.dropFileLabel = new System.Windows.Forms.Label();
            this.droppedFileIcon = new System.Windows.Forms.PictureBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.dropFileField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.droppedFileIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // dropFileField
            // 
            this.dropFileField.AllowDrop = true;
            this.dropFileField.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.dropFileField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dropFileField.Controls.Add(this.dropFileLabel);
            this.dropFileField.Controls.Add(this.droppedFileIcon);
            this.dropFileField.Location = new System.Drawing.Point(7, 8);
            this.dropFileField.Name = "dropFileField";
            this.dropFileField.Size = new System.Drawing.Size(378, 100);
            this.dropFileField.TabIndex = 4;
            this.dropFileField.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDropped);
            this.dropFileField.DragEnter += new System.Windows.Forms.DragEventHandler(this.dropFileField_DragEnter);
            // 
            // dropFileLabel
            // 
            this.dropFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dropFileLabel.AutoSize = true;
            this.dropFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropFileLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dropFileLabel.Location = new System.Drawing.Point(123, 37);
            this.dropFileLabel.Name = "dropFileLabel";
            this.dropFileLabel.Size = new System.Drawing.Size(137, 24);
            this.dropFileLabel.TabIndex = 1;
            this.dropFileLabel.Text = "Drop file here";
            this.dropFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dropFileLabel.Click += new System.EventHandler(this.dropFileLabel_Click);
            // 
            // droppedFileIcon
            // 
            this.droppedFileIcon.BackColor = System.Drawing.Color.Transparent;
            this.droppedFileIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.droppedFileIcon.Location = new System.Drawing.Point(142, 0);
            this.droppedFileIcon.Name = "droppedFileIcon";
            this.droppedFileIcon.Size = new System.Drawing.Size(100, 100);
            this.droppedFileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.droppedFileIcon.TabIndex = 0;
            this.droppedFileIcon.TabStop = false;
            this.droppedFileIcon.Visible = false;
            // 
            // okBtn
            // 
            this.okBtn.Enabled = false;
            this.okBtn.Location = new System.Drawing.Point(289, 140);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 29);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(187, 140);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 29);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.AllowDrop = true;
            this.pathTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathTextBox.Location = new System.Drawing.Point(8, 114);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(377, 21);
            this.pathTextBox.TabIndex = 0;
            this.pathTextBox.WordWrap = false;
            this.pathTextBox.TextChanged += new System.EventHandler(this.pathTextBox_TextChanged);
            // 
            // FileDropForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(394, 176);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.dropFileField);
            this.MaximumSize = new System.Drawing.Size(410, 215);
            this.MinimumSize = new System.Drawing.Size(410, 215);
            this.Name = "FileDropForm";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.Text = "Get file";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FileDropForm_Load);
            this.dropFileField.ResumeLayout(false);
            this.dropFileField.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.droppedFileIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel dropFileField;
        private System.Windows.Forms.PictureBox droppedFileIcon;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Label dropFileLabel;
    }
}