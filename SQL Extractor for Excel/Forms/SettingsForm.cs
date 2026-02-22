using System;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;
using SQL_Extractor_for_Excel.Settings;

namespace SQL_Extractor_for_Excel.Forms
{
    /// <summary>
    /// Settings form with Excel-style category sidebar and per-setting "Apply globally" checkboxes.
    /// Each setting has its own "Apply globally" checkbox above it that determines whether
    /// the setting applies only to the current window (default) or to all windows and future windows.
    /// Settings that differ from global values are marked with an asterisk (*).
    /// </summary>
    public partial class SettingsForm : Form
    {
        #region Private Fields

        private readonly SqlEditorForm m_ownerForm;
        private readonly EditorSettingsManager m_settingsManager;
        private readonly AppSettings m_globalSettings;

        // Color buttons stored for easy access during color picker operations
        private Button m_currentColorButton;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new SettingsForm instance using the owner's current settings.
        /// </summary>
        /// <param name="owner">The SqlEditorForm that opened this settings dialog.
        /// Settings can be applied to this specific window or globally.</param>
        /// <param name="currentSettings">The current local settings from the owner window.
        /// If null, global settings will be used as defaults.</param>
        public SettingsForm(SqlEditorForm owner, AppSettings currentSettings = null, SettingsOverrideFlags currentFlags = null)
        {
            m_ownerForm = owner ?? throw new ArgumentNullException(nameof(owner));
            m_settingsManager = new EditorSettingsManager(currentSettings, currentFlags);
            m_globalSettings = GlobalAppSettings.Instance;

            InitializeComponent();

            // Update form title to include owner's window name
            UpdateFormTitle();

            // Wire up events FIRST (before building categories)
            SetupEventHandlers();

            // Add GroupBoxes to content panel (only one visible at a time)
            SetupContentPanel();

            // Build category tree
            BuildCategories();

            // Load current settings into controls
            LoadAllSettings();
        }

