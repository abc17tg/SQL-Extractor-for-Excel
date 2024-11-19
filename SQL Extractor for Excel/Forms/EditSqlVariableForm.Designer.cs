namespace SQL_Extractor_for_Excel.Forms
{
    partial class EditSqlVariableForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.countLabel = new System.Windows.Forms.Label();
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
            this.addBracketsButton = new System.Windows.Forms.Button();
            this.divideToLabel = new System.Windows.Forms.Label();
            this.dividerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.uniqueValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.renameButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.variableNameLabel = new System.Windows.Forms.Label();
            this.valuesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.fetchButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dividerNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.countLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.renameButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.okButton, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.variableNameLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.valuesRichTextBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.fetchButton, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.addButton, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(577, 305);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countLabel.Location = new System.Drawing.Point(3, 0);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(109, 30);
            this.countLabel.TabIndex = 10;
            this.countLabel.Text = "Count";
            this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel, 5);
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
            this.tableLayoutPanel.Controls.Add(this.addBracketsButton, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.divideToLabel, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.dividerNumericUpDown, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.uniqueValuesCheckBox, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel, 3);
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(571, 84);
            this.tableLayoutPanel.TabIndex = 8;
            // 
            // endTextBox
            // 
            this.endTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endTextBox.Location = new System.Drawing.Point(459, 59);
            this.endTextBox.Name = "endTextBox";
            this.endTextBox.Size = new System.Drawing.Size(109, 22);
            this.endTextBox.TabIndex = 14;
            this.endTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // endLabel
            // 
            this.endLabel.AutoSize = true;
            this.endLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endLabel.Location = new System.Drawing.Point(459, 28);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(109, 28);
            this.endLabel.TabIndex = 15;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startTextBox
            // 
            this.startTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTextBox.Location = new System.Drawing.Point(3, 59);
            this.startTextBox.Name = "startTextBox";
            this.startTextBox.Size = new System.Drawing.Size(108, 22);
            this.startTextBox.TabIndex = 12;
            this.startTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startLabel.Location = new System.Drawing.Point(3, 28);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(108, 28);
            this.startLabel.TabIndex = 13;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toSqlFormatButton
            // 
            this.toSqlFormatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toSqlFormatButton.Location = new System.Drawing.Point(345, 3);
            this.toSqlFormatButton.Name = "toSqlFormatButton";
            this.toSqlFormatButton.Size = new System.Drawing.Size(108, 22);
            this.toSqlFormatButton.TabIndex = 2;
            this.toSqlFormatButton.Text = "To SQL (\' \', \' \', ...)";
            this.toSqlFormatButton.UseVisualStyleBackColor = true;
            // 
            // appendTextBox
            // 
            this.appendTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appendTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appendTextBox.Location = new System.Drawing.Point(345, 59);
            this.appendTextBox.Name = "appendTextBox";
            this.appendTextBox.Size = new System.Drawing.Size(108, 22);
            this.appendTextBox.TabIndex = 6;
            this.appendTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // delimiterTextBox
            // 
            this.delimiterTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delimiterTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delimiterTextBox.Location = new System.Drawing.Point(231, 59);
            this.delimiterTextBox.Name = "delimiterTextBox";
            this.delimiterTextBox.Size = new System.Drawing.Size(108, 22);
            this.delimiterTextBox.TabIndex = 5;
            this.delimiterTextBox.Text = ", ";
            this.delimiterTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // prependTextBox
            // 
            this.prependTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prependTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prependTextBox.Location = new System.Drawing.Point(117, 59);
            this.prependTextBox.Name = "prependTextBox";
            this.prependTextBox.Size = new System.Drawing.Size(108, 22);
            this.prependTextBox.TabIndex = 4;
            this.prependTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // prependLabel
            // 
            this.prependLabel.AutoSize = true;
            this.prependLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prependLabel.Location = new System.Drawing.Point(117, 28);
            this.prependLabel.Name = "prependLabel";
            this.prependLabel.Size = new System.Drawing.Size(108, 28);
            this.prependLabel.TabIndex = 7;
            this.prependLabel.Text = "Prepend";
            this.prependLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delimiterLabel
            // 
            this.delimiterLabel.AutoSize = true;
            this.delimiterLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.delimiterLabel.Location = new System.Drawing.Point(231, 28);
            this.delimiterLabel.Name = "delimiterLabel";
            this.delimiterLabel.Size = new System.Drawing.Size(108, 28);
            this.delimiterLabel.TabIndex = 8;
            this.delimiterLabel.Text = "Delimiter";
            this.delimiterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // appendLabel
            // 
            this.appendLabel.AutoSize = true;
            this.appendLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appendLabel.Location = new System.Drawing.Point(345, 28);
            this.appendLabel.Name = "appendLabel";
            this.appendLabel.Size = new System.Drawing.Size(108, 28);
            this.appendLabel.TabIndex = 9;
            this.appendLabel.Text = "Append";
            this.appendLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addBracketsButton
            // 
            this.addBracketsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addBracketsButton.Location = new System.Drawing.Point(459, 3);
            this.addBracketsButton.Name = "addBracketsButton";
            this.addBracketsButton.Size = new System.Drawing.Size(109, 22);
            this.addBracketsButton.TabIndex = 16;
            this.addBracketsButton.Text = "Add ( ) at the ends";
            this.addBracketsButton.UseVisualStyleBackColor = true;
            // 
            // divideToLabel
            // 
            this.divideToLabel.AutoSize = true;
            this.divideToLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divideToLabel.Location = new System.Drawing.Point(117, 0);
            this.divideToLabel.Name = "divideToLabel";
            this.divideToLabel.Size = new System.Drawing.Size(108, 28);
            this.divideToLabel.TabIndex = 17;
            this.divideToLabel.Text = "Divide to:";
            this.divideToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dividerNumericUpDown
            // 
            this.dividerNumericUpDown.AutoSize = true;
            this.dividerNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dividerNumericUpDown.Dock = System.Windows.Forms.DockStyle.Left;
            this.dividerNumericUpDown.Location = new System.Drawing.Point(231, 3);
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
            // 
            // uniqueValuesCheckBox
            // 
            this.uniqueValuesCheckBox.AutoSize = true;
            this.uniqueValuesCheckBox.Checked = true;
            this.uniqueValuesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uniqueValuesCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uniqueValuesCheckBox.Location = new System.Drawing.Point(3, 3);
            this.uniqueValuesCheckBox.Name = "uniqueValuesCheckBox";
            this.uniqueValuesCheckBox.Size = new System.Drawing.Size(108, 22);
            this.uniqueValuesCheckBox.TabIndex = 19;
            this.uniqueValuesCheckBox.Text = "Unique values";
            this.uniqueValuesCheckBox.UseVisualStyleBackColor = true;
            // 
            // renameButton
            // 
            this.renameButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renameButton.Location = new System.Drawing.Point(463, 3);
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size(111, 24);
            this.renameButton.TabIndex = 4;
            this.renameButton.Text = "Rename";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 2);
            this.textBox1.Location = new System.Drawing.Point(233, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(224, 20);
            this.textBox1.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(348, 277);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(109, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okButton.Location = new System.Drawing.Point(463, 277);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(111, 25);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // variableNameLabel
            // 
            this.variableNameLabel.AutoSize = true;
            this.variableNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableNameLabel.Location = new System.Drawing.Point(118, 0);
            this.variableNameLabel.Name = "variableNameLabel";
            this.variableNameLabel.Size = new System.Drawing.Size(109, 30);
            this.variableNameLabel.TabIndex = 2;
            this.variableNameLabel.Text = "Variable name:";
            this.variableNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // valuesRichTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.valuesRichTextBox, 5);
            this.valuesRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valuesRichTextBox.Location = new System.Drawing.Point(3, 123);
            this.valuesRichTextBox.Name = "valuesRichTextBox";
            this.tableLayoutPanel1.SetRowSpan(this.valuesRichTextBox, 2);
            this.valuesRichTextBox.Size = new System.Drawing.Size(571, 148);
            this.valuesRichTextBox.TabIndex = 6;
            this.valuesRichTextBox.Text = "";
            // 
            // fetchButton
            // 
            this.fetchButton.AccessibleName = "fetchButton";
            this.fetchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fetchButton.Location = new System.Drawing.Point(3, 277);
            this.fetchButton.Name = "fetchButton";
            this.fetchButton.Size = new System.Drawing.Size(109, 25);
            this.fetchButton.TabIndex = 11;
            this.fetchButton.Text = "Fetch";
            this.fetchButton.UseVisualStyleBackColor = true;
            this.fetchButton.Click += new System.EventHandler(this.fetchButton_Click);
            // 
            // addButton
            // 
            this.addButton.AccessibleName = "fetchButton";
            this.addButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addButton.Location = new System.Drawing.Point(118, 277);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(109, 25);
            this.addButton.TabIndex = 12;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // EditSqlVariableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 305);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditSqlVariableForm";
            this.Text = "EditSqlVariableForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dividerNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label variableNameLabel;
        private System.Windows.Forms.Button renameButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox startTextBox;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Button toSqlFormatButton;
        private System.Windows.Forms.TextBox appendTextBox;
        private System.Windows.Forms.TextBox delimiterTextBox;
        private System.Windows.Forms.TextBox prependTextBox;
        private System.Windows.Forms.Label prependLabel;
        private System.Windows.Forms.Label delimiterLabel;
        private System.Windows.Forms.Label appendLabel;
        private System.Windows.Forms.Label divideToLabel;
        private System.Windows.Forms.NumericUpDown dividerNumericUpDown;
        private System.Windows.Forms.CheckBox uniqueValuesCheckBox;
        private System.Windows.Forms.TextBox endTextBox;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.Button addBracketsButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RichTextBox valuesRichTextBox;
        private System.Windows.Forms.Button fetchButton;
        private System.Windows.Forms.Button addButton;
    }
}