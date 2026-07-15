using System;
using TouchpadNumpad.Models;
using System.Text.Json;
using System.IO;

namespace TouchpadNumpad.Services
{
    public class SettingsManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        private void EnsureConfigFolder()
        {
            Directory.CreateDirectory(_configFolder);
        }

        private readonly string _configFolder =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Config");

        private readonly string _settingsPath =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Config",
            "settings.json");

        public SettingsManager()
        {
            _configFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Config");

            _settingsPath = Path.Combine(
                _configFolder,
                "settings.json");
        }
        public Settings Load()
        {
            EnsureConfigFolder();

            if (!File.Exists(_settingsPath))
            {
                Settings defaultSettings = new Settings();

                Save(defaultSettings);

                return defaultSettings;
            }

            string json = File.ReadAllText(_settingsPath);

            Settings? settings = JsonSerializer.Deserialize<Settings>(json);

            if (settings == null)
            {
                throw new InvalidDataException(
                    "Failed to load settings.");
            }

            return settings;
        }

        public void Save(Settings settings)
        {
            EnsureConfigFolder();

            string json = JsonSerializer.Serialize(settings,JsonOptions);

            File.WriteAllText(_settingsPath, json);
        }
    }
}
