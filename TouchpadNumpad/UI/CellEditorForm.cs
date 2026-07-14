using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TouchpadNumpad.Models;
using TouchpadNumpad.Services;
using TouchpadNumpad.UI;

namespace TouchpadNumpad.UI
{
    public partial class CellEditorForm : Form
    {
        private GridCell _cell;
        public CellEditorForm(GridCell cell)
        {
            InitializeComponent();

            _cell = cell;
        }

        private void CellEditorForm_Load(object sender, EventArgs e)
        {
            lblCurrentCell.Text = KeyDefinitions.GetDisplayLabel(_cell.Action.Value);

            cmbAction.DataSource = Enum.GetValues<ActionType>();
            cmbAction.SelectedItem = _cell.Action.Type;

            LoadValues();

            cmbValue.SelectedItem = _cell.Action.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _cell.Action.Type = (ActionType)cmbAction.SelectedItem!;

            _cell.Action.Value = cmbValue.SelectedItem?.ToString() ?? "";
            _cell.Label = KeyDefinitions.GetDisplayLabel(_cell.Action.Value);

            DialogResult = DialogResult.OK;

            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadValues()
        {
            cmbValue.Items.Clear();

            switch ((ActionType)cmbAction.SelectedItem!)
            {
                case ActionType.KeyboardKey:
                    cmbValue.Items.AddRange(KeyDefinitions.KeyboardKeys.Keys.ToArray());
                    break;

                case ActionType.Media:
                    cmbValue.Items.AddRange(KeyDefinitions.MediaKeys);
                    break;

                case ActionType.Brightness:
                    cmbValue.Items.AddRange(KeyDefinitions.BrightnessKeys);
                    break;
            }
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadValues();
            if (cmbValue.Items.Count > 0)
                cmbValue.SelectedIndex = 0;
        }

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCurrentCell.Text = KeyDefinitions.GetDisplayLabel(cmbValue.Text);
        }
    }
}
