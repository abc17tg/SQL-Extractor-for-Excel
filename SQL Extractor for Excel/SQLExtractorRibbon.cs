using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SQL_Extractor_for_Excel.Scripts;
using SQL_Extractor_for_Excel.Properties;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Ribbon;
using System.Diagnostics;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new SQLExtractorRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace SQL_Extractor_for_Excel
{
    [ComVisible(true)]
    public class SQLExtractorRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI m_ribbon;

        public SQLExtractorRibbon()
        {
        }

        public void sqlEditorDataFolderButtonClick(Office.IRibbonControl control)
        {
            Process.Start("explorer.exe", FileManager.SqlQueriesPath);
        }

        public void OnLaunchSqlExtractorButtonClick(Office.IRibbonControl control)
        {
            // Handle button click event
            //System.Windows.Forms.MessageBox.Show("SQL Extractor for Excel");
            SqlEditorForm form = new SqlEditorForm(Globals.ThisAddIn.Application);
            form.Show();
        }

        public Bitmap launchSqlExtractorButton_GetImage(Office.IRibbonControl control)
        {
            try
            {
                string filePath = Path.Combine(FileManager.ResourcesPath, "SQL_Editor_Logo.png");
                // Load the PNG file into an Image object
                using (Image image = Image.FromFile(filePath))
                {
                    // Convert the Image object to a Bitmap
                    return new Bitmap(image);
                }
            }
            catch
            {
                return new Bitmap(100, 100);
            }
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("SQL_Extractor_for_Excel.SQLExtractorRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.m_ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
