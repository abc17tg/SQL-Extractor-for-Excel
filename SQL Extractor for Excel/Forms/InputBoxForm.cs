using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class InputBoxForm : Form
    {
        public string Result => this.DialogResult == DialogResult.OK ? inputTextBox.Text : null;
        private bool m_multiline;

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 ToggleTopMostMenuItem = 1000;
        public const Int32 CenterFormMenuItem = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);
        public InputBoxForm(string title = null, string labelText = null, string defaultText = null, bool multiline = false)
        {
            InitializeComponent();
            label.Text = labelText ?? string.Empty;
            this.Text = title ?? this.Text;
            inputTextBox.Text = defaultText ?? string.Empty;
            m_multiline = multiline;
            inputTextBox.AcceptsReturn = m_multiline;
        }

        private void InputBoxForm_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 5, MF_BYPOSITION, ToggleTopMostMenuItem, "Pin/Unpin this window");
            InsertMenu(MenuHandle, 6, MF_BYPOSITION, CenterFormMenuItem, "Center window");
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case ToggleTopMostMenuItem:
                        ToggleTopMost();
                        return;
                    case CenterFormMenuItem:
                        Utils.MoveFormToCenter(this);
                        return;
                    default:
                        break;
                }
            }
            base.WndProc(ref msg);
        }

        private void ToggleTopMost()
        {
            this.TopMost = !this.TopMost;
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (m_multiline || !inputTextBox.Text.Contains(Environment.NewLine))
                return;

            inputTextBox.Text = inputTextBox.Text.Replace(Environment.NewLine, string.Empty);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InputBoxForm_Activated(object sender, EventArgs e)
        {

        }
    }
}
