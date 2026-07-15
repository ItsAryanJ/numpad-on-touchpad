namespace TouchpadNumpad.UI
{
    partial class RecordShortcutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubTitle = new Label();
            lblShortcut = new Label();
            btnCancel = new Button();
            lblError = new Label();
            lblRules = new Label();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(12, 32);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(115, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Record Shortcut";
            // 
            // lblSubTitle
            // 
            lblSubTitle.AutoSize = true;
            lblSubTitle.Location = new Point(12, 52);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(190, 20);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Press the Desired Shortcut...";
            // 
            // lblShortcut
            // 
            lblShortcut.AutoSize = true;
            lblShortcut.Location = new Point(40, 140);
            lblShortcut.Name = "lblShortcut";
            lblShortcut.Size = new Size(0, 20);
            lblShortcut.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(12, 194);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblError
            // 
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(40, 308);
            lblError.Name = "lblError";
            lblError.Size = new Size(720, 40);
            lblError.TabIndex = 4;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRules
            // 
            lblRules.AutoSize = true;
            lblRules.Location = new Point(469, 9);
            lblRules.Name = "lblRules";
            lblRules.Size = new Size(307, 120);
            lblRules.TabIndex = 5;
            lblRules.Text = "Rules\r\n• 2 to 4 keys\r\n• At least one modifier (Ctrl, Shift, Alt or Win)\r\n• At least one non-modifier key\r\n• Esc, Caps Lock, Num Lock, Scroll Lock, \r\nPrint Screen and Enter Key are not allowed";
            // 
            // RecordShortcutForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(800, 450);
            Controls.Add(lblRules);
            Controls.Add(lblError);
            Controls.Add(btnCancel);
            Controls.Add(lblShortcut);
            Controls.Add(lblSubTitle);
            Controls.Add(lblTitle);
            Name = "RecordShortcutForm";
            Text = "RecordShortcutForm";
            KeyDown += RecordShortcutForm_KeyDown;
            KeyUp += RecordShortcutForm_KeyUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblSubTitle;
        private Label lblShortcut;
        private Button btnCancel;
        private Label lblError;
        private Label lblRules;
    }
}