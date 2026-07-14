using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;


namespace TouchpadNumpad.UI
{
    using TouchpadNumpad.Models;
    using TouchpadNumpad.Services;

    public partial class SettingsForm : Form
    {
        private readonly SettingsManager _settingsManager = new();
        private readonly ProfileManager _profileManager = new();

        private Settings? _settings;
        private Profile? _profile;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _settings = _settingsManager.Load();
            _profile = _profileManager.LoadProfile(_settings.ActiveProfile);

            RefreshProfiles();

            numRows.Value = _profile.Rows;
            numColumns.Value = _profile.Columns;

            DrawPreview();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_settings == null || _profile == null)
                return;

            _settings.ActiveProfile = cmbProfiles.Text;

            _profile.Rows = (int)numRows.Value;
            _profile.Columns = (int)numColumns.Value;

            _settingsManager.Save(_settings);
            _profileManager.SaveProfile(_profile);

            AppState.Settings = _settings;
            AppState.CurrentProfile = _profile;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DrawPreview()
        {
            pnlPreview.Controls.Clear();

            int rows = (int)numRows.Value;
            int cols = (int)numColumns.Value;

            int cellWidth = pnlPreview.ClientSize.Width / cols;
            int cellHeight = pnlPreview.ClientSize.Height / rows;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Button cell = new Button();

                    cell.Width = cellWidth;
                    cell.Height = cellHeight;

                    cell.Left = c * cellWidth;
                    cell.Top = r * cellHeight;

                    GridCell? gridCell = _profile?.Cells.FirstOrDefault(x => x.Row == r && x.Column == c);

                    cell.Text = gridCell?.Label ?? "";
                    cell.Tag = gridCell;

                    cell.Font = new Font("Segoe UI", 14, FontStyle.Bold);

                    cell.FlatStyle = FlatStyle.Flat;
                    cell.FlatAppearance.BorderSize = 1;
                    cell.BackColor = Color.White;

                    cell.Margin = Padding.Empty;

                    cell.TabStop = false;

                    cell.Click += PreviewCell_Click;

                    pnlPreview.Controls.Add(cell);
                }
            }
        }

        private void PreviewCell_Click(object? sender, EventArgs e)
        {
            GridCell? gridCell = (sender as Button)?.Tag as GridCell;

            if (gridCell == null)
                return;

            using CellEditorForm editor = new(gridCell);

            if (editor.ShowDialog() == DialogResult.OK)
            {
                _profileManager.SaveProfile(_profile!);
                DrawPreview();
            }
        }

        private void numRows_ValueChanged(object sender, EventArgs e)
        {
            DrawPreview();
        }

        private void numColumns_ValueChanged(object sender, EventArgs e)
        {
            DrawPreview();
        }

        private void btnNewProfile_Click(object sender, EventArgs e)
        {
            using InputDialog dialog = new("New Profile", "Profile Name");

            dialog.Validator = name =>
            {
                bool exists = _profileManager
                    .GetProfiles()
                    .Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase));

                return exists
                    ? "A profile with this name already exists."
                    : null;
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string profileName = dialog.InputText;

            _profileManager.CreateProfile(profileName);

            _settings.ActiveProfile = profileName;
            _settingsManager.Save(_settings);

            _profile = _profileManager.LoadProfile(profileName);

            RefreshProfiles();

            numRows.Value = _profile.Rows;
            numColumns.Value = _profile.Columns;

            DrawPreview();
        }

        private void RefreshProfiles()
        {
            cmbProfiles.Items.Clear();

            foreach (string profile in _profileManager.GetProfiles())
            {
                cmbProfiles.Items.Add(profile);
            }

            if (cmbProfiles.Items.Contains(_settings.ActiveProfile))
            {
                cmbProfiles.SelectedItem = _settings.ActiveProfile;
            }
            else if (cmbProfiles.Items.Count > 0)
            {
                cmbProfiles.SelectedIndex = 0;
            }

            btnRenameProfile.Enabled = cmbProfiles.Items.Count > 0;
            btnDeleteProfile.Enabled = cmbProfiles.Items.Count > 1;
        }

        private void btnDuplicateProfile_Click(object sender, EventArgs e)
        {
            using InputDialog dialog = new(
                "Duplicate Profile",
                "New Profile Name");

            dialog.Validator = name =>
            {
                bool exists = _profileManager
                    .GetProfiles()
                    .Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase));

                return exists
                    ? "A profile with this name already exists."
                    : null;
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string profileName = dialog.InputText;

            _profileManager.DuplicateProfile(
                _settings.ActiveProfile,
                profileName);

            _settings.ActiveProfile = profileName;
            _settingsManager.Save(_settings);

            _profile = _profileManager.LoadProfile(profileName);

            RefreshProfiles();

            numRows.Value = _profile.Rows;
            numColumns.Value = _profile.Columns;

            DrawPreview();
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            List<string> profiles = _profileManager.GetProfiles();

            if (profiles.Count <= 1)
            {
                MessageBox.Show(
                    "At least one profile must exist.",
                    "Touchpad Numpad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the profile '{_settings.ActiveProfile}'?",
                "Touchpad Numpad",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            string deletedProfile = _settings.ActiveProfile;

            int deletedIndex = profiles.IndexOf(deletedProfile);

            _profileManager.DeleteProfile(deletedProfile);

            profiles = _profileManager.GetProfiles();

            if (profiles.Count == 0)
            {
                MessageBox.Show(
                    "No profiles are available.",
                    "Touchpad Numpad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int newIndex = Math.Min(deletedIndex, profiles.Count - 1);

            _settings.ActiveProfile = profiles[newIndex];
            _settingsManager.Save(_settings);

            _profile = _profileManager.LoadProfile(_settings.ActiveProfile);

            RefreshProfiles();
            btnDeleteProfile.Enabled = cmbProfiles.Items.Count > 1;

            numRows.Value = _profile.Rows;
            numColumns.Value = _profile.Columns;

            DrawPreview();
        }

        private void btnRenameProfile_Click(object sender, EventArgs e)
        {
            using InputDialog dialog = new(
                "Rename Profile",
                "New Profile Name");

            dialog.InputText = _settings.ActiveProfile;

            dialog.Validator = name =>
            {
                if (name.Equals(
                    _settings.ActiveProfile,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                bool exists = _profileManager
                    .GetProfiles()
                    .Any(p => p.Equals(
                        name,
                        StringComparison.OrdinalIgnoreCase));

                return exists
                    ? "A profile with this name already exists."
                    : null;
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string newName = dialog.InputText;

            _profileManager.RenameProfile(
                _settings.ActiveProfile,
                newName);

            _settings.ActiveProfile = newName;
            _settingsManager.Save(_settings);

            _profile = _profileManager.LoadProfile(newName);

            RefreshProfiles();

            DrawPreview();
        }
    }
}
