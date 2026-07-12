using System;
using System.Collections.Generic;
using System.Text;
using TouchpadNumpad.Models;
using System.Text.Json;
using System.IO;

namespace TouchpadNumpad.Services
{
    public class SettingsManager
    {
        private readonly string _settingsPath =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Config",
            "settings.json");
        public Settings Load()
        {
            if (!File.Exists(_settingsPath))
            {
                Settings settings = new Settings();

                Save(settings);

                return settings;
            }

            string json = File.ReadAllText(_settingsPath);

            return JsonSerializer.Deserialize<Settings>(json)!;
        }

        public void Save(Settings settings)
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(_settingsPath)!);

            string json =
                JsonSerializer.Serialize(
                    settings,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(_settingsPath, json);
        }
    }
}
