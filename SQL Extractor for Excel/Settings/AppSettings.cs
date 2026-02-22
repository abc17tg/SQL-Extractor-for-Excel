using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SQL_Extractor_for_Excel.Settings
{
    /// <summary>
    /// Represents application settings with versioning support for backward compatibility.
    /// When adding new settings in future versions, add them with default values and
    /// the JSON importer will use defaults for missing properties from older files.
    /// </summary>
    public class AppSettings
    {
        #region Versioning

        /// <summary>
        /// Settings schema version. Increment when adding new settings or changing structure.
        /// Used for migration and compatibility checks.
        /// </summary>
        [JsonPropertyName("settingsVersion")]
        public int SettingsVersion { get; set; } = CurrentSettingsVersion;

        public const int CurrentSettingsVersion = 1;

        #endregion

        #region Window Behavior Settings

        /// <summary>
        /// Gets or sets whether the SQL Editor window should stay on top of other windows.
        /// Default: true (pinned)
        /// </summary>
        [JsonPropertyName("keepSqlEditorOnTop")]
        public bool KeepSqlEditorOnTop { get; set; } = DefaultKeepSqlEditorOnTop;

        public const bool DefaultKeepSqlEditorOnTop = true;

        /// <summary>
        /// Gets or sets whether text wrapping is enabled in the SQL editor.
        /// Default: true
        /// </summary>
        [JsonPropertyName("textWrapEnabled")]
        public bool TextWrapEnabled { get; set; } = DefaultTextWrapEnabled;

        public const bool DefaultTextWrapEnabled = true;

        #endregion

        #region Export Settings

        /// <summary>
        /// Gets or sets the default export option for query results.
        /// Options: "DataTableForm", "NewWorksheet", "SelectedCell"
        /// Default: "DataTableForm"
        /// </summary>
        [JsonPropertyName("defaultExportOption")]
        public string DefaultExportOption { get; set; } = DefaultDefaultExportOption;

        public const string DefaultDefaultExportOption = "DataTableForm";

        /// <summary>
        /// List of available export options for the dropdown.
        /// </summary>
        public static readonly string[] AvailableExportOptions = { "DataTableForm", "NewWorksheet", "SelectedCell" };

        #endregion

        #region Startup Settings

        /// <summary>
        /// Gets or sets the SQL query text that is loaded when the editor launches.
        /// Default: empty string (no default query)
        /// </summary>
        [JsonPropertyName("startSqlQueryOnLaunch")]
        public string StartSqlQueryOnLaunch { get; set; } = DefaultStartSqlQueryOnLaunch;

        public const string DefaultStartSqlQueryOnLaunch = "";

        #endregion

        #region SQL Keywords Settings (Per Database Type)

        /// <summary>
        /// Gets or sets the MS SQL Server keywords for syntax highlighting.
        /// Keywords are space-separated.
        /// </summary>
        [JsonPropertyName("msSqlKeywords")]
        public string MsSqlKeywords { get; set; } = DefaultMsSqlKeywords;

        public const string DefaultMsSqlKeywords =
            "SELECT FROM WHERE GROUP BY ORDER BY JOIN INNER LEFT RIGHT OUTER ON AND OR NOT IN IS NULL " +
            "AS DISTINCT TOP PERCENT WITH TIES INTO BY HAVING UNION ALL EXCEPT INTERSECT " +
            "INSERT UPDATE DELETE VALUES SET CREATE ALTER DROP TABLE INDEX VIEW PROCEDURE FUNCTION " +
            "TRIGGER DATABASE SCHEMA GRANT REVOKE DENY EXEC EXECUTE DECLARE SET @ " +
            "IF ELSE BEGIN END WHILE RETURN PRINT RAISERROR THROW TRY CATCH TRANSACTION COMMIT ROLLBACK " +
            "PRIMARY KEY FOREIGN REFERENCES UNIQUE CHECK DEFAULT CONSTRAINT IDENTITY " +
            "VARCHAR NVARCHAR INT BIGINT SMALLINT TINYINT DECIMAL NUMERIC FLOAT REAL " +
            "DATETIME DATE TIME DATETIME2 SMALLDATETIME BIT CHAR NCHAR NTEXT TEXT IMAGE VARBINARY " +
            "XML JSON CASE WHEN THEN ELSE END COALESCE NULLIF ISNULL CONVERT CAST " +
            "EXISTS BETWEEN LIKE ESCAPE PIVOT UNPIVOT OVER PARTITION BY ROW_NUMBER RANK DENSE_RANK " +
            "LAG LEAD FIRST_VALUE LAST_VALUE CUBE ROLLUP GROUPING GROUPING_ID " +
            "MERGE OUTPUT INSERTED DELETED DUAL GO USE";

        /// <summary>
        /// Gets or sets the Oracle keywords for syntax highlighting.
        /// Keywords are space-separated.
        /// </summary>
        [JsonPropertyName("oracleKeywords")]
        public string OracleKeywords { get; set; } = DefaultOracleKeywords;

        public const string DefaultOracleKeywords =
            "SELECT FROM WHERE GROUP BY ORDER BY JOIN INNER LEFT RIGHT OUTER FULL ON AND OR NOT IN IS NULL " +
            "AS DISTINCT UNIQUE ALL INTO BY HAVING UNION INTERSECT MINUS " +
            "INSERT UPDATE DELETE VALUES SET CREATE ALTER DROP TABLE INDEX VIEW PROCEDURE FUNCTION " +
            "PACKAGE TRIGGER DATABASE SCHEMA GRANT REVOKE EXECUTE DECLARE BEGIN END " +
            "IF THEN ELSIF ELSE WHILE LOOP FOR RETURN EXIT WHEN EXCEPTION RAISE " +
            "PRIMARY KEY FOREIGN REFERENCES UNIQUE CHECK DEFAULT CONSTRAINT " +
            "VARCHAR2 NVARCHAR2 NUMBER INTEGER SMALLINT DECIMAL FLOAT DOUBLE PRECISION " +
            "DATE TIMESTAMP INTERVAL YEAR MONTH DAY TIMEZONE WITH LOCAL " +
            "BLOB CLOB NCLOB BFILE XMLTYPE JSON CASE WHEN THEN ELSE END COALESCE NULLIF NVL DECODE " +
            "EXISTS BETWEEN LIKE ESCAPE OVER PARTITION BY ROW_NUMBER RANK DENSE_RANK " +
            "LAG LEAD FIRST_VALUE LAST_VALUE LISTAGG WITHIN CUBE ROLLUP GROUPING " +
            "MERGE MATCHED USING DUAL CONNECT BY START WITH PRIOR LEVEL ROWNUM " +
            "SYSDATE SYSTIMESTAMP CURRENT_DATE CURRENT_TIMESTAMP USER DUAL";

        #endregion

        #region Syntax Highlighting Color Settings

        /// <summary>
        /// Gets or sets the default foreground color for text in the editor.
        /// </summary>
        [JsonPropertyName("defaultForegroundColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color DefaultForegroundColor { get; set; } = DefaultDefaultForegroundColor;

        public static readonly Color DefaultDefaultForegroundColor = Color.FromArgb(240, 240, 240); // Almost white (dark theme)

        /// <summary>
        /// Gets or sets the color for SQL keywords.
        /// </summary>
        [JsonPropertyName("keywordColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color KeywordColor { get; set; } = DefaultKeywordColor;

        public static readonly Color DefaultKeywordColor = Color.FromArgb(86, 156, 214); // Blue (VS Dark theme)

        /// <summary>
        /// Gets or sets the color for string literals.
        /// </summary>
        [JsonPropertyName("stringColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color StringColor { get; set; } = DefaultStringColor;

        public static readonly Color DefaultStringColor = Color.FromArgb(181, 220, 168); // Light green (VS Dark theme)

        /// <summary>
        /// Gets or sets the color for comments.
        /// </summary>
        [JsonPropertyName("commentColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color CommentColor { get; set; } = DefaultCommentColor;

        public static readonly Color DefaultCommentColor = Color.FromArgb(100, 100, 100); // Gray (VS Dark theme)

        /// <summary>
        /// Gets or sets the color for numeric literals.
        /// </summary>
        [JsonPropertyName("numberColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color NumberColor { get; set; } = DefaultNumberColor;

        public static readonly Color DefaultNumberColor = Color.FromArgb(214, 157, 133); // Orange (VS Dark theme)

        /// <summary>
        /// Gets or sets the background color for the editor.
        /// </summary>
        [JsonPropertyName("editorBackgroundColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color EditorBackgroundColor { get; set; } = DefaultEditorBackgroundColor;

        public static readonly Color DefaultEditorBackgroundColor = Color.FromArgb(30, 30, 30); // Dark background

        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep copy of the current settings instance.
        /// </summary>
        public AppSettings Clone()
        {
            return new AppSettings
            {
                SettingsVersion = this.SettingsVersion,
                KeepSqlEditorOnTop = this.KeepSqlEditorOnTop,
                TextWrapEnabled = this.TextWrapEnabled,
                DefaultExportOption = this.DefaultExportOption,
                StartSqlQueryOnLaunch = this.StartSqlQueryOnLaunch,
                MsSqlKeywords = this.MsSqlKeywords,
                OracleKeywords = this.OracleKeywords,
                DefaultForegroundColor = this.DefaultForegroundColor,
                KeywordColor = this.KeywordColor,
                StringColor = this.StringColor,
                CommentColor = this.CommentColor,
                NumberColor = this.NumberColor,
                EditorBackgroundColor = this.EditorBackgroundColor
            };
        }

        /// <summary>
        /// Applies default values for any properties that are not set or invalid.
        /// This is used when loading settings from older versions.
        /// </summary>
        public void ApplyDefaultsForMissingValues()
        {
            // Version check
            if (SettingsVersion < 1)
            {
                SettingsVersion = CurrentSettingsVersion;
            }

            // Window behavior
            KeepSqlEditorOnTop = KeepSqlEditorOnTop; // bool can't be null, keep as is

            // Export option validation
            if (string.IsNullOrWhiteSpace(DefaultExportOption) ||
                !Array.Exists(AvailableExportOptions, o => o.Equals(DefaultExportOption, StringComparison.OrdinalIgnoreCase)))
            {
                DefaultExportOption = DefaultDefaultExportOption;
            }

            // Keywords validation
            if (string.IsNullOrWhiteSpace(MsSqlKeywords))
            {
                MsSqlKeywords = DefaultMsSqlKeywords;
            }

            if (string.IsNullOrWhiteSpace(OracleKeywords))
            {
                OracleKeywords = DefaultOracleKeywords;
            }

            // Colors validation (check if color is empty/default)
            if (DefaultForegroundColor.IsEmpty || DefaultForegroundColor == Color.Empty)
            {
                DefaultForegroundColor = DefaultDefaultForegroundColor;
            }

            if (KeywordColor.IsEmpty || KeywordColor == Color.Empty)
            {
                KeywordColor = DefaultKeywordColor;
            }

            if (StringColor.IsEmpty || StringColor == Color.Empty)
            {
                StringColor = DefaultStringColor;
            }

            if (CommentColor.IsEmpty || CommentColor == Color.Empty)
            {
                CommentColor = DefaultCommentColor;
            }

            if (NumberColor.IsEmpty || NumberColor == Color.Empty)
            {
                NumberColor = DefaultNumberColor;
            }

            if (EditorBackgroundColor.IsEmpty || EditorBackgroundColor == Color.Empty)
            {
                EditorBackgroundColor = DefaultEditorBackgroundColor;
            }
        }

        /// <summary>
        /// Gets the keywords for the specified database type.
        /// </summary>
        /// <param name="dbType">The database type (SqlServer, Oracle)</param>
        /// <returns>Space-separated keywords string</returns>
        public string GetKeywordsForDbType(string dbType)
        {
            if (string.IsNullOrWhiteSpace(dbType))
                return MsSqlKeywords;

            switch (dbType.ToLowerInvariant())
            {
                case "sqlserver":
                    return MsSqlKeywords;
                case "oracle":
                    return OracleKeywords;
                default:
                    return MsSqlKeywords;
            }
        }

        #endregion
    }

    /// <summary>
    /// JSON converter for System.Drawing.Color that serializes as ARGB integer or named color.
    /// Handles backward compatibility when colors are missing from older settings files.
    /// </summary>
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                // Try to read as object with ARGB properties
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    using (var doc = JsonDocument.ParseValue(ref reader))
                    {
                        var root = doc.RootElement;

                        if (root.TryGetProperty("a", out var aProp) &&
                            root.TryGetProperty("r", out var rProp) &&
                            root.TryGetProperty("g", out var gProp) &&
                            root.TryGetProperty("b", out var bProp))
                        {
                            return Color.FromArgb(aProp.GetInt32(), rProp.GetInt32(), gProp.GetInt32(), bProp.GetInt32());
                        }

                        // Try named color
                        if (root.TryGetProperty("name", out var nameProp))
                        {
                            return Color.FromName(nameProp.GetString());
                        }
                    }
                }
                // Try to read as integer (ARGB value)
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    return Color.FromArgb(reader.GetInt32());
                }
                // Try to read as string (named color or hex)
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var colorString = reader.GetString();
                    if (!string.IsNullOrEmpty(colorString))
                    {
                        // Try hex format: #RRGGBB or #AARRGGBB
                        if (colorString.StartsWith("#"))
                        {
                            var hex = colorString.Substring(1);
                            if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var argb))
                            {
                                if (hex.Length == 6)
                                    argb = 0xFF << 24 | argb; // Add full alpha
                                return Color.FromArgb(argb);
                            }
                        }

                        // Try named color
                        return Color.FromName(colorString);
                    }
                }
            }
            catch
            {
                // Return empty color on error, will be replaced by default
            }

            return Color.Empty;
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            // Write as object with ARGB components for clarity
            writer.WriteStartObject();
            writer.WriteNumber("a", value.A);
            writer.WriteNumber("r", value.R);
            writer.WriteNumber("g", value.G);
            writer.WriteNumber("b", value.B);
            writer.WriteEndObject();
        }
    }
}
