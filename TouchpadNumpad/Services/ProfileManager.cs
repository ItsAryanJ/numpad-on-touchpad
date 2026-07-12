using System;
using System.IO;
using System.Text.Json;
using TouchpadNumpad.Models;

namespace TouchpadNumpad.Services
{
    public class ProfileManager
    {
        private readonly string _profilesFolder =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Profiles");

        public Profile LoadProfile(string profileName)
        {
            Directory.CreateDirectory(_profilesFolder);

            string profilePath = Path.Combine(
                _profilesFolder,
                $"{profileName}.json");

            if (!File.Exists(profilePath))
            {
                Profile profile = CreateDefaultProfile(profileName);
                SaveProfile(profile);
                return profile;
            }

            string json = File.ReadAllText(profilePath);

            return JsonSerializer.Deserialize<Profile>(json)!;
        }

        public void SaveProfile(Profile profile)
        {
            Directory.CreateDirectory(_profilesFolder);

            string profilePath = Path.Combine(
                _profilesFolder,
                $"{profile.Name}.json");

            string json = JsonSerializer.Serialize(
                profile,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(profilePath, json);
        }

        private Profile CreateDefaultProfile(string profileName)
        {
            return new Profile
            {
                Name = profileName,
                Rows = 4,
                Columns = 3,
                Cells = new()
            };
        }
    }
}