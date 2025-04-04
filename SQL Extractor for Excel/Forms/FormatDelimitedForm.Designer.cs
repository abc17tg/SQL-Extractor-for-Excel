namespace SQL_Extractor_for_Excel.Forms
{
    partial class FormatDelimitedForm
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
            this.endTextBox = new System.Windows.Forms.TextBox();
            this.endLabel = new System.Windows.Forms.Label();
            this.startTextBox = new System.Windows.Forms.TextBox();
            this.startLabel = new System.Windows.Forms.Label();
            this.toSqlFormatButton = new System.Windows.Forms.Button();
            this.appendTextBox = new System.Windows.Forms.TextBox();
            this.delimiterTextBox = new System.Windows.Forms.TextBox();
            this.prependTextBox = new System.Windows.Forms.TextBox();
            this.prependLabel = new System.Windows.Forms.Label();
            this.delimiterLabel = new System.Windows.Forms.Label();
            this.appendLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.exampleLabel = new System.Windows.Forms.Label();
            this.addBracketsButton = new System.Windows.Forms.Button();
            this.divideToLabel = new System.Windows.Forms.Label();
            this.dividerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.uniqueValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dividerNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0008F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0008F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0008F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.9988F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.9988F));
            this.tableLayoutPanel.Controls.Add(this.endTextBox, 4, 2);
            this.tableLayoutPanel.Controls.Add(this.endLabel, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.startTextBox, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.startLabel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.toSqlFormatButton, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.appendTextBox, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.delimiterTextBox, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.prependTextBox, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.prependLabel, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.delimiterLabel, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.appendLabel, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.countLabel, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.okButton, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.cancelButton, 4, 4);
            this.tableLayoutPanel.Controls.Add(this.exampleLabel, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.addBracketsButton, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.divideToLabel, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.dividerNumericUpDown, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.uniqueValuesCheckBox, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(534, 161);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // endTextBox
            // 
            this.endTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endTextBox.Location = new System.Drawing.Point(427, 67);
            this.endTextBox.Name = "endTextBox";
            this.endTextBox.Size = new System.Drawing.Size(104, 22);
            this.endTextBox.TabIndex = 14;
            this.endTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // endLabel
            // 
            this.endLabel.AutoSize = true;
            this.endLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endLabel.Location = new System.Drawing.Point(427, 32);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(104, 32);
            this.endLabel.TabIndex = 15;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startTextBox
            // 
            this.startTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTextBox.Location = new System.Drawing.Point(3, 67);
            this.startTextBox.Name = "startTextBox";
            this.startTextBox.Size = new System.Drawing.Size(100, 22);
            this.startTextBox.TabIndex = 12;
            this.startTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.startTextBox.TextChanged += new System.EventHandler(this.startTextBox_TextChanged);
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startLabel.Location = new System.Drawing.Point(3, 32);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(100, 32);
            this.startLabel.TabIndex = 13;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toSqlFormatButton
            // 
            this.toSqlFormatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toSqlFormatButton.Location = new System.Drawing.Point(321, 3);
            this.toSqlFormatButton.Name = "toSqlFormatButton";
            this.toSqlFormatButton.Size = new System.Drawing.Size(100, 26);
            this.toSqlFormatButton.TabIndex = 2;
            this.toSqlFormatButton.Text = "To SQL (\' \', \' \', ...)";
            this.toSqlFormatButton.UseVisualStyleBackColor = true;
            this.toSqlFormatButton.Click += new System.EventHandler(this.toSqlFormatButton_Click);
            // 
            // appendTextBox
            // 
            this.appendTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appendTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appendTextBox.Location = new System.Drawing.Point(321, 67);
            this.appendTextBox.Name = "appendTextBox";
            this.appendTextBox.Size = new System.Drawing.Size(100, 22);
            this.appendTextBox.TabIndex = 6;
            this.appendTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // delimiterTextBox
            // 
            this.delimiterTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delimiterTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delimiterTextBox.Location = new System.Drawing.Point(215, 67);
            this.delimiterTextBox.Name = "delimiterTextBox";
            this.delimiterTextBox.Size = new System.Drawing.Size(100, 22);
            this.delimiterTextBox.TabIndex = 5;
            this.delimiterTextBox.Text = ";";
            this.delimiterTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // prependTextBox
            // 
            this.prependTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prependTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prependTextBox.Location = new System.Drawing.Point(109, 67);
            this.prependTextBox.Name = "prependTextBox";
            this.prependTextBox.Size = new System.Drawing.Size(100, 22);
            this.prependTextBox.TabIndex = 4;
            this.prependTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.prependTextBox.TextChanged += new System.EventHandler(this.prependTextBox_TextChanged);
            // 
            // prependLabel
            // 
            this.prependLabel.AutoSize = true;
            this.prependLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prependLabel.Location = new System.Drawing.Point(109, 32);
            this.prependLabel.Name = "prependLabel";
            this.prependLabel.Size = new System.Drawing.Size(100, 32);
            this.prependLabel.TabIndex = 7;
            this.prependLabel.Text = "Prepend";
            this.prependLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delimiterLabel
            // 
            this.delimiterLabel.AutoSize = true;
            this.delimiterLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delimiterLabel.Location = new System.Drawing.Point(215, 32);
            this.delimiterLabel.Name = "delimiterLabel";
            this.delimiterLabel.Size = new System.Drawing.Size(100, 32);
            this.delimiterLabel.TabIndex = 8;
            this.delimiterLabel.Text = "Delimiter";
            this.delimiterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // appendLabel
            // 
            this.appendLabel.AutoSize = true;
            this.appendLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appendLabel.Location = new System.Drawing.Point(321, 32);
            this.appendLabel.Name = "appendLabel";
            this.appendLabel.Size = new System.Drawing.Size(100, 32);
            this.appendLabel.TabIndex = 9;
            this.appendLabel.Text = "Append";
            this.appendLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countLabel.Location = new System.Drawing.Point(3, 96);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(100, 32);
            this.countLabel.TabIndex = 10;
            this.countLabel.Text = "Count";
            this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // okButton
            // 
            this.tableLayoutPanel.SetColumnSpan(this.okButton, 4);
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(3, 131);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(418, 27);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(427, 131);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(104, 27);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // exampleLabel
            // 
            this.exampleLabel.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.exampleLabel, 3);
            this.exampleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exampleLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exampleLabel.Location = new System.Drawing.Point(109, 96);
            this.exampleLabel.Name = "exampleLabel";
            this.exampleLabel.Size = new System.Drawing.Size(312, 32);
            this.exampleLabel.TabIndex = 11;
            this.exampleLabel.Text = "example";
            this.exampleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addBracketsButton
            // 
            this.addBracketsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addBracketsButton.Location = new System.Drawing.Point(427, 3);
            this.addBracketsButton.Name = "addBracketsButton";
            this.addBracketsButton.Size = new System.Drawing.Size(104, 26);
            this.addBracketsButton.TabIndex = 16;
            this.addBracketsButton.Text = "Add ( ) at ends";
            this.addBracketsButton.UseVisualStyleBackColor = true;
            this.addBracketsButton.Click += new System.EventHandler(this.addBracketsButton_Click);
            // 
            // divideToLabel
            // 
            this.divideToLabel.AutoSize = true;
            this.divideToLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divideToLabel.Location = new System.Drawing.Point(109, 0);
            this.divideToLabel.Name = "divideToLabel";
            this.divideToLabel.Size = new System.Drawing.Size(100, 32);
            this.divideToLabel.TabIndex = 17;
            this.divideToLabel.Text = "Divide to:";
            this.divideToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dividerNumericUpDown
            // 
            this.dividerNumericUpDown.AutoSize = true;
            this.dividerNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dividerNumericUpDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.dividerNumericUpDown.Location = new System.Drawing.Point(215, 3);
            this.dividerNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dividerNumericUpDown.Name = "dividerNumericUpDown";
            this.dividerNumericUpDown.Size = new System.Drawing.Size(41, 20);
            this.dividerNumericUpDown.TabIndex = 18;
            this.dividerNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dividerNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dividerNumericUpDown.ValueChanged += new System.EventHandler(this.dividerNumericUpDown_ValueChanged);
            // 
            // uniqueValuesCheckBox
            // 
            this.uniqueValuesCheckBox.AutoSize = true;
            this.uniqueValuesCheckBox.Checked = true;
            this.uniqueValuesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uniqueValuesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uniqueValuesCheckBox.Location = new System.Drawing.Point(3, 3);
            this.uniqueValuesCheckBox.Name = "uniqueValuesCheckBox";
            this.uniqueValuesCheckBox.Size = new System.Drawing.Size(100, 26);
            this.uniqueValuesCheckBox.TabIndex = 19;
            this.uniqueValuesCheckBox.Text = "Unique values";
            this.uniqueValuesCheckBox.UseVisualStyleBackColor = true;
            this.uniqueValuesCheckBox.CheckedChanged += new System.EventHandler(this.uniqueValuesCheckBox_CheckedChanged);
            // 
            // FormatDelimitedForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(534, 161);
            this.Controls.Add(this.tableLayoutPanel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 200);
            this.MinimumSize = new System.Drawing.Size(550, 200);
            this.Name = "FormatDelimitedForm";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Format text delimited";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.FormatDelimitedForm_Activated);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dividerNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button toSqlFormatButton;
        private System.Windows.Forms.TextBox prependTextBox;
        private System.Windows.Forms.TextBox delimiterTextBox;
        private System.Windows.Forms.TextBox appendTextBox;
        private System.Windows.Forms.Label prependLabel;
        private System.Windows.Forms.Label delimiterLabel;
        private System.Windows.Forms.Label appendLabel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label exampleLabel;
        private System.Windows.Forms.TextBox endTextBox;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.TextBox startTextBox;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Button addBracketsButton;
        private System.Windows.Forms.Label divideToLabel;
        private System.Windows.Forms.NumericUpDown dividerNumericUpDown;
        private System.Windows.Forms.CheckBox uniqueValuesCheckBox;
    }
}