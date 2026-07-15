using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TouchpadNumpad.Services;

namespace TouchpadNumpad.UI
{
    public partial class RecordShortcutForm : Form
    {
        public List<string> Shortcut { get; } = new();

        private readonly HashSet<Keys> _pressedKeys = new();
        
        private static readonly HashSet<string> ForbiddenKeys = new()
        {
            "Capital",      // Caps Lock
            "NumLock",
            "Scroll",       // Scroll Lock
            "PrintScreen",
            "Escape"
        };
        public RecordShortcutForm()
        {
            InitializeComponent();

            KeyPreview = true;
        }
        private void UpdateShortcut()
        {
            Shortcut.Clear();

            bool ctrl = _pressedKeys.Any(k =>
                k == Keys.ControlKey ||
                k == Keys.LControlKey ||
                k == Keys.RControlKey);

            bool shift = _pressedKeys.Any(k =>
                k == Keys.ShiftKey ||
                k == Keys.LShiftKey ||
                k == Keys.RShiftKey);

            bool alt = _pressedKeys.Any(k =>
                k == Keys.Menu ||
                k == Keys.LMenu ||
                k == Keys.RMenu);

            if (ctrl)
                Shortcut.Add("Ctrl");

            if (shift)
                Shortcut.Add("Shift");

            if (alt)
                Shortcut.Add("Alt");

            foreach (Keys key in _pressedKeys.OrderBy(k => k))
            {
                if (key == Keys.ControlKey ||
                    key == Keys.LControlKey ||
                    key == Keys.RControlKey ||

                    key == Keys.ShiftKey ||
                    key == Keys.LShiftKey ||
                    key == Keys.RShiftKey ||

                    key == Keys.Menu ||
                    key == Keys.LMenu ||
                    key == Keys.RMenu)
                {
                    continue;
                }

                Shortcut.Add(key.ToString());
            }

            lblShortcut.Text = string.Join(" + ", Shortcut);
        }
        private string? ValidateShortcut()
        {
            string? forbiddenKey = Shortcut.FirstOrDefault(ForbiddenKeys.Contains);

            if (forbiddenKey != null)
            {
                return $"{KeyDefinitions.GetDisplayLabel(forbiddenKey)} cannot be used.";
            }

            if (Shortcut.Count < 2)
                return "Shortcut must contain at least 2 keys.";

            if (Shortcut.Count > 4)
                return "Shortcut cannot contain more than 4 keys.";

            bool hasModifier = Shortcut.Any(key =>
                key == "Ctrl" ||
                key == "Shift" ||
                key == "Alt" ||
                key == "Win");

            if (!hasModifier)
                return "Shortcut must contain Ctrl, Shift, Alt or Win.";

            bool hasNonModifier = Shortcut.Any(key =>
                key != "Ctrl" &&
                key != "Shift" &&
                key != "Alt" &&
                key != "Win");

            if (!hasNonModifier)
                return "Shortcut must contain a non-modifier key.";

            return null;
        }

        private void RecordShortcutForm_KeyDown(object sender, KeyEventArgs e)
        {
            lblError.Text = "";
            _pressedKeys.Add(e.KeyCode);

            UpdateShortcut();

            e.SuppressKeyPress = true;
        }

        private void RecordShortcutForm_KeyUp(object sender, KeyEventArgs e)
        {
            _pressedKeys.Remove(e.KeyCode);

            if (_pressedKeys.Count == 0)
            {
                string? error = ValidateShortcut();

                if (error != null)
                {
                    lblError.Text = error;

                    Shortcut.Clear();
                    _pressedKeys.Clear();

                    lblShortcut.Text = "Try Again...";

                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

    }
}
