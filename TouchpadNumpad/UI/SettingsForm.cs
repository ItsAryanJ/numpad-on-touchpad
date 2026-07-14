using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


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

            cmbProfiles.Items.Clear();

            cmbProfiles.Items.Add(_settings.ActiveProfile);
            cmbProfiles.SelectedItem = _settings.ActiveProfile;

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
    }
}
