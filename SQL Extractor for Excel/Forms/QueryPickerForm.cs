using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Scripts;

namespace SQL_Extractor_for_Excel.Forms
{
    public partial class QueryPickerForm : Form
    {
        public enum PasteType
        {
            PasteIntoSelection,
            PasteBelow,
            Replace,
            OpenInNewWindow,
            Cancel
        }

        private readonly string m_directoryPath;
        private string m_selectedText;
        private PasteType m_pasteType;

        public string SelectedText => m_selectedText;
        public PasteType SelectedPasteType => m_pasteType;

        private SqlServerManager.ServerType m_serverType;

        public SqlServerManager.ServerType ServerType => m_serverType;

        public QueryPickerForm(string directoryPath, SqlServerManager.ServerType serverType)
        {
            m_directoryPath = directoryPath;
            m_serverType = serverType;
            m_pasteType = PasteType.Cancel; // Default value
            InitializeComponent();
            SetupForm();

        }

        private void SetupForm()
        {
            // Configure Scintilla
            UtilsScintilla.SetupSqlEditorReadOnly(queryViewEditorScintilla);
            queryViewEditorScintilla.Styles.AsEnumerable().ToList().ForEach(p => { p.Size = 7; p.Bold = true; });
            queryViewEditorScintilla.ContextMenuStrip = CreateScintillaContextMenu();

            // Populate TreeView
            PopulateTreeView();

            // Setup search
            searchTextBox.KeyDown += SearchTextBox_KeyDown;
        }

        private ContextMenuStrip CreateScintillaContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            var copy = new ToolStripMenuItem("Copy", null, (s, e) =>
            {
                queryViewEditorScintilla.Copy();
            });

            var toggleWrapMode = new ToolStripMenuItem("Toggle text wrap mode", null, (s, e) =>
            {
                if (queryViewEditorScintilla.WrapMode == ScintillaNET.WrapMode.None) 
                    queryViewEditorScintilla.WrapMode = ScintillaNET.WrapMode.Word;
                else
                    queryViewEditorScintilla.WrapMode = ScintillaNET.WrapMode.None;
            });

            var pasteIntoSelection = new ToolStripMenuItem("Paste into selection", null, (s, e) =>
            {
                m_selectedText = queryViewEditorScintilla.SelectedText;
                m_pasteType = PasteType.PasteIntoSelection;
                this.DialogResult = DialogResult.OK;
                this.Close();
            });

            var pasteBelow = new ToolStripMenuItem("Paste below", null, (s, e) =>
            {
                m_selectedText = queryViewEditorScintilla.SelectedText;
                m_pasteType = PasteType.PasteBelow;
                this.DialogResult = DialogResult.OK;
                this.Close();
            });

            contextMenu.Items.AddRange(new ToolStripItem[] { copy, pasteIntoSelection, pasteBelow, toggleWrapMode });
            return contextMenu;
        }

        private void PopulateTreeView()
        {
            queriesTreeView.Nodes.Clear();

            if (!Directory.Exists(m_directoryPath))
                return;

            var rootDirectories = Directory.GetDirectories(m_directoryPath);
            foreach (var dir in rootDirectories)
            {
                var dirInfo = new DirectoryInfo(dir);
                var node = queriesTreeView.Nodes.Add(dirInfo.Name);
                node.Tag = dirInfo.FullName;

                // Only expand the directory matching server type
                if (dirInfo.Name.Equals(m_serverType.ToString(), StringComparison.Ordinal))
                {
                    node.Expand();
                    PopulateSubNodes(node);
                }

                // Add dummy node to show expand arrow
                if (Directory.GetDirectories(dir).Length > 0 || Directory.GetFiles(dir, "*.sql").Length > 0)
                {
                    node.Nodes.Add("Loading...");
                }
            }

            queriesTreeView.BeforeExpand += QueriesTreeView_BeforeExpand;
            queriesTreeView.AfterSelect += QueriesTreeView_AfterSelect;
            queriesTreeView.NodeMouseClick += QueriesTreeView_NodeMouseClick;
        }

