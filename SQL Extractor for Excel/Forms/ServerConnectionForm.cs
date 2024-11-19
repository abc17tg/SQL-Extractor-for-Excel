using System;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Scripts;

namespace SQL_Extractor_for_Excel
{
    public partial class ServerConnectionForm : Form
    {
        private SqlConn SqlConn => new SqlConn(nameTextBox.Enabled ? nameTextBox.Text : null, usernameTextBox.Enabled ? usernameTextBox.Text : null,
            passwordTextBox.Enabled ? passwordTextBox.Text : null, linkTextBox.Enabled ? linkTextBox.Text : null,
            portTextBox.Enabled ? portTextBox.Text : null, (SqlServerManager.ServerType)serverTypeComboBox.SelectedIndex, true);

        public ServerConnectionForm()
        {
            InitializeComponent();
            passwordTextBox.UseSystemPasswordChar = true;
            serverTypeComboBox.Items.Insert((int)SqlServerManager.ServerType.SqlServer, SqlServerManager.ServerType.SqlServer.ToString());
            serverTypeComboBox.Items.Insert((int)SqlServerManager.ServerType.Oracle, SqlServerManager.ServerType.Oracle.ToString());
            serverTypeComboBox_SelectedIndexChanged(null, null);
        }

        private void testBtn_Click(object sender, EventArgs e)
        {
            bool result;
            switch ((SqlServerManager.ServerType)serverTypeComboBox.SelectedIndex)
            {
                case SqlServerManager.ServerType.SqlServer:
                    result = SqlServerManager.TestConnectionSqlServer(SqlConn.ConnectionString());
                    break;
                case SqlServerManager.ServerType.Oracle:
                    result = SqlServerManager.TestConnectionOracle(SqlConn.ConnectionString());
                    break;
                default:
                    MessageBox.Show("Failed!");
                    return;
            }

            if (result)
                MessageBox.Show("Success!");

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            bool result = false;
            switch ((SqlServerManager.ServerType)serverTypeComboBox.SelectedIndex)
            {
                case SqlServerManager.ServerType.SqlServer:
                    result = SqlServerManager.TestConnectionSqlServer(SqlConn.ConnectionString());
                    break;
                case SqlServerManager.ServerType.Oracle:
                    result = SqlServerManager.TestConnectionOracle(SqlConn.ConnectionString());
                    break;
                default:
                    break;
            }
            if (result)
            {
                DialogResult = DialogResult.OK;
                SqlConn.SaveSqlConn(SqlConn);
                Close();
            }
            else
                MessageBox.Show("Failed to connect");
        }

        private void serverTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((SqlServerManager.ServerType)serverTypeComboBox.SelectedIndex)
            {
                case SqlServerManager.ServerType.SqlServer:
                    nameTextBox.Enabled = false;
                    usernameTextBox.Enabled = true;
                    passwordTextBox.Enabled = true;
                    linkTextBox.Enabled = true;
                    portTextBox.Enabled = true;
                    testBtn.Enabled = true;
                    okBtn.Enabled = true;
                    break;
                case SqlServerManager.ServerType.Oracle:
                    nameTextBox.Enabled = true;
                    usernameTextBox.Enabled = true;
                    passwordTextBox.Enabled = true;
                    linkTextBox.Enabled = true;
                    portTextBox.Enabled = true;
                    testBtn.Enabled = true;
                    okBtn.Enabled = true;
                    break;
                default:
                    nameTextBox.Enabled = false;
                    usernameTextBox.Enabled = false;
                    passwordTextBox.Enabled = false;
                    linkTextBox.Enabled = false;
                    portTextBox.Enabled = false;
                    testBtn.Enabled = false;
                    okBtn.Enabled = false;
                    return;
            }
        }
    }
}