        /// <summary>
        /// Updates the form title to include the owner's window name.
        /// </summary>
        private void UpdateFormTitle()
        {
            string windowName = m_ownerForm.WindowName;
            if (!string.IsNullOrEmpty(windowName))
            {
                this.Text = $"Settings – [{windowName}]";
            }
            else
            {
                this.Text = "Settings";
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Adds all category GroupBoxes to the content panel.
        /// They will be shown/hidden based on tree selection.
        /// </summary>
        private void SetupContentPanel()
        {
            // Add all GroupBoxes to the content panel
            m_panelContent.Controls.AddRange(new Control[]
            {
                m_grpGeneral,
                m_grpQueryEditor,
                m_grpSyntax,
                m_grpExport,
                m_grpKeywords,
                m_grpStartup
            });

            // Set all GroupBoxes to fill width and hide them initially
            foreach (Control ctrl in m_panelContent.Controls)
            {
                if (ctrl is GroupBox grp)
                {
                    grp.Dock = DockStyle.Top;
                    grp.Visible = false;
                }
            }
        }

        /// <summary>
        /// Builds the category tree on the left side of the form.
        /// </summary>
        private void BuildCategories()
        {
            m_treeCategories.Nodes.Clear();

            var nodes = new[]
            {
                new TreeNode("General") { Tag = m_grpGeneral, Name = "General" },
                new TreeNode("Query Editor") { Tag = m_grpQueryEditor, Name = "QueryEditor" },
                new TreeNode("Syntax Highlighting") { Tag = m_grpSyntax, Name = "Syntax" },
                new TreeNode("Export") { Tag = m_grpExport, Name = "Export" },
                new TreeNode("Keywords") { Tag = m_grpKeywords, Name = "Keywords" },
                new TreeNode("Startup") { Tag = m_grpStartup, Name = "Startup" }
            };

            m_treeCategories.Nodes.AddRange(nodes);
            m_treeCategories.ExpandAll();

            // Select first node
            if (nodes.Length > 0)
            {
                m_treeCategories.SelectedNode = nodes[0];
            }
        }

        /// <summary>
        /// Sets up all event handlers for the form controls.
        /// </summary>
        private void SetupEventHandlers()
        {
            // Tree selection changed
            m_treeCategories.AfterSelect += TreeCategories_AfterSelect;

            // Button clicks
            m_btnOK.Click += BtnOK_Click;
            m_btnCancel.Click += BtnCancel_Click;
            m_btnApply.Click += BtnApply_Click;
            m_btnReset.Click += BtnReset_Click;

            // Color buttons
            m_btnKeywordColor.Click += ColorButton_Click;
            m_btnStringColor.Click += ColorButton_Click;
            m_btnCommentColor.Click += ColorButton_Click;
            m_btnNumberColor.Click += ColorButton_Click;
            m_btnDefaultForeground.Click += ColorButton_Click;
            m_btnEditorBackground.Click += ColorButton_Click;

            // Reset keywords button
            m_btnResetKeywords.Click += BtnResetKeywords_Click;

            // Global checkbox changes - update local settings from global when checked
            m_chkKeepOnTopGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkTextWrapGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkDefaultExportGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkStartQueryGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkMsSqlKeywordsGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkOracleKeywordsGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkKeywordColorGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkStringColorGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkCommentColorGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkNumberColorGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkDefaultForegroundGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
            m_chkEditorBackgroundGlobal.CheckedChanged += GlobalCheckbox_CheckedChanged;
        }

        #endregion

        #region Settings Loading

        /// <summary>
        /// Loads all settings from the settings manager into the UI controls.
        /// Settings that differ from global values are marked with an asterisk (*).
        /// </summary>
        private void LoadAllSettings()
        {
            var local = m_settingsManager.Local;
            var global = m_globalSettings;
            var flags = m_settingsManager.OverrideFlags;

            // General - Keep On Top
            m_chkKeepOnTop.Checked = local.KeepSqlEditorOnTop;
            m_chkKeepOnTopGlobal.Checked = flags.KeepSqlEditorOnTopUseGlobal;
            UpdateControlEnabledState(m_chkKeepOnTop, m_chkKeepOnTopGlobal, global.KeepSqlEditorOnTop);
            UpdateLabelIndicator(m_lblKeepOnTop, local.KeepSqlEditorOnTop != global.KeepSqlEditorOnTop);

            // Query Editor - Text Wrap
            m_chkTextWrap.Checked = local.TextWrapEnabled;
            m_chkTextWrapGlobal.Checked = flags.TextWrapEnabledUseGlobal;
            UpdateControlEnabledState(m_chkTextWrap, m_chkTextWrapGlobal, global.TextWrapEnabled);
            UpdateLabelIndicator(m_lblTextWrap, local.TextWrapEnabled != global.TextWrapEnabled);

            // Syntax Colors
            LoadColorSetting(m_btnKeywordColor, local.KeywordColor, global.KeywordColor,
                m_chkKeywordColorGlobal, flags.KeywordColorUseGlobal);
            UpdateLabelIndicator(m_lblKeywordColor, local.KeywordColor != global.KeywordColor);

            LoadColorSetting(m_btnStringColor, local.StringColor, global.StringColor,
                m_chkStringColorGlobal, flags.StringColorUseGlobal);
            UpdateLabelIndicator(m_lblStringColor, local.StringColor != global.StringColor);

            LoadColorSetting(m_btnCommentColor, local.CommentColor, global.CommentColor,
                m_chkCommentColorGlobal, flags.CommentColorUseGlobal);
            UpdateLabelIndicator(m_lblCommentColor, local.CommentColor != global.CommentColor);

            LoadColorSetting(m_btnNumberColor, local.NumberColor, global.NumberColor,
                m_chkNumberColorGlobal, flags.NumberColorUseGlobal);
            UpdateLabelIndicator(m_lblNumberColor, local.NumberColor != global.NumberColor);

            LoadColorSetting(m_btnDefaultForeground, local.DefaultForegroundColor, global.DefaultForegroundColor,
                m_chkDefaultForegroundGlobal, flags.DefaultForegroundColorUseGlobal);
            UpdateLabelIndicator(m_lblDefaultForeground, local.DefaultForegroundColor != global.DefaultForegroundColor);

            LoadColorSetting(m_btnEditorBackground, local.EditorBackgroundColor, global.EditorBackgroundColor,
                m_chkEditorBackgroundGlobal, flags.EditorBackgroundColorUseGlobal);
            UpdateLabelIndicator(m_lblEditorBackground, local.EditorBackgroundColor != global.EditorBackgroundColor);

            // Export
            m_cmbDefaultExport.SelectedItem = local.DefaultExportOption;
            m_chkDefaultExportGlobal.Checked = flags.DefaultExportOptionUseGlobal;
            UpdateControlEnabledState(m_cmbDefaultExport, m_chkDefaultExportGlobal, global.DefaultExportOption);
            UpdateLabelIndicator(m_lblDefaultExport, local.DefaultExportOption != global.DefaultExportOption);

            // Keywords
            m_txtMsSqlKeywords.Text = local.MsSqlKeywords;
            m_chkMsSqlKeywordsGlobal.Checked = flags.MsSqlKeywordsUseGlobal;
            UpdateControlEnabledState(m_txtMsSqlKeywords, m_chkMsSqlKeywordsGlobal, global.MsSqlKeywords);
            UpdateLabelIndicator(m_lblMsSqlKeywords, local.MsSqlKeywords != global.MsSqlKeywords);

            m_txtOracleKeywords.Text = local.OracleKeywords;
            m_chkOracleKeywordsGlobal.Checked = flags.OracleKeywordsUseGlobal;
            UpdateControlEnabledState(m_txtOracleKeywords, m_chkOracleKeywordsGlobal, global.OracleKeywords);
            UpdateLabelIndicator(m_lblOracleKeywords, local.OracleKeywords != global.OracleKeywords);

            // Startup
            m_txtStartQuery.Text = local.StartSqlQueryOnLaunch;
            m_chkStartQueryGlobal.Checked = flags.StartSqlQueryOnLaunchUseGlobal;
            UpdateControlEnabledState(m_txtStartQuery, m_chkStartQueryGlobal, global.StartSqlQueryOnLaunch);
            UpdateLabelIndicator(m_lblStartQuery, local.StartSqlQueryOnLaunch != global.StartSqlQueryOnLaunch);
        }

        /// <summary>
        /// Updates a label to show an asterisk (*) if the setting differs from global.
        /// </summary>
        /// <param name="label">The label to update</param>
        /// <param name="differsFromGlobal">True if the setting differs from global</param>
        private void UpdateLabelIndicator(Label label, bool differsFromGlobal)
        {
            if (label == null)
                return;

            // Get the base text without any asterisk
            string baseText = label.Text.TrimEnd('*').TrimEnd();

            // Add asterisk if differs from global
            if (differsFromGlobal)
            {
                label.Text = baseText + " *";
                label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);
            }
            else
            {
                label.Text = baseText;
                label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Regular);
            }
        }

