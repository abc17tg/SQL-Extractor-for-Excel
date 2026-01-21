using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Controls;
using SQL_Extractor_for_Excel.Scripts;
using static ScintillaNET.Style;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class DataTableTabbedForm : Form
    {
        // Global Window Management (Static)
        public static List<DataTableTabbedForm> OpenForms = new List<DataTableTabbedForm>();
        public static DataTableTabbedForm GlobalLastActiveForm = null;

        public static DataTableTabbedForm GetActiveOrNew()
        {
            // If the last active one is still open and valid, use it
            if (GlobalLastActiveForm != null && !GlobalLastActiveForm.IsDisposed)
                return GlobalLastActiveForm;

            // Otherwise, check if ANY form is open
            var anyOpen = OpenForms.Where(f => !f.IsDisposed).LastOrDefault();
            if (anyOpen != null)
                return anyOpen;

            // If absolutely nothing is open, create new
            var newForm = new DataTableTabbedForm();
            return newForm;
        }
        // Win32 API
        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 CenterFormMenuItem = 1001;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        // Dragging Logic State
        private bool m_isDragging = false;
        private TabPage m_draggedTab;

        // Add this variable
        private Random m_random = new Random();
        // Constructors
        public DataTableTabbedForm()
        {
            InitializeComponent();

            // Enable custom tab coloring
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += tabControl_DrawItem;

            tabControl.MouseWheel += tabControl_MouseWheel;

            RegisterForm();
            UpdateFormTitle();
        }

        public DataTableTabbedForm(DataTable dataTable, string query, Excel.Application app, string name = null, string displayQuery = null)
        {
            InitializeComponent();
            RegisterForm();
            AddNewTab(name, (ctrl) => ctrl.InitializeFromDataTable(dataTable, query, app, name, displayQuery));
        }

        public DataTableTabbedForm(SqlResult sqlResult, string query, Excel.Application app, string name = null, string displayQuery = null)
        {
            InitializeComponent();
            RegisterForm();
            AddNewTab(name, (ctrl) => ctrl.InitializeFromSqlResult(sqlResult, query, app, name, displayQuery));
        }

        public DataTableTabbedForm(SqlServerManager sqlServerManager, string query, Excel.Application app, SqlConn sqlConn, string name = null, int batchSize = 500, int timeout = 0)
        {
            InitializeComponent();
            RegisterForm();
            AddNewTab(name, (ctrl) => ctrl.InitializeLive(sqlServerManager, query, app, sqlConn, name, batchSize, timeout));
        }

        private void RegisterForm()
        {
            if (!OpenForms.Contains(this)) OpenForms.Add(this);
            GlobalLastActiveForm = this;

            // Hook into closing to cleanup static list
            this.FormClosed += (s, e) =>
            {
                OpenForms.Remove(this);
                if (GlobalLastActiveForm == this)
                {
                    GlobalLastActiveForm = OpenForms.LastOrDefault();
                }
            };
        }
        // Tab Management Logic
        private void AddNewTab(string title, Action<DataTableControl> initializer)
        {
            TabPage newPage = new TabPage(title ?? "DataTable");

            // Store unique color in Tag, keep content background white
            newPage.Tag = GetUniquePastelColor();
            newPage.BackColor = SystemColors.Control;

            DataTableControl content = new DataTableControl();
            content.Dock = DockStyle.Fill;
            initializer(content);
            newPage.Controls.Add(content);
            tabControl.TabPages.Add(newPage);
            tabControl.SelectedTab = newPage;

            UpdateFormTitle();
        }

        public void AddExistingTab(TabPage tab)
        {
            // If the tab doesn't have a color in Tag yet, give it one
            if (tab.Tag == null || !(tab.Tag is Color))
            {
                tab.Tag = GetUniquePastelColor();
                tab.BackColor = SystemColors.Control;
            }

            tabControl.TabPages.Add(tab);
            tabControl.SelectedTab = tab;

            UpdateFormTitle();
        }

        // Public methods for external callers
        public void AddLiveTab(SqlServerManager sqlServerManager, string query, Excel.Application app, SqlConn sqlConn, string name = null, int batchSize = 500, int timeout = 0)
        {
            AddNewTab(name, (ctrl) => ctrl.InitializeLive(sqlServerManager, query, app, sqlConn, name, batchSize, timeout));
        }

        public void AddSqlResultTab(SqlResult sqlResult, string query, Excel.Application app, string name = null, string displayQuery = null)
        {
            AddNewTab(name, (ctrl) => ctrl.InitializeFromSqlResult(sqlResult, query, app, name, displayQuery));
        }

        public void AddDataTableTab(DataTable dataTable, string query, Excel.Application app, string name = null, string displayQuery = null)
        {
            AddNewTab(name, (ctrl) => ctrl.InitializeFromDataTable(dataTable, query, app, name, displayQuery));
        }

        // Drag & Drop Logic
        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                Rectangle r = tabControl.GetTabRect(i);
                if (r.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Middle || (e.Button == MouseButtons.Right && ModifierKeys == Keys.Control))
                    {
                        RemoveTabInternal(tabControl.TabPages[i]);
                        return;
                    }

                    if (e.Button == MouseButtons.Left)
                    {
                        m_isDragging = true;
                        m_draggedTab = tabControl.TabPages[i];
                    }
                    return;
                }
            }
        }

        private void tabControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                Rectangle r = tabControl.GetTabRect(i);
                if (r.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        RemoveTabInternal(tabControl.TabPages[i]);
                        return;
                    }
                }
            }
        }

        private void tabControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    RemoveTabInternal(tabControl.SelectedTab);
                    return;
                }

                if (e.KeyCode == Keys.Q)
                {
                    if (tabControl.SelectedTab != null)
                        tabControl.SelectedTab.FindAllChildrenByType<DataTableControl>().FirstOrDefault()?.ToogleQueryView();
                }
            }
        }

        private void tabControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isDragging && m_draggedTab != null)
            {
                // Optional: Add drag threshold check here
                if (e.Button != MouseButtons.Left)
                {
                    m_isDragging = false;
                    m_draggedTab = null;
                }
            }
        }

        private void tabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_isDragging && m_draggedTab != null)
            {
                Point screenPoint = tabControl.PointToScreen(e.Location);

                // 1. Check if dropped inside THIS window (Reordering - simplified, currently does nothing)
                if (this.DesktopBounds.Contains(screenPoint))
                {
                    // Logic for reordering tabs could go here
                    m_isDragging = false;
                    m_draggedTab = null;
                    return;
                }

                // 2. Check if dropped inside ANOTHER existing window
                foreach (var otherForm in OpenForms)
                {
                    if (otherForm != this && !otherForm.IsDisposed && otherForm.Visible)
                    {
                        if (otherForm.DesktopBounds.Contains(screenPoint))
                        {
                            MoveTabToForm(m_draggedTab, otherForm);
                            m_isDragging = false;
                            m_draggedTab = null;
                            return;
                        }
                    }
                }

                // 3. Dropped in empty space -> Create NEW window
                CreateNewWindowWithTab(m_draggedTab);

                m_isDragging = false;
                m_draggedTab = null;
            }
        }

        private void tabControl_MouseWheel(object sender, MouseEventArgs e)
        {
            // Only scroll if hovering over the tab header area
            if (tabControl.TabPages.Count == 0) return;

            if (tabControl.SelectedTab != null && tabControl.SelectedTab.Controls.Count > 0)
            {
                // strict "header only" scrolling, uncomment next line:
                if (e.Location.Y > tabControl.GetTabRect(0).Height) 
                    return;
            }

            int currentIndex = tabControl.SelectedIndex;

            // Scroll Up -> Go Left
            if (e.Delta > 0)
            {
                if (currentIndex > 0)
                    tabControl.SelectedIndex = currentIndex - 1;
            }
            // Scroll Down -> Go Right
            else
            {
                if (currentIndex < tabControl.TabPages.Count - 1)
                    tabControl.SelectedIndex = currentIndex + 1;
            }
        }

        public void CloseActiveTab()
        {
            RemoveTabInternal(tabControl.SelectedTab);
        }

        private void RemoveTabInternal(TabPage tabToRemove)
        {
            if (tabToRemove == null) return;

            int originalIndex = tabControl.TabPages.IndexOf(tabToRemove);

            if (tabToRemove.Controls.Count > 0 && tabToRemove.Controls[0] is DataTableControl dtc)
            {
                dtc.Dispose();
            }

            tabControl.TabPages.Remove(tabToRemove);

            // Switch to tab on the left
            if (tabControl.TabPages.Count > 0)
            {
                int newIndex = originalIndex - 1;
                if (newIndex < 0) newIndex = 0; // Fallback to first if we closed the first one

                // Ensure index is valid
                if (newIndex < tabControl.TabPages.Count)
                    tabControl.SelectedIndex = newIndex;
            }

            // Update Count immediately after removal
            UpdateFormTitle();

            if (tabControl.TabPages.Count == 0) this.Close();
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            // Get color from Tag (fallback to Control color if null)
            Color tabHeaderColor = (tabPage.Tag is Color c) ? c : SystemColors.Control;

            // 1. Draw the Header Background
            using (Brush brush = new SolidBrush(tabHeaderColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // 2. Active Tab Highlight (Blue Frame)
            if (e.Index == tabControl.SelectedIndex)
            {
                // Draw a thick blue border inside the tab
                using (Pen pen = new Pen(Color.DodgerBlue, 3))
                {
                    Rectangle borderRect = tabRect;
                    borderRect.Inflate(-2, -2); // Shrink slightly to fit inside
                    e.Graphics.DrawRectangle(pen, borderRect);
                }
            }

            // 3. Draw the Text
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabControl.Font, tabRect, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void MoveTabToForm(TabPage tab, DataTableTabbedForm targetForm)
        {
            // Remove from current
            this.tabControl.TabPages.Remove(tab);

            // Add to target
            targetForm.AddExistingTab(tab);
            targetForm.Activate(); // Bring target to front

            // Close current if empty
            if (this.tabControl.TabPages.Count == 0) this.Close();
        }

        private void CreateNewWindowWithTab(TabPage tab)
        {
            tabControl.TabPages.Remove(tab);
            UpdateFormTitle(); // Update title of OLD form

            if (tabControl.TabPages.Count == 0) this.Close();

            DataTableTabbedForm newForm = new DataTableTabbedForm();
            newForm.AddExistingTab(tab);

            newForm.StartPosition = FormStartPosition.Manual;
            newForm.Location = Cursor.Position;

            newForm.Show();
            newForm.Activate();
        }

        private void UpdateFormTitle()
        {
            // Updates form title to "DataTable [X]"
            this.Text = $"DataTable [{tabControl.TabPages.Count}]";
        }

        private Color GetUniquePastelColor()
        {
            // Generates a random light color (RGB values between 200-255)
            return Color.FromArgb(255, m_random.Next(200, 256), m_random.Next(200, 256), m_random.Next(200, 256));
        }
        // Standard Form Events
        private void DataTableTabbedForm_Load(object sender, EventArgs e)
        {
            IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
            InsertMenu(MenuHandle, 6, MF_BYPOSITION, CenterFormMenuItem, "Center window");
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SYSCOMMAND)
            {
                if (msg.WParam.ToInt32() == CenterFormMenuItem)
                {
                    Utils.MoveFormToCenter(this);
                    return;
                }
            }
            base.WndProc(ref msg);
        }

        private void DataTableTabbedForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                Utils.EnsureWindowIsVisible(this);
        }

        private void DataTableTabbedForm_Activated(object sender, EventArgs e)
        {
            // Update the global tracker when this window is clicked/focused
            GlobalLastActiveForm = this;
        }
    }
}