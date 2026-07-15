using System;
using System.IO;
using System.Text.Json;
using TouchpadNumpad.Models;

namespace TouchpadNumpad.Services
{
    public class ProfileManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };
        private void EnsureProfilesFolder()
        {
            Directory.CreateDirectory(_profilesFolder);
        }

        private readonly string _profilesFolder =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Profiles");
        private string GetProfilePath(string profileName)
        {
            return Path.Combine(
                _profilesFolder,
                $"{profileName}.json");
        }
        public Profile LoadProfile(string profileName)
        {
            EnsureProfilesFolder();
            string profilePath = GetProfilePath(profileName);

            if (!File.Exists(profilePath))
            {
                Profile defaultProfile = CreateDefaultProfile(profileName, DefaultLayouts.Numpad);
                SaveProfile(defaultProfile);
                return defaultProfile;
            }

            string json = File.ReadAllText(profilePath);

            Profile? profile = JsonSerializer.Deserialize<Profile>(json);

            if (profile == null)
            {
                throw new InvalidDataException(
                    $"Failed to load profile '{profileName}'.");
            }

            return profile;
        }

        public void SaveProfile(Profile profile)
        {
            EnsureProfilesFolder();
            string profilePath = GetProfilePath(profile.Name);

            string json = JsonSerializer.Serialize(profile,JsonOptions);

            File.WriteAllText(profilePath, json);
        }

        private Profile CreateDefaultProfile(string profileName, (int Row, int Column, string Label, string Value)[] layout)
        {
            Profile profile = new()
            {
                Name = profileName,
                Rows = 4,
                Columns = 3
            };

            foreach (var cell in layout)
            {
                profile.Cells.Add(new GridCell
                {
                    Row = cell.Row,
                    Column = cell.Column,
                    Label = cell.Label,
                    Action = new CellAction
                    {
                        Type = ActionType.KeyboardKey,
                        Value = cell.Value
                    }
                });
            }

            return profile;
        }

        public List<string> GetProfiles()
        {
            EnsureProfilesFolder();

            return Directory.GetFiles(_profilesFolder, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .OrderBy(x => x)
                .ToList();
        }
        public void CreateProfile(string profileName)
        {
            Profile profile = CreateDefaultProfile(profileName,DefaultLayouts.Numpad);

            SaveProfile(profile);
        }

        public void DuplicateProfile(string sourceProfile, string newProfileName)
        {
            Profile profile = LoadProfile(sourceProfile);

            profile.Name = newProfileName;

            SaveProfile(profile);
        }
        public void DeleteProfile(string profileName)
        {
            string profilePath = GetProfilePath(profileName);

            if (File.Exists(profilePath))
            {
                File.Delete(profilePath);
            }
        }
        public void RenameProfile(string oldName, string newName)
        {
            Profile profile = LoadProfile(oldName);

            profile.Name = newName;

            SaveProfile(profile);

            string oldPath = GetProfilePath(oldName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }
    }

}