using System;
using System.Drawing;
using ScintillaNET;
using static ScintillaNET.Style;

namespace SQL_Extractor_for_Excel.Settings
{
    /// <summary>
    /// Manages editor settings for a specific SQL Editor window.
    /// Supports both global settings (shared across all windows) and
    /// local settings (specific to a single window instance).
    /// </summary>
    public class EditorSettingsManager
    {
        #region Private Fields

        private readonly AppSettings m_globalSettings;
        private AppSettings m_localSettings;

        // Track which settings should use global vs local values
        // This is determined by the user's "Apply globally" checkbox selections
        private SettingsOverrideFlags m_overrideFlags;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new EditorSettingsManager with a copy of global settings for local modifications.
        /// Use this constructor when creating a new window without existing settings.
        /// </summary>
        public EditorSettingsManager()
        {
            m_globalSettings = GlobalAppSettings.Instance;
            m_localSettings = m_globalSettings.Clone();
            m_overrideFlags = new SettingsOverrideFlags();
        }

        /// <summary>
        /// Creates a new EditorSettingsManager with existing local settings.
        /// Use this constructor when opening settings for an existing window.
        /// </summary>
        /// <param name="existingLocalSettings">The current local settings from the owner window</param>
        public EditorSettingsManager(AppSettings existingLocalSettings, SettingsOverrideFlags existingOverrideFlags)
        {
            m_globalSettings = GlobalAppSettings.Instance;
            m_localSettings = existingLocalSettings?.Clone() ?? m_globalSettings.Clone();
            m_overrideFlags = existingOverrideFlags?.Clone() ?? new SettingsOverrideFlags();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the local settings (window-specific modifications).
        /// </summary>
        public AppSettings Local => m_localSettings;

        /// <summary>
        /// Gets the global settings (shared across all windows).
        /// </summary>
        public AppSettings Global => m_globalSettings;

        /// <summary>
        /// Gets the override flags that control which settings use global vs local values.
        /// </summary>
        public SettingsOverrideFlags OverrideFlags => m_overrideFlags;

        #endregion

        #region Public Methods - Get Effective Values

        public void UpdateFrom(AppSettings newLocalSettings, SettingsOverrideFlags newOverrideFlags)
        {
            if (newLocalSettings != null)
                m_localSettings = newLocalSettings.Clone();

            if (newOverrideFlags != null)
                m_overrideFlags = newOverrideFlags.Clone();
        }

        /// <summary>
        /// Gets the effective KeepSqlEditorOnTop setting based on override flag.
        /// </summary>
        public bool GetKeepSqlEditorOnTop()
        {
            return m_overrideFlags.KeepSqlEditorOnTopUseGlobal
                ? m_globalSettings.KeepSqlEditorOnTop
                : m_localSettings.KeepSqlEditorOnTop;
        }

        /// <summary>
        /// Gets the effective TextWrapEnabled setting based on override flag.
        /// </summary>
        public bool GetTextWrapEnabled()
        {
            return m_overrideFlags.TextWrapEnabledUseGlobal
                ? m_globalSettings.TextWrapEnabled
                : m_localSettings.TextWrapEnabled;
        }

        /// <summary>
        /// Gets the effective DefaultExportOption setting based on override flag.
        /// </summary>
        public string GetDefaultExportOption()
        {
            return m_overrideFlags.DefaultExportOptionUseGlobal
                ? m_globalSettings.DefaultExportOption
                : m_localSettings.DefaultExportOption;
        }

        /// <summary>
        /// Gets the effective StartSqlQueryOnLaunch setting based on override flag.
        /// </summary>
        public string GetStartSqlQueryOnLaunch()
        {
            return m_overrideFlags.StartSqlQueryOnLaunchUseGlobal
                ? m_globalSettings.StartSqlQueryOnLaunch
                : m_localSettings.StartSqlQueryOnLaunch;
        }

        /// <summary>
        /// Gets the effective MsSqlKeywords setting based on override flag.
        /// </summary>
        public string GetMsSqlKeywords()
        {
            return m_overrideFlags.MsSqlKeywordsUseGlobal
                ? m_globalSettings.MsSqlKeywords
                : m_localSettings.MsSqlKeywords;
        }

        /// <summary>
        /// Gets the effective OracleKeywords setting based on override flag.
        /// </summary>
        public string GetOracleKeywords()
        {
            return m_overrideFlags.OracleKeywordsUseGlobal
                ? m_globalSettings.OracleKeywords
                : m_localSettings.OracleKeywords;
        }

        /// <summary>
        /// Gets the effective KeywordColor setting based on override flag.
        /// </summary>
        public Color GetKeywordColor()
        {
            return m_overrideFlags.KeywordColorUseGlobal
                ? m_globalSettings.KeywordColor
                : m_localSettings.KeywordColor;
        }

        /// <summary>
        /// Gets the effective StringColor setting based on override flag.
        /// </summary>
        public Color GetStringColor()
        {
            return m_overrideFlags.StringColorUseGlobal
                ? m_globalSettings.StringColor
                : m_localSettings.StringColor;
        }

        /// <summary>
        /// Gets the effective CommentColor setting based on override flag.
        /// </summary>
        public Color GetCommentColor()
        {
            return m_overrideFlags.CommentColorUseGlobal
                ? m_globalSettings.CommentColor
                : m_localSettings.CommentColor;
        }

        /// <summary>
        /// Gets the effective NumberColor setting based on override flag.
        /// </summary>
        public Color GetNumberColor()
        {
            return m_overrideFlags.NumberColorUseGlobal
                ? m_globalSettings.NumberColor
                : m_localSettings.NumberColor;
        }

        /// <summary>
        /// Gets the effective DefaultForegroundColor setting based on override flag.
        /// </summary>
        public Color GetDefaultForegroundColor()
        {
            return m_overrideFlags.DefaultForegroundColorUseGlobal
                ? m_globalSettings.DefaultForegroundColor
                : m_localSettings.DefaultForegroundColor;
        }

        /// <summary>
        /// Gets the effective EditorBackgroundColor setting based on override flag.
        /// </summary>
        public Color GetEditorBackgroundColor()
        {
            return m_overrideFlags.EditorBackgroundColorUseGlobal
                ? m_globalSettings.EditorBackgroundColor
                : m_localSettings.EditorBackgroundColor;
        }

        /// <summary>
        /// Gets keywords for the specified database type based on override flag.
        /// </summary>
        public string GetKeywordsForDbType(string dbType)
        {
            // Keywords are tied together - if any keyword setting uses global, use global for all
            if (m_overrideFlags.MsSqlKeywordsUseGlobal || m_overrideFlags.OracleKeywordsUseGlobal)
            {
                return m_globalSettings.GetKeywordsForDbType(dbType);
            }
            return m_localSettings.GetKeywordsForDbType(dbType);
        }

        #endregion

        #region Public Methods - Apply to Form

        /// <summary>
        /// Applies the current settings to the specified SQL Editor form.
        /// This only applies window-level settings (TopMost). Use ApplyAllSettings
        /// to apply editor-specific settings like syntax colors and text wrap.
        /// </summary>
        /// <param name="form">The SqlEditorForm to apply settings to</param>
        public void ApplyToForm(SqlEditorForm form)
        {
            if (form == null)
                return;

            // Apply window behavior settings
            form.TopMost = GetKeepSqlEditorOnTop();
        }

        /// <summary>
        /// Applies all settings to the form and editor in one call.
        /// This is the comprehensive method that applies everything.
        /// </summary>
        /// <param name="form">The SqlEditorForm to apply window settings to</param>
        /// <param name="editor">The Scintilla editor to apply editor settings to</param>
        /// <param name="dbType">The database type for keywords (SqlServer, Oracle, Excel)</param>
        public void ApplyAllSettings(SqlEditorForm form, Scintilla editor, string dbType)
        {
            // Apply window settings
            ApplyToForm(form);

            // Apply editor settings
            if (editor != null)
            {
                ApplySyntaxColors(editor);
                ApplyTextWrap(editor);

/*                if (!string.IsNullOrEmpty(dbType))
                {
                    ApplyKeywords(editor, dbType);
                }*/
            }
        }

        /// <summary>
        /// Applies syntax highlighting colors to the specified Scintilla editor.
        /// </summary>
        /// <param name="editor">The Scintilla editor to apply colors to</param>
        public void ApplySyntaxColors(Scintilla editor)
        {
            if (editor == null)
                return;

            Color keywordColor = GetKeywordColor();
            Color stringColor = GetStringColor();
            Color commentColor = GetCommentColor();
            Color numberColor = GetNumberColor();
            Color defaultColor = GetDefaultForegroundColor();
            Color backColor = GetEditorBackgroundColor();

            // Apply colors to Scintilla styles
            editor.Styles[Style.Sql.Default].ForeColor = defaultColor;
            editor.Styles[Style.Sql.Default].BackColor = backColor;

            editor.Styles[Style.Sql.Word].ForeColor = keywordColor;
            editor.Styles[Style.Sql.Word].BackColor = backColor;

            editor.Styles[Style.Sql.Word2].ForeColor = keywordColor;
            editor.Styles[Style.Sql.Word2].BackColor = backColor;

            editor.Styles[Style.Sql.String].ForeColor = stringColor;
            editor.Styles[Style.Sql.String].BackColor = backColor;

            editor.Styles[Style.Sql.Character].ForeColor = stringColor;
            editor.Styles[Style.Sql.Character].BackColor = backColor;

            editor.Styles[Style.Sql.Comment].ForeColor = commentColor;
            editor.Styles[Style.Sql.Comment].BackColor = backColor;

            editor.Styles[Style.Sql.CommentLine].ForeColor = commentColor;
            editor.Styles[Style.Sql.CommentLine].BackColor = backColor;

            editor.Styles[Style.Sql.CommentDoc].ForeColor = commentColor;
            editor.Styles[Style.Sql.CommentDoc].BackColor = backColor;

            editor.Styles[Style.Sql.Number].ForeColor = numberColor;
            editor.Styles[Style.Sql.Number].BackColor = backColor;

            editor.Styles[Style.Sql.Operator].ForeColor = defaultColor;
            editor.Styles[Style.Sql.Operator].BackColor = backColor;

            editor.Styles[Style.Sql.Identifier].ForeColor = defaultColor;
            editor.Styles[Style.Sql.Identifier].BackColor = backColor;

            // Apply background to default style
            editor.Styles[Style.Default].BackColor = backColor;
            editor.CaretLineBackColor = Color.FromArgb(
                Math.Max(0, Math.Min(255, backColor.R + 5)),
                Math.Max(0, Math.Min(255, backColor.G + 5)),
                Math.Max(0, Math.Min(255, backColor.B + 5)));

            // Force redraw
            editor.Colorize(0, -1);
        }

        /// <summary>
        /// Applies text wrap setting to the specified Scintilla editor.
        /// </summary>
        /// <param name="editor">The Scintilla editor to apply text wrap to</param>
        public void ApplyTextWrap(Scintilla editor)
        {
            if (editor == null)
                return;

            editor.WrapMode = GetTextWrapEnabled()
                ? WrapMode.Word
                : WrapMode.None;
        }

        /// <summary>
        /// Applies SQL keywords to the specified Scintilla editor for the given database type.
        /// </summary>
        /// <param name="editor">The Scintilla editor to apply keywords to</param>
        /// <param name="dbType">The database type (SqlServer, Oracle)</param>
        public void ApplyKeywords(Scintilla editor, string dbType)
        {
            if (editor == null)
                return;

            string keywords = GetKeywordsForDbType(dbType);
            editor.SetKeywords(0, keywords);
        }

        /// <summary>
        /// Synchronizes local settings with global settings for all properties
        /// that have their "Use Global" flag set.
        /// </summary>
        public void SyncLocalWithGlobal()
        {
            if (m_overrideFlags.KeepSqlEditorOnTopUseGlobal)
                m_localSettings.KeepSqlEditorOnTop = m_globalSettings.KeepSqlEditorOnTop;

            if (m_overrideFlags.TextWrapEnabledUseGlobal)
                m_localSettings.TextWrapEnabled = m_globalSettings.TextWrapEnabled;

            if (m_overrideFlags.DefaultExportOptionUseGlobal)
                m_localSettings.DefaultExportOption = m_globalSettings.DefaultExportOption;

            if (m_overrideFlags.StartSqlQueryOnLaunchUseGlobal)
                m_localSettings.StartSqlQueryOnLaunch = m_globalSettings.StartSqlQueryOnLaunch;

            if (m_overrideFlags.MsSqlKeywordsUseGlobal)
                m_localSettings.MsSqlKeywords = m_globalSettings.MsSqlKeywords;

            if (m_overrideFlags.OracleKeywordsUseGlobal)
                m_localSettings.OracleKeywords = m_globalSettings.OracleKeywords;

            if (m_overrideFlags.KeywordColorUseGlobal)
                m_localSettings.KeywordColor = m_globalSettings.KeywordColor;

            if (m_overrideFlags.StringColorUseGlobal)
                m_localSettings.StringColor = m_globalSettings.StringColor;

            if (m_overrideFlags.CommentColorUseGlobal)
                m_localSettings.CommentColor = m_globalSettings.CommentColor;

            if (m_overrideFlags.NumberColorUseGlobal)
                m_localSettings.NumberColor = m_globalSettings.NumberColor;

            if (m_overrideFlags.DefaultForegroundColorUseGlobal)
                m_localSettings.DefaultForegroundColor = m_globalSettings.DefaultForegroundColor;

            if (m_overrideFlags.EditorBackgroundColorUseGlobal)
                m_localSettings.EditorBackgroundColor = m_globalSettings.EditorBackgroundColor;
        }

        #endregion
    }