        /// <summary>
        /// Loads a color setting into a button and updates its global checkbox state.
        /// </summary>
        private void LoadColorSetting(Button button, Color localColor, Color globalColor,
            CheckBox globalCheckbox, bool useGlobal)
        {
            button.BackColor = localColor;
            button.ForeColor = GetContrastingColor(localColor);
            button.Text = ColorToName(localColor);
            globalCheckbox.Checked = useGlobal;

            if (useGlobal)
            {
                button.BackColor = globalColor;
                button.ForeColor = GetContrastingColor(globalColor);
                button.Text = ColorToName(globalColor);
            }
        }

        #endregion

        #region Settings Saving

        /// <summary>
        /// Saves all settings from the UI controls to the settings manager.
        /// Updates global settings if their "Apply globally" checkbox is checked.
        /// </summary>
        private void SaveAllSettings()
        {
            var local = m_settingsManager.Local;
            var global = m_globalSettings;
            var flags = m_settingsManager.OverrideFlags;

            // General - Keep On Top
            flags.KeepSqlEditorOnTopUseGlobal = m_chkKeepOnTopGlobal.Checked;
            if (m_chkKeepOnTopGlobal.Checked)
            {
                global.KeepSqlEditorOnTop = m_chkKeepOnTop.Checked;
                local.KeepSqlEditorOnTop = m_chkKeepOnTop.Checked;
            }
            else
            {
                local.KeepSqlEditorOnTop = m_chkKeepOnTop.Checked;
            }

            // Query Editor - Text Wrap
            flags.TextWrapEnabledUseGlobal = m_chkTextWrapGlobal.Checked;
            if (m_chkTextWrapGlobal.Checked)
            {
                global.TextWrapEnabled = m_chkTextWrap.Checked;
                local.TextWrapEnabled = m_chkTextWrap.Checked;
            }
            else
            {
                local.TextWrapEnabled = m_chkTextWrap.Checked;
            }

            // Syntax Colors
            flags.KeywordColorUseGlobal = m_chkKeywordColorGlobal.Checked;
            SaveColorSetting(m_btnKeywordColor, m_chkKeywordColorGlobal.Checked, local, global,
                (s, c) => s.KeywordColor = c);

            flags.StringColorUseGlobal = m_chkStringColorGlobal.Checked;
            SaveColorSetting(m_btnStringColor, m_chkStringColorGlobal.Checked, local, global,
                (s, c) => s.StringColor = c);

            flags.CommentColorUseGlobal = m_chkCommentColorGlobal.Checked;
            SaveColorSetting(m_btnCommentColor, m_chkCommentColorGlobal.Checked, local, global,
                (s, c) => s.CommentColor = c);

            flags.NumberColorUseGlobal = m_chkNumberColorGlobal.Checked;
            SaveColorSetting(m_btnNumberColor, m_chkNumberColorGlobal.Checked, local, global,
                (s, c) => s.NumberColor = c);

            flags.DefaultForegroundColorUseGlobal = m_chkDefaultForegroundGlobal.Checked;
            SaveColorSetting(m_btnDefaultForeground, m_chkDefaultForegroundGlobal.Checked, local, global,
                (s, c) => s.DefaultForegroundColor = c);

            flags.EditorBackgroundColorUseGlobal = m_chkEditorBackgroundGlobal.Checked;
            SaveColorSetting(m_btnEditorBackground, m_chkEditorBackgroundGlobal.Checked, local, global,
                (s, c) => s.EditorBackgroundColor = c);

            // Export
            flags.DefaultExportOptionUseGlobal = m_chkDefaultExportGlobal.Checked;
            string exportOption = m_cmbDefaultExport.SelectedItem?.ToString() ?? AppSettings.DefaultDefaultExportOption;
            if (m_chkDefaultExportGlobal.Checked)
            {
                global.DefaultExportOption = exportOption;
                local.DefaultExportOption = exportOption;
            }
            else
            {
                local.DefaultExportOption = exportOption;
            }

            // Keywords
            flags.MsSqlKeywordsUseGlobal = m_chkMsSqlKeywordsGlobal.Checked;
            if (m_chkMsSqlKeywordsGlobal.Checked)
            {
                global.MsSqlKeywords = m_txtMsSqlKeywords.Text;
                local.MsSqlKeywords = m_txtMsSqlKeywords.Text;
            }
            else
            {
                local.MsSqlKeywords = m_txtMsSqlKeywords.Text;
            }

            flags.OracleKeywordsUseGlobal = m_chkOracleKeywordsGlobal.Checked;
            if (m_chkOracleKeywordsGlobal.Checked)
            {
                global.OracleKeywords = m_txtOracleKeywords.Text;
                local.OracleKeywords = m_txtOracleKeywords.Text;
            }
            else
            {
                local.OracleKeywords = m_txtOracleKeywords.Text;
            }

            // Startup
            flags.StartSqlQueryOnLaunchUseGlobal = m_chkStartQueryGlobal.Checked;
            if (m_chkStartQueryGlobal.Checked)
            {
                global.StartSqlQueryOnLaunch = m_txtStartQuery.Text;
                local.StartSqlQueryOnLaunch = m_txtStartQuery.Text;
            }
            else
            {
                local.StartSqlQueryOnLaunch = m_txtStartQuery.Text;
            }

            // Save global settings to file if any global checkboxes were checked
            SaveGlobalSettings();
        }

