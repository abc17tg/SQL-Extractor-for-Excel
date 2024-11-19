using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class FileDropForm : Form
    {
        public string FilePath = string.Empty;
        private List<string> m_filter;

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 ToggleTopMostMenuItem = 1000;
        public const Int32 CenterFormMenuItem = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public FileDropForm(List<string> filter = null)
        {
            InitializeComponent();
            m_filter = filter.Select(p => p.Trim('.', ' ')).ToList();
            Activate();
            pathTextBox.Focus();
            Load += (o, s) => pathTextBox.Focus();
        }

        ~FileDropForm()
        {
            FilePath = pathTextBox.Text;
            Load -= (o, s) => pathTextBox.Focus();
        }


        private void FileDropForm_Load(object sender, EventArgs e)
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

        private void dropFileField_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void FileDropped(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
                pathTextBox.Text = files[0];
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            FilePath = pathTextBox.Text;
            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            FilePath = pathTextBox.Text;
            Close();
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            string filePath = pathTextBox.Text.Trim(' ', '\"');

            if (string.IsNullOrEmpty(filePath))
            {
                dropFileLabel.Visible = true;
                droppedFileIcon.Visible = false;
                okBtn.Enabled = false;
                pathTextBox.BackColor = SystemColors.Window;
            }
            else if (File.Exists(filePath) && (m_filter == null || m_filter.Contains(Path.GetExtension(filePath).TrimStart('.').ToLower())))
            {
                dropFileLabel.Visible = false;
                droppedFileIcon.Visible = true;
                Icon icon = Icon.ExtractAssociatedIcon(filePath);
                Image image = icon.ToBitmap();
                Bitmap newImage = new Bitmap(droppedFileIcon.Width, droppedFileIcon.Height);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    g.DrawImage(image, 0, 0, droppedFileIcon.Width, droppedFileIcon.Height);
                }
                droppedFileIcon.Image = newImage;
                okBtn.Enabled = true;
                Activate();
                okBtn.Focus();
                pathTextBox.BackColor = Color.PaleGreen;
            }
            else
            {
                dropFileLabel.Visible = true;
                droppedFileIcon.Visible = false;
                okBtn.Enabled = false;
                pathTextBox.BackColor = Color.LightPink;
            }
            pathTextBox.Text = filePath;
        }

        private void dropFileLabel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = m_filter != null ? "Files|" + string.Join(";", m_filter.Select(p => "*." + p)) : "Files|*.*";
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pathTextBox.Text = openFileDialog.FileName;
            }
        }

    }
}
