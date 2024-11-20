using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
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

                foreach (string backupFile in backupFiles)
                {
                    SqlEditorForm form = new SqlEditorForm(app, backupFile);
                    form.Show();
                    File.Delete(backupFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening backup files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
