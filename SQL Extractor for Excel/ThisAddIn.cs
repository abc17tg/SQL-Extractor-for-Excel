using System;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using SQL_Extractor_for_Excel.Scripts;
using System.IO;
using System.Windows.Forms;

namespace SQL_Extractor_for_Excel
{
    public partial class ThisAddIn
    {
        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new SQLExtractorRibbon();
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Excel.Application app = Globals.ThisAddIn.Application;
            if (app.Visible)
                OpenBackupFiles(app);
        }

        private void OpenBackupFiles(Excel.Application app)
        {
            if (!Directory.Exists(FileManager.SqlEditorBackupPath))
                return; // Directory doesn't exist; nothing to open.

            try
            {
                string[] backupFiles = Directory.GetFiles(FileManager.SqlEditorBackupPath, "*.json");
                if (backupFiles == null || backupFiles.Length < 1)
                    return;

                var result = MessageBox.Show($"There {(backupFiles.Length == 1 ? "was" : $"were {backupFiles.Length}")} SQL Extractor editor{(backupFiles.Length == 1 ? string.Empty : "s")} from last session.\nDo you want to open them or not? Click:\n-No to delete backup{(backupFiles.Length == 1 ? string.Empty : "s")}\n-Yes to open all\n-Cancel to ignore", "Queries from last session", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        foreach (string backupFile in backupFiles)
                        {
                            SqlEditorForm form = new SqlEditorForm(app, backupFile);
                            form.Show();
                            File.Delete(backupFile);
                        }
                        break;
                    case DialogResult.None:
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.No:
                        foreach (string backupFile in backupFiles)
                        {
                            File.Delete(backupFile);
                        }
                        break;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with backup files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
