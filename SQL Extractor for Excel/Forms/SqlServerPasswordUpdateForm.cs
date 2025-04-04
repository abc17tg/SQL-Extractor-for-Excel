using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Scripts;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class SqlServerPasswordUpdateForm : Form
    {
        private Dictionary<string, SqlConn> m_connDic;
        private SqlConn m_tempSqlConn;
        private System.Windows.Forms.Timer clearLabelTimer;

        public SqlServerManager.ServerType ServerType;

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 ToggleTopMostMenuItem = 1000;
        public const Int32 CenterFormMenuItem = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        private void ToggleTopMost()
        {
            this.TopMost = !this.TopMost;
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
                    default:
                        break;
                }
            }
            base.WndProc(ref msg);
        }

        public SqlServerPasswordUpdateForm(SqlServerManager.ServerType serverType, string selectedServerKeyForDic)
        {
            InitializeComponent();
            ServerType = serverType; // Set ServerType from parameter
            RefreshServerComboBox();
            // Select server from selectedServerKeyForDic and populate old password
            if (!string.IsNullOrWhiteSpace(selectedServerKeyForDic) && m_connDic.ContainsKey(selectedServerKeyForDic))
            {
                ServerComboBox.SelectedItem = selectedServerKeyForDic;
                // m_tempSqlConn is set in ServerComboBox_SelectedIndexChanged, triggered by setting SelectedItem
            }

            // Initialize timer for clearing ResultLabel after 3 seconds
            clearLabelTimer = new System.Windows.Forms.Timer();
            clearLabelTimer.Interval = 3000; // 3000 ms = 3 seconds
            clearLabelTimer.Tick += ClearLabelTimer_Tick;
        }

        private void SqlServerPasswordUpdateForm_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 5, MF_BYPOSITION, ToggleTopMostMenuItem, "Pin/Unpin this window");
            InsertMenu(MenuHandle, 6, MF_BYPOSITION, CenterFormMenuItem, "Center window");
        }

        private void RefreshServerComboBox()
        {
            switch (ServerType)
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

            ServerComboBox.Items.Clear();
            try
            {
                ServerComboBox.Items.AddRange(m_connDic.Keys.ToArray());
            }
            catch (Exception ex)
            {
                // Log exception in a real application
                Close(); // Close without changes as per original intent
            }
        }

        // Toggle password visibility on hover over ShowOldPasswordLabel
        private void ShowOldPasswordLabel_MouseEnter(object sender, EventArgs e)
        {
            OldPasswordTextBox.UseSystemPasswordChar = false;
        }

        private void ShowOldPasswordLabel_MouseLeave(object sender, EventArgs e)
        {
            OldPasswordTextBox.UseSystemPasswordChar = true;
        }

        // Toggle password visibility on hover over ShowPasswordLabel
        private void ShowPasswordLabel_MouseEnter(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = false;
        }

        private void ShowPasswordLabel_MouseLeave(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = true;
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            if (m_tempSqlConn != null && m_tempSqlConn.Test())
            {
                ResultLabel.Text = "Connection success!";
            }
            else
            {
                ResultLabel.Text = "Connection failed!";
            }
            clearLabelTimer.Start(); // Start timer to clear label
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            bool result = false;
            switch (ServerType)
            {
                case SqlServerManager.ServerType.SqlServer:
                    result = SqlServerManager.TestConnectionSqlServer(m_tempSqlConn.ConnectionString());
                    break;
                case SqlServerManager.ServerType.Oracle:
                    result = SqlServerManager.TestConnectionOracle(m_tempSqlConn.ConnectionString());
                    break;
                default:
                    break;
            }
            if (result)
            {
                DialogResult = DialogResult.OK;
                SqlConn.SaveSqlConn(m_tempSqlConn, ServerComboBox.SelectedItem.ToString());
                Close();
            }
            else
            {
                ResultLabel.Text = "Failed to connect";
                clearLabelTimer.Start(); // Start timer only if connection fails (form closes on success)
            }
        }

        private void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServerComboBox.SelectedItem == null)
            {
                ResultLabel.Text = "Please select a server";
                OldPasswordTextBox.Text = string.Empty;
                PasswordTextBox.Text = string.Empty;
                TestBtn.Enabled = false;
                AcceptBtn.Enabled = false;
            }
            else
            {
                string selectedKey = ServerComboBox.SelectedItem.ToString();
                if (m_connDic.ContainsKey(selectedKey))
                {
                    m_tempSqlConn = m_connDic[selectedKey];
                    OldPasswordTextBox.Text = AesEncryption.DecryptString(m_tempSqlConn.Password); // Populate old password
                    TestBtn.Enabled = true;
                    AcceptBtn.Enabled = true;
                }
                else
                {
                    ResultLabel.Text = "Selected server not found";
                    TestBtn.Enabled = false;
                    AcceptBtn.Enabled = false;
                }
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            string newPassword = PasswordTextBox.Text;
            bool isValid = IsPasswordValid(newPassword);

            if (isValid)
            {
                // Update the connection object with the new password
                m_tempSqlConn = new SqlConn(m_tempSqlConn.Name, m_tempSqlConn.UserName, newPassword,
                    m_tempSqlConn.Link, m_tempSqlConn.Port, m_tempSqlConn.Type, true);

                TestBtn.Enabled = true;    // Enable the Test button
                AcceptBtn.Enabled = true;  // Enable the Accept button
                ResultLabel.Text = string.Empty;  // Clear any error message
            }
            else
            {
                TestBtn.Enabled = false;   // Disable the Test button
                AcceptBtn.Enabled = false; // Disable the Accept button
                ResultLabel.Text = "Invalid password";  // Show error message
                clearLabelTimer.Start();   // Start a timer to clear the message after 3 seconds
            }
        }

        private bool IsPasswordValid(string password)
        {
            // Check if the password is null, empty, or whitespace
            return !string.IsNullOrWhiteSpace(password);
        }

        private void ClearLabelTimer_Tick(object sender, EventArgs e)
        {
            ResultLabel.Text = string.Empty;
            clearLabelTimer.Stop(); // Stop timer after clearing
        }

        private void SqlServerPasswordUpdateForm_Activated(object sender, EventArgs e)
        {
            Utils.EnsureWindowIsVisible(this);
        }
    }
}