    /// <summary>
    /// Tracks which settings should use global values vs local values.
    /// Each flag corresponds to a setting and indicates whether the global
    /// value should override the local value.
    /// </summary>
    public class SettingsOverrideFlags
    {
        // Window behavior
        public bool KeepSqlEditorOnTopUseGlobal { get; set; } = false;
        public bool TextWrapEnabledUseGlobal { get; set; } = false;

        // Export
        public bool DefaultExportOptionUseGlobal { get; set; } = false;

        // Startup
        public bool StartSqlQueryOnLaunchUseGlobal { get; set; } = false;

        // Keywords
        public bool MsSqlKeywordsUseGlobal { get; set; } = false;
        public bool OracleKeywordsUseGlobal { get; set; } = false;

        // Syntax colors
        public bool KeywordColorUseGlobal { get; set; } = false;
        public bool StringColorUseGlobal { get; set; } = false;
        public bool CommentColorUseGlobal { get; set; } = false;
        public bool NumberColorUseGlobal { get; set; } = false;
        public bool DefaultForegroundColorUseGlobal { get; set; } = false;
        public bool EditorBackgroundColorUseGlobal { get; set; } = false;

        /// <summary>
        /// Resets all flags to false (use local values).
        /// </summary>
        public void ResetAll()
        {
            KeepSqlEditorOnTopUseGlobal = false;
            TextWrapEnabledUseGlobal = false;
            DefaultExportOptionUseGlobal = false;
            StartSqlQueryOnLaunchUseGlobal = false;
            MsSqlKeywordsUseGlobal = false;
            OracleKeywordsUseGlobal = false;
            KeywordColorUseGlobal = false;
            StringColorUseGlobal = false;
            CommentColorUseGlobal = false;
            NumberColorUseGlobal = false;
            DefaultForegroundColorUseGlobal = false;
            EditorBackgroundColorUseGlobal = false;
        }

