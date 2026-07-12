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
            Profile profile = new Profile
            {
                Name = profileName,
                Rows = 4,
                Columns = 3
            };

            profile.Cells.Add(new GridCell
            {
                Row = 0,
                Column = 0,
                Label = "7",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad7"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 0,
                Column = 1,
                Label = "8",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad8"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 0,
                Column = 2,
                Label = "9",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad9"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 1,
                Column = 0,
                Label = "4",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad4"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 1,
                Column = 1,
                Label = "5",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad5"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 1,
                Column = 2,
                Label = "6",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad6"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 2,
                Column = 0,
                Label = "1",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad1"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 2,
                Column = 1,
                Label = "2",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad2"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 2,
                Column = 2,
                Label = "3",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad3"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 3,
                Column = 0,
                Label = "0",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad0"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 3,
                Column = 1,
                Label = "0",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "NumPad0"
                }
            });

            profile.Cells.Add(new GridCell
            {
                Row = 3,
                Column = 2,
                Label = ".",
                Action = new CellAction
                {
                    Type = ActionType.KeyboardKey,
                    Value = "Decimal"
                }
            });

            return profile;
        }
    }
}