        private void QueriesTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "Loading...")
            {
                e.Node.Nodes.Clear();
                PopulateSubNodes(e.Node);
            }
        }

        private void PopulateSubNodes(TreeNode parentNode)
        {
            string path = parentNode.Tag.ToString();

            // Add subdirectories
            foreach (var dir in Directory.GetDirectories(path))
            {
                var dirInfo = new DirectoryInfo(dir);
                var node = parentNode.Nodes.Add(dirInfo.Name);
                node.Tag = dirInfo.FullName;

                // Add dummy node if there are subdirectories or SQL files
                if (Directory.GetDirectories(dir).Length > 0 || Directory.GetFiles(dir, "*.sql").Length > 0)
                {
                    node.Nodes.Add("Loading...");
                }
            }

            // Add SQL files
            foreach (var file in Directory.GetFiles(path, "*.sql"))
            {
                var fileInfo = new FileInfo(file);
                var node = parentNode.Nodes.Add(fileInfo.Name);
                node.Tag = fileInfo.FullName;
            }
        }

        private void QueriesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;

            string path = e.Node.Tag.ToString();
            if (File.Exists(path) && path.EndsWith(".sql"))
            {
                try
                {
                    queryViewEditorScintilla.ReadOnly = false;
                    queryViewEditorScintilla.Text = File.ReadAllText(path);
                    queryViewEditorScintilla.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void QueriesTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node.Tag != null)
            {
                string path = e.Node.Tag.ToString();
                if (File.Exists(path) && path.EndsWith(".sql"))
                {
                    try
                    {
                        m_selectedText = File.ReadAllText(path);
                        m_pasteType = PasteType.OpenInNewWindow;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading file: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchPattern = searchTextBox.Text;
                queriesTreeView.Nodes.Clear();

                if (string.IsNullOrWhiteSpace(searchPattern))
                {
                    PopulateTreeView();
                    // Find and expand the server node
                    foreach (TreeNode node in queriesTreeView.Nodes)
                    {
                        if (node.Text.Equals(m_serverType.ToString(), StringComparison.Ordinal))
                        {
                            node.Expand();
                            break;
                        }
                    }
                    return;
                }

                try
                {
                    var regex = new Regex(searchPattern, RegexOptions.IgnoreCase);
                    SearchDirectory(m_directoryPath, regex);
                    // Expand all nodes after search to show results
                    queriesTreeView.ExpandAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Invalid search pattern: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PopulateTreeView();
                    // Restore expansion after error
                    foreach (TreeNode node in queriesTreeView.Nodes)
                    {
                        if (node.Text.Equals(m_serverType.ToString(), StringComparison.Ordinal))
                        {
                            node.Expand();
                            break;
                        }
                    }
                }
            }
        }

        private void SearchDirectory(string path, Regex regex)
        {
            var rootDirectories = Directory.GetDirectories(path);
            foreach (var dir in rootDirectories)
            {
                var dirInfo = new DirectoryInfo(dir);
                var node = queriesTreeView.Nodes.Add(dirInfo.Name);
                node.Tag = dirInfo.FullName;

                // Always search subdirectories, but mark if they have matches
                bool hasMatches = SearchSubDirectory(dir, regex, node);

                // Remove the node if it has no matching children and doesn't match itself
                if (!hasMatches && !regex.IsMatch(dirInfo.Name))
                {
                    queriesTreeView.Nodes.Remove(node);
                }
            }
        }

        private bool SearchSubDirectory(string path, Regex regex, TreeNode parentNode)
        {
            bool hasMatches = false;

            // Add subdirectories
            foreach (var dir in Directory.GetDirectories(path))
            {
                var dirInfo = new DirectoryInfo(dir);
                var node = parentNode.Nodes.Add(dirInfo.Name);
                node.Tag = dirInfo.FullName;

                bool subHasMatches = SearchSubDirectory(dir, regex, node);
                if (subHasMatches || regex.IsMatch(dirInfo.Name))
                {
                    hasMatches = true;
                }
                else
                {
                    parentNode.Nodes.Remove(node);
                }
            }

            // Add SQL files
            foreach (var file in Directory.GetFiles(path, "*.sql"))
            {
                var fileInfo = new FileInfo(file);
                if (regex.IsMatch(fileInfo.Name))
                {
                    var node = parentNode.Nodes.Add(fileInfo.Name);
                    node.Tag = fileInfo.FullName;
                    hasMatches = true;
                }
            }

            return hasMatches;
        }

        private void pasteQueryButton_Click(object sender, EventArgs e)
        {
            m_selectedText = queryViewEditorScintilla.Text;
            m_pasteType = PasteType.PasteBelow;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void replaceQueryButton_Click(object sender, EventArgs e)
        {
            m_selectedText = queryViewEditorScintilla.Text;
            m_pasteType = PasteType.Replace;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            m_pasteType = PasteType.Cancel;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