        /// <summary>
        /// Sets all flags to true (use global values).
        /// </summary>
        public void SetAllGlobal()
        {
            KeepSqlEditorOnTopUseGlobal = true;
            TextWrapEnabledUseGlobal = true;
            DefaultExportOptionUseGlobal = true;
            StartSqlQueryOnLaunchUseGlobal = true;
            MsSqlKeywordsUseGlobal = true;
            OracleKeywordsUseGlobal = true;
            KeywordColorUseGlobal = true;
            StringColorUseGlobal = true;
            CommentColorUseGlobal = true;
            NumberColorUseGlobal = true;
            DefaultForegroundColorUseGlobal = true;
            EditorBackgroundColorUseGlobal = true;
        }

        public SettingsOverrideFlags Clone()
        {
            return new SettingsOverrideFlags
            {
                KeepSqlEditorOnTopUseGlobal = KeepSqlEditorOnTopUseGlobal,
                TextWrapEnabledUseGlobal = TextWrapEnabledUseGlobal,
                DefaultExportOptionUseGlobal = DefaultExportOptionUseGlobal,
                StartSqlQueryOnLaunchUseGlobal = StartSqlQueryOnLaunchUseGlobal,
                MsSqlKeywordsUseGlobal = MsSqlKeywordsUseGlobal,
                OracleKeywordsUseGlobal = OracleKeywordsUseGlobal,
                KeywordColorUseGlobal = KeywordColorUseGlobal,
                StringColorUseGlobal = StringColorUseGlobal,
                CommentColorUseGlobal = CommentColorUseGlobal,
                NumberColorUseGlobal = NumberColorUseGlobal,
                DefaultForegroundColorUseGlobal = DefaultForegroundColorUseGlobal,
                EditorBackgroundColorUseGlobal = EditorBackgroundColorUseGlobal
            };
        }
    }
}
