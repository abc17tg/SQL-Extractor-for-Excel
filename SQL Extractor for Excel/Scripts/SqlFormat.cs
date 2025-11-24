using System;
/*using Jint;*/
public class SqlFormatResult
{
    public string OriginalQuery { get; }
    public string FormattedQuery { get; }
    public bool Success { get; }
    public string Errors { get; }
    public SqlFormatResult(string originalQuery, string formattedQuery, bool success, string errors)
    {
        OriginalQuery = originalQuery;
        FormattedQuery = formattedQuery;
        Success = success;
        Errors = errors;
    }
}
public static class SqlFormat
{
    public enum Dialect
    {
        Sql,
        N1ql,
        Db2,
        PlSql
    }
/*    private const string JsCode = @"// Paste the full content of https://raw.githubusercontent.com/kufii/sql-formatter-plus/main/dist/sql-formatter.min.js here.
// You can download it or copy from the GitHub page and replace this string with the minified JS code.
// Ensure it's a single string, escaping any necessary characters if needed.
";
    private static readonly Engine _engine;*/
    static SqlFormat()
    {
/*        _engine = new Engine();
        _engine.Execute(JsCode);*/
    }
/*    public static SqlFormatResult Format(string query, Dialect dialect = Dialect.Sql)
    {
        string lang = dialect.ToString().ToLowerInvariant();
        string original = query;
        string formatted = null;
        string errors = null;
        bool success = true;
        try
        {
            var options = new
            {
                language = lang,
                indent = "\t",
                uppercase = true,
                // You can add linesBetweenQueries = 2 if needed for multiple queries
            };
            var result = _engine.Invoke("sqlFormatter.format", query, options);
            formatted = result.AsString();
        }
        catch (Exception ex)
        {
            success = false;
            errors = ex.Message;
            formatted = original; // or null, depending on preference
        }
        return new SqlFormatResult(original, formatted, success, errors);
    }*/
}
// Usage example:
// var result = SqlFormatter.Format("select distinct primary_transaction_id, opportunity_id from omega_loaded where source_feeder_code in ('O') or sales_org_country != 'US' and upper(site_c_name) like '%ERICSSON%'", SqlFormatter.Dialect.Sql);
// if (result.Success) Console.WriteLine(result.FormattedQuery);