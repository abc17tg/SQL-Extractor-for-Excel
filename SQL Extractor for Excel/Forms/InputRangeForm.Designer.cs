namespace SQL_Extractor_for_Excel.Forms
{
    partial class InputRangeForm
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
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.rangeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(320, 38);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(96, 27);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "Accept";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(218, 38);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(96, 27);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // rangeTextBox
            // 
            this.rangeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rangeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rangeTextBox.ForeColor = System.Drawing.Color.ForestGreen;
            this.rangeTextBox.Location = new System.Drawing.Point(6, 7);
            this.rangeTextBox.Margin = new System.Windows.Forms.Padding(10);
            this.rangeTextBox.MaximumSize = new System.Drawing.Size(410, 27);
            this.rangeTextBox.MinimumSize = new System.Drawing.Size(410, 27);
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.ReadOnly = true;
            this.rangeTextBox.Size = new System.Drawing.Size(410, 22);
            this.rangeTextBox.TabIndex = 2;
            this.rangeTextBox.Text = "Select range and accept";
            this.rangeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rangeTextBox.WordWrap = false;
            // 
            // InputRangeForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(424, 71);
            this.Controls.Add(this.rangeTextBox);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(440, 110);
            this.MinimumSize = new System.Drawing.Size(440, 110);
            this.Name = "InputRangeForm";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.Text = "Range input";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.InputRangeForm_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputRangeForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.TextBox rangeTextBox;
    }
}