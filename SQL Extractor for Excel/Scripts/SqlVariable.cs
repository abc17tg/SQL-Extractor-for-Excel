using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
