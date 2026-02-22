using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using SQL_Extractor_for_Excel.Scripts;

namespace SQL_Extractor_for_Excel.Settings
{
    /// <summary>
    /// Global application settings manager that handles loading, saving, and maintaining
    /// application-wide settings. Supports backward compatibility with older settings files.
    /// </summary>
    public static class GlobalAppSettings
    {
        #region Private Fields

        private static readonly object m_lockObject = new object();
        private static AppSettings m_instance;
        private static readonly string m_filePath = Path.Combine(FileManager.SettingsPath, "settings.json");

        // JSON serializer options with custom converters
        private static readonly JsonSerializerOptions m_jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance of the global application settings.
        /// Settings are automatically loaded from file on first access.
        /// </summary>
        public static AppSettings Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lockObject)
                    {
                        if (m_instance == null)
                        {
                            m_instance = Load();
                        }
                    }
                }
                return m_instance;
            }
        }

        /// <summary>
        /// Gets the full path to the settings file.
        /// </summary>
        public static string FilePath => m_filePath;

        #endregion

        #region Static Constructor

        static GlobalAppSettings()
        {
            // Register custom converters
            m_jsonOptions.Converters.Add(new ColorJsonConverter());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads settings from the JSON file. If the file doesn't exist or is corrupted,
        /// returns default settings. Handles older settings file versions by applying
        /// default values for missing properties.
        /// </summary>
        public static AppSettings Load()
        {
            try
            {
                // Ensure settings directory exists
                EnsureSettingsDirectoryExists();

                // Check if file exists
                if (!File.Exists(m_filePath))
                {
                    // Create default settings file
                    var defaultSettings = CreateDefaultSettings();
                    Save(defaultSettings);
                    return defaultSettings;
                }

                // Read and deserialize
                string json = File.ReadAllText(m_filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    var defaultSettings = CreateDefaultSettings();
                    Save(defaultSettings);
                    return defaultSettings;
                }

                var settings = JsonSerializer.Deserialize<AppSettings>(json, m_jsonOptions);

                if (settings == null)
                {
                    var defaultSettings = CreateDefaultSettings();
                    Save(defaultSettings);
                    return defaultSettings;
                }

                // Apply defaults for any missing values (handles older versions)
                settings.ApplyDefaultsForMissingValues();

                // Update version to current if needed
                if (settings.SettingsVersion < AppSettings.CurrentSettingsVersion)
                {
                    MigrateSettings(settings);
                }

                return settings;
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON deserialization error: {ex.Message}");
                var defaultSettings = CreateDefaultSettings();
                Save(defaultSettings);
                return defaultSettings;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Settings load error: {ex.Message}");
                var defaultSettings = CreateDefaultSettings();
                Save(defaultSettings);
                return defaultSettings;
            }
        }

        /// <summary>
        /// Saves the specified settings to the JSON file and updates the global instance.
        /// </summary>
        /// <param name="settings">The settings to save</param>
        /// <returns>True if save was successful, false otherwise</returns>
        public static bool Save(AppSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            try
            {
                lock (m_lockObject)
                {
                    // Ensure settings directory exists
                    if (!EnsureSettingsDirectoryExists())
                    {
                        return false;
                    }

                    // Update version to current
                    settings.SettingsVersion = AppSettings.CurrentSettingsVersion;

                    // Serialize and write
                    string json = JsonSerializer.Serialize(settings, m_jsonOptions);

                    // Write to temporary file first, then replace (atomic operation)
                    string tempPath = m_filePath + ".tmp";
                    File.WriteAllText(tempPath, json);

                    // Backup existing file
                    if (File.Exists(m_filePath))
                    {
                        string backupPath = m_filePath + ".bak";
                        if (File.Exists(backupPath))
                        {
                            File.Delete(backupPath);
                        }
                        File.Move(m_filePath, backupPath);
                    }

                    // Move temp to actual
                    File.Move(tempPath, m_filePath);

                    // Update instance
                    m_instance = settings;
                    // broadcast global change to all open windows
                    ApplyToAllOpenEditors();

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Settings save error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Instantly applies any "Use Global" settings to EVERY open SqlEditorForm.
        /// Called automatically after every successful global save.
        /// </summary>
        private static void ApplyToAllOpenEditors()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is SqlEditorForm editor && !editor.IsDisposed && !editor.Disposing)
                {
                    editor.SyncAndApplyGlobalSettings();
                }
            }
        }

        /// <summary>
        /// Resets all settings to their default values and saves to file.
        /// </summary>
        /// <returns>True if reset was successful, false otherwise</returns>
        public static bool ResetToDefaults()
        {
            var defaultSettings = CreateDefaultSettings();
            return Save(defaultSettings);
        }

        /// <summary>
        /// Reloads settings from the file, discarding any in-memory changes.
        /// </summary>
        public static void Reload()
        {
            lock (m_lockObject)
            {
                m_instance = Load();
            }
        }

        /// <summary>
        /// Creates a copy of the current global settings for local modification.
        /// This is useful for per-window settings that start with global defaults.
        /// </summary>
        public static AppSettings CreateLocalCopy()
        {
            return Instance.Clone();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new AppSettings instance with all default values.
        /// </summary>
        private static AppSettings CreateDefaultSettings()
        {
            var settings = new AppSettings();
            settings.ApplyDefaultsForMissingValues();
            return settings;
        }

        /// <summary>
        /// Ensures the settings directory exists.
        /// Creates all nested directories if needed.
        /// </summary>
        private static bool EnsureSettingsDirectoryExists()
        {
            try
            {
                string directory = Path.GetDirectoryName(m_filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    if (!Directory.Exists(directory))
                    {
                        // Create all directories in the path
                        Directory.CreateDirectory(directory);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create settings directory: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Migrates settings from older versions to the current version.
        /// This method is called when the loaded settings version is older than current.
        /// </summary>
        /// <param name="settings">The settings to migrate</param>
        private static void MigrateSettings(AppSettings settings)
        {
            // Version 0 to 1: Initial version, all properties have defaults
            if (settings.SettingsVersion < 1)
            {
                // All default values are already applied by ApplyDefaultsForMissingValues
                settings.SettingsVersion = 1;
            }

            // Future migrations would go here:
            // if (settings.SettingsVersion < 2) { ... migrate to v2 ... }

            settings.SettingsVersion = AppSettings.CurrentSettingsVersion;
        }

        #endregion
    }
}