        /// <summary>
        /// Saves a color setting from a button to both local and global settings as appropriate.
        /// </summary>
        private void SaveColorSetting(Button button, bool useGlobal, AppSettings local, AppSettings global,
            Action<AppSettings, Color> setter)
        {
            Color color = button.BackColor;

            if (useGlobal)
            {
                setter(global, color);
                setter(local, color);
            }
            else
            {
                setter(local, color);
            }
        }

        /// <summary>
        /// Saves global settings to the settings file.
        /// </summary>
        private void SaveGlobalSettings()
        {
            GlobalAppSettings.Save(m_globalSettings);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles tree category selection changes to show the appropriate settings panel.
        /// </summary>
        private void TreeCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is GroupBox selectedGroup)
            {
                // Hide all groups
                foreach (Control ctrl in m_panelContent.Controls)
                {
                    if (ctrl is GroupBox grp)
                    {
                        grp.Visible = false;
                    }
                }

                // Show selected group
                selectedGroup.Visible = true;
                selectedGroup.BringToFront();

                // Scroll to top
                m_panelContent.ScrollControlIntoView(selectedGroup);
            }
        }

        /// <summary>
        /// Handles OK button click - saves settings and closes the form.
        /// </summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            SaveAllSettings();
            m_ownerForm.UpdateEditorSettings(m_settingsManager.Local, m_settingsManager.OverrideFlags);
            ApplySettingsToOwner();
            DialogResult = DialogResult.OK;
            Close();
        }


        /// <summary>
        /// Handles Cancel button click - discards changes and closes the form.
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Handles Apply button click - saves settings without closing the form.
        /// </summary>
        private void BtnApply_Click(object sender, EventArgs e)
        {
            SaveAllSettings();
            m_ownerForm.UpdateEditorSettings(m_settingsManager.Local, m_settingsManager.OverrideFlags);
            ApplySettingsToOwner();
            MessageBox.Show("Settings applied successfully.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles Reset to Defaults button click.
        /// </summary>
        private void BtnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Reset all settings to their default values?\n\nThis will reset both local and global settings.",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Reset to defaults
                var defaults = new AppSettings();
                defaults.ApplyDefaultsForMissingValues();

                // Copy defaults to local
                CopySettings(defaults, m_settingsManager.Local);

                // Uncheck all global checkboxes
                m_settingsManager.OverrideFlags.ResetAll();

                // Reload UI
                LoadAllSettings();

                MessageBox.Show("Settings reset to defaults. Click Apply or OK to save.",
                    "Settings Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles Reset Keywords button click.
        /// </summary>
        private void BtnResetKeywords_Click(object sender, EventArgs e)
        {
            m_txtMsSqlKeywords.Text = AppSettings.DefaultMsSqlKeywords;
            m_txtOracleKeywords.Text = AppSettings.DefaultOracleKeywords;
        }

        /// <summary>
        /// Handles color button clicks - shows color picker dialog.
        /// </summary>
        private void ColorButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                using (var dialog = new ColorDialog())
                {
                    dialog.Color = button.BackColor;
                    dialog.FullOpen = true;
                    dialog.ShowHelp = false;

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        button.BackColor = dialog.Color;
                        button.ForeColor = GetContrastingColor(dialog.Color);
                        button.Text = ColorToName(dialog.Color);
                    }
                }
            }
        }

        /// <summary>
        /// Handles global checkbox changes - pushes value to global when turned ON.
        /// </summary>
        private void GlobalCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkbox)
            {
                Control associatedControl = GetAssociatedControl(checkbox);
                if (associatedControl == null) return;

                bool useGlobal = checkbox.Checked;

                if (useGlobal)
                {
                    // Take whatever the user currently sees in the control and push it to GLOBAL immediately
                    object currentValue = GetControlValue(associatedControl);
                    SetGlobalValueForControl(associatedControl, currentValue);

                    // Now disable the control and show the (now updated) global value
                    UpdateControlEnabledState(associatedControl, checkbox, currentValue);

                    // Persist global settings right away (nice live feel)
                    SaveGlobalSettings();
                }
                else
                {
                    // Switching back to local - just re-enable the control
                    associatedControl.Enabled = true;
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Applies settings to the owner form.
        /// </summary>
        private void ApplySettingsToOwner()
        {
            m_settingsManager.SyncLocalWithGlobal();

            // Find the Scintilla editor control on the form
            Scintilla editor = FindScintillaEditor(m_ownerForm);

            // Get the database type
            string dbType = m_ownerForm.ServerType?.ToString() ?? "Oracle";

            // Apply all settings
            m_settingsManager.ApplyAllSettings(m_ownerForm, editor, dbType);
        }

        /// <summary>
        /// Reads the current value from a UI control (the value the user sees).
        /// </summary>
        private object GetControlValue(Control control)
        {
            if (control is CheckBox cb) return cb.Checked;
            if (control is ComboBox cmb) return cmb.SelectedItem?.ToString() ?? "";
            if (control is TextBox txt) return txt.Text ?? "";
            if (control is Button btn) return btn.BackColor;
            return null;
        }

        /// <summary>
        /// Writes a value directly into m_globalSettings for the matching control.
        /// </summary>
        private void SetGlobalValueForControl(Control control, object value)
        {
            if (control == m_chkKeepOnTop)
                m_globalSettings.KeepSqlEditorOnTop = (bool)value;
            else if (control == m_chkTextWrap)
                m_globalSettings.TextWrapEnabled = (bool)value;
            else if (control == m_cmbDefaultExport)
                m_globalSettings.DefaultExportOption = value?.ToString() ?? AppSettings.DefaultDefaultExportOption;
            else if (control == m_txtStartQuery)
                m_globalSettings.StartSqlQueryOnLaunch = value?.ToString() ?? "";
            else if (control == m_txtMsSqlKeywords)
                m_globalSettings.MsSqlKeywords = value?.ToString() ?? AppSettings.DefaultMsSqlKeywords;
            else if (control == m_txtOracleKeywords)
                m_globalSettings.OracleKeywords = value?.ToString() ?? AppSettings.DefaultOracleKeywords;
            else if (control == m_btnKeywordColor)
                m_globalSettings.KeywordColor = (Color)value;
            else if (control == m_btnStringColor)
                m_globalSettings.StringColor = (Color)value;
            else if (control == m_btnCommentColor)
                m_globalSettings.CommentColor = (Color)value;
            else if (control == m_btnNumberColor)
                m_globalSettings.NumberColor = (Color)value;
            else if (control == m_btnDefaultForeground)
                m_globalSettings.DefaultForegroundColor = (Color)value;
            else if (control == m_btnEditorBackground)
                m_globalSettings.EditorBackgroundColor = (Color)value;
        }

        /// <summary>
        /// Finds the Scintilla editor control on the specified form.
        /// </summary>
        /// <param name="form">The form to search</param>
        /// <returns>The Scintilla editor if found, otherwise null</returns>
        private Scintilla FindScintillaEditor(Form form)
        {
            if (form == null)
                return null;

            // Try to find by name first (common naming convention)
            var controls = form.Controls.Find("sqlEditorScintilla", true);
            if (controls.Length > 0 && controls[0] is Scintilla editor)
            {
                return editor;
            }

            // Search by type if not found by name
            foreach (Control control in form.Controls)
            {
                if (control is Scintilla scintilla)
                {
                    return scintilla;
                }

                // Search child controls
                if (control.HasChildren)
                {
                    var found = FindScintillaInChildren(control);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively searches for a Scintilla control in child controls.
        /// </summary>
        private Scintilla FindScintillaInChildren(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Scintilla scintilla)
                {
                    return scintilla;
                }

                if (control.HasChildren)
                {
                    var found = FindScintillaInChildren(control);
                    if (found != null)
                        return found;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates a control's enabled state based on whether it's using global or local settings.
        /// </summary>
        private void UpdateControlEnabledState(Control control, CheckBox globalCheckbox, object globalValue)
        {
            bool useGlobal = globalCheckbox.Checked;
            control.Enabled = !useGlobal;

            if (useGlobal)
                SetControlValue(control, globalValue);
        }

        /// <summary>
        /// Gets the control associated with a global checkbox.
        /// </summary>
        private Control GetAssociatedControl(CheckBox globalCheckbox)
        {
            if (globalCheckbox == m_chkKeepOnTopGlobal) return m_chkKeepOnTop;
            if (globalCheckbox == m_chkTextWrapGlobal) return m_chkTextWrap;
            if (globalCheckbox == m_chkDefaultExportGlobal) return m_cmbDefaultExport;
            if (globalCheckbox == m_chkStartQueryGlobal) return m_txtStartQuery;
            if (globalCheckbox == m_chkMsSqlKeywordsGlobal) return m_txtMsSqlKeywords;
            if (globalCheckbox == m_chkOracleKeywordsGlobal) return m_txtOracleKeywords;
            if (globalCheckbox == m_chkKeywordColorGlobal) return m_btnKeywordColor;
            if (globalCheckbox == m_chkStringColorGlobal) return m_btnStringColor;
            if (globalCheckbox == m_chkCommentColorGlobal) return m_btnCommentColor;
            if (globalCheckbox == m_chkNumberColorGlobal) return m_btnNumberColor;
            if (globalCheckbox == m_chkDefaultForegroundGlobal) return m_btnDefaultForeground;
            if (globalCheckbox == m_chkEditorBackgroundGlobal) return m_btnEditorBackground;
            return null;
        }

        /// <summary>
        /// Gets the global value for a control.
        /// </summary>
        private object GetGlobalValueForControl(Control control)
        {
            if (control == m_chkKeepOnTop) return m_globalSettings.KeepSqlEditorOnTop;
            if (control == m_chkTextWrap) return m_globalSettings.TextWrapEnabled;
            if (control == m_cmbDefaultExport) return m_globalSettings.DefaultExportOption;
            if (control == m_txtStartQuery) return m_globalSettings.StartSqlQueryOnLaunch;
            if (control == m_txtMsSqlKeywords) return m_globalSettings.MsSqlKeywords;
            if (control == m_txtOracleKeywords) return m_globalSettings.OracleKeywords;
            if (control == m_btnKeywordColor) return m_globalSettings.KeywordColor;
            if (control == m_btnStringColor) return m_globalSettings.StringColor;
            if (control == m_btnCommentColor) return m_globalSettings.CommentColor;
            if (control == m_btnNumberColor) return m_globalSettings.NumberColor;
            if (control == m_btnDefaultForeground) return m_globalSettings.DefaultForegroundColor;
            if (control == m_btnEditorBackground) return m_globalSettings.EditorBackgroundColor;
            return null;
        }

        /// <summary>
        /// Sets a control's value from an object.
        /// </summary>
        private void SetControlValue(Control control, object value)
        {
            if (control is CheckBox checkbox && value is bool boolValue)
            {
                checkbox.Checked = boolValue;
            }
            else if (control is ComboBox combobox && value is string stringValue)
            {
                combobox.SelectedItem = stringValue;
            }
            else if (control is TextBox textbox && value is string textValue)
            {
                textbox.Text = textValue;
            }
            else if (control is Button button && value is Color colorValue)
            {
                button.BackColor = colorValue;
                button.ForeColor = GetContrastingColor(colorValue);
                button.Text = ColorToName(colorValue);
            }
        }

        /// <summary>
        /// Gets a contrasting color (black or white) for text on a colored background.
        /// </summary>
        private Color GetContrastingColor(Color backColor)
        {
            // Calculate luminance
            double luminance = (0.299 * backColor.R + 0.587 * backColor.G + 0.114 * backColor.B) / 255;
            return luminance > 0.5 ? Color.Black : Color.White;
        }

        /// <summary>
        /// Converts a color to a human-readable name.
        /// </summary>
        private string ColorToName(Color color)
        {
            // Check for known colors first
            if (color.IsKnownColor)
            {
                return color.Name;
            }

            // Return RGB for custom colors
            if (color.A == 255)
            {
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            return $"ARGB({color.A}, {color.R}, {color.G}, {color.B})";
        }

        /// <summary>
        /// Copies settings from one AppSettings instance to another.
        /// </summary>
        private void CopySettings(AppSettings source, AppSettings target)
        {
            target.SettingsVersion = source.SettingsVersion;
            target.KeepSqlEditorOnTop = source.KeepSqlEditorOnTop;
            target.TextWrapEnabled = source.TextWrapEnabled;
            target.DefaultExportOption = source.DefaultExportOption;
            target.StartSqlQueryOnLaunch = source.StartSqlQueryOnLaunch;
            target.MsSqlKeywords = source.MsSqlKeywords;
            target.OracleKeywords = source.OracleKeywords;
            target.KeywordColor = source.KeywordColor;
            target.StringColor = source.StringColor;
            target.CommentColor = source.CommentColor;
            target.NumberColor = source.NumberColor;
            target.DefaultForegroundColor = source.DefaultForegroundColor;
            target.EditorBackgroundColor = source.EditorBackgroundColor;
        }

        #endregion
    }
}
