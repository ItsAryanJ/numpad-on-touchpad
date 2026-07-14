using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace TouchpadNumpad.UI
{
    public partial class InputDialog : Form
    {
        
        public InputDialog(string title, string prompt)
        {
            InitializeComponent();

            Text = title;
            lblPrompt.Text = prompt;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InputText
        {
            get => txtInput.Text;
            set => txtInput.Text = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<string, string?>? Validator { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string text = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(
                    "Please enter a name.",
                    "Touchpad Numpad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtInput.Focus();
                return;
            }

            if (Validator != null)
            {
                string? error = Validator(text);

                if (error != null)
                {
                    MessageBox.Show(
                        error,
                        "Touchpad Numpad",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtInput.Focus();
                    txtInput.SelectAll();
                    return;
                }
            }

            InputText = text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
    }
}
