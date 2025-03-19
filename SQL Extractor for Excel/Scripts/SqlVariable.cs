using System.Collections.Generic;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class SqlVariable
    {
        public string Name;
        public List<string> Values;
        public string LeftOuter = "";
        public string RightOuter = "";
        public string LeftInner = "";
        public string RightInner = "";
        public string Delimiter = ", ";
    }
}
