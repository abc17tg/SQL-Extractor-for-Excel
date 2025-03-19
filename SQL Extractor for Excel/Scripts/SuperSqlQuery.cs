using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class SuperSqlQuery
    {
        public static readonly string VariableIndicator = ":::";

        public readonly string BaseQuery;
        private Dictionary<string, List<string>> m_variablesD;

        public Dictionary<string, List<string>> VariablesD => m_variablesD;

        public SuperSqlQuery(string query, Dictionary<string, List<string>> allVariablesD)
        {
            BaseQuery = query;
            m_variablesD = allVariablesD.Where(p => BaseQuery.Contains($"{VariableIndicator}{p.Key}")).ToDictionary(p => p.Key, p => p.Value);
        }

        public static bool IsSuperQuery(string query, Dictionary<string, List<string>> allVariablesD) => allVariablesD.Keys.Any(p => query.Contains($"{VariableIndicator}{p}"));

        public static int SimulatedCount(string query, Dictionary<string, List<string>> allVariablesD)
        {
            if (string.IsNullOrWhiteSpace(query))
                return -1;

            int count = 1;
            foreach (var v in allVariablesD)
            {
                if (query.Contains($"{VariableIndicator}{v.Key}"))
                    count *= v.Value.Count;
            }

            return count;
        }

        public static List<string> GetVariablesFromString(string text)
        {
            // Create a regex pattern to match words preceded by the specified prefix
            string pattern = $@"(?<!\S){VariableIndicator}(\w+)(?!\S)";

            var matches = Regex.Matches(text, pattern);

            // Use a HashSet to ensure all words are distinct
            var distinctWords = new HashSet<string>();

            foreach (Match match in matches)
                distinctWords.Add(match.Groups[1].Value);

            return distinctWords.ToList();
        }

        public int SimulatedCount()
        {
            int count = 1;
            foreach (var v in m_variablesD)
                count *= v.Value.Count;

            return count;
        }

        public List<string> GetAllQueries()
        {
            // SimulatedCount should be 0 when variable did not got populated
            if (string.IsNullOrWhiteSpace(BaseQuery) || SimulatedCount() == 0)
                return null;

            List<string> queries = new List<string> { BaseQuery };
            foreach (var vD in m_variablesD)
            {
                if (BaseQuery.Contains($"{VariableIndicator}{vD.Key}"))
                {
                    List<string> newQueries = new List<string>();
                    foreach (string q in queries)
                    {
                        foreach (var s in vD.Value)
                            newQueries.Add(BaseQuery.Replace($"{VariableIndicator}{vD.Key}", s));
                    }
                    queries = newQueries;
                }
            }
            return queries;
        }

    }
}
