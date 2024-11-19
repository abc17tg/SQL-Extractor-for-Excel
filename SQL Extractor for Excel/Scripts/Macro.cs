using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelVB = Microsoft.Vbe.Interop;

namespace SQL_Extractor_for_Excel.Scripts
{
    internal class Macro
    {
        public string Name;
        public string ModuleName;
        public string Code;
        public string FirstCodeLine => Code.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
        public string FullName => $"{ModuleName}.{Name}";

        public Macro() { }

        public Macro(string name, string moduleName, string code)
        {
            Name = name;
            ModuleName = moduleName;
            Code = code;
        }

        public static bool Exists(string macroName, string moduleName, Excel.Workbook wb)
        {
            try
            {

                var component = wb.VBProject.VBComponents.Item(moduleName);
                if (component != null && component.Type == ExcelVB.vbext_ComponentType.vbext_ct_StdModule)
                    for (int i = 1; i < component.CodeModule.CountOfLines; i++)
                        if (macroName == component.CodeModule.ProcOfLine[i, out ExcelVB.vbext_ProcKind procKind])
                            return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<Macro> GetMacrosFromLines(ExcelVB.VBComponent component, int startLine = -1, int endLine = -1)
        {
            if (component == null)
                return null;

            List<Macro> macrosList = new List<Macro>();
            if (component.Type == ExcelVB.vbext_ComponentType.vbext_ct_StdModule)
            {
                startLine = startLine > 0 ? startLine : 1;
                endLine = endLine > 0 ? endLine : component.CodeModule.CountOfLines - 1;

                for (int i = startLine; i <= endLine; i++)
                {
                    string macroName = component.CodeModule.ProcOfLine[i, out ExcelVB.vbext_ProcKind procKind];
                    if (!string.IsNullOrWhiteSpace(macroName) && procKind == ExcelVB.vbext_ProcKind.vbext_pk_Proc)
                    {
                        macrosList.Add(new Macro
                        {
                            Name = macroName,
                            ModuleName = component.Name,
                            Code = component.CodeModule.Lines[component.CodeModule.ProcStartLine[macroName, procKind], component.CodeModule.ProcCountLines[macroName, procKind]]
                        });
                        i += component.CodeModule.ProcCountLines[macroName, procKind] - 1;
                    }
                }
            }
            else
                return null;

            return macrosList.DistinctBy(p => new { p.Name, p.ModuleName }).ToList();
        }

        public static string GetMacroNameForButton(string btnId, Excel.Workbook wb)
        {
            try
            {
                var xe = XElement.Load(Path.Combine(FileManager.PropertiesFilesPath, "ButtonSubroutineMapping.xml"));
                var mapping = xe.Elements("Mapping").FirstOrDefault(m => (string)m.Element("ButtonID") == btnId);
                string macroModuleName = mapping?.Element("Subroutine")?.Value;
                /*                string[] temp = macroModuleName.Split('.');
                                string macroName = temp[1];
                                string module = temp[0];

                                if (Exists(macroName, module, wb))*/
                return macroModuleName;
                /*                else
                                    return null;*/
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
