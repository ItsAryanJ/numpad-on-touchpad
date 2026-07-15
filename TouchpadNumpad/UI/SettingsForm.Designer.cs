namespace TouchpadNumpad.UI
{
    partial class SettingsForm
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
            label1 = new Label();
            cmbProfiles = new ComboBox();
            label2 = new Label();
            numRows = new NumericUpDown();
            label3 = new Label();
            numColumns = new NumericUpDown();
            btnCancel = new Button();
            btnSave = new Button();
            pnlPreview = new Panel();
            btnNewProfile = new Button();
            btnDuplicateProfile = new Button();
            btnDeleteProfile = new Button();
            btnRenameProfile = new Button();
            txtToggleShortcut = new TextBox();
            btnChangeShortcut = new Button();
            ((System.ComponentModel.ISupportInitialize)numRows).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numColumns).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 42);
            label1.Name = "label1";
            label1.Size = new Size(97, 20);
            label1.TabIndex = 0;
            label1.Text = "Active Profile";
            // 
            // cmbProfiles
            // 
            cmbProfiles.FormattingEnabled = true;
            cmbProfiles.Location = new Point(144, 39);
            cmbProfiles.Name = "cmbProfiles";
            cmbProfiles.Size = new Size(151, 28);
            cmbProfiles.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(80, 85);
            label2.Name = "label2";
            label2.Size = new Size(44, 20);
            label2.TabIndex = 2;
            label2.Text = "Rows";
            // 
            // numRows
            // 
            numRows.Location = new Point(144, 85);
            numRows.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numRows.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRows.Name = "numRows";
            numRows.Size = new Size(150, 27);
            numRows.TabIndex = 3;
            numRows.Value = new decimal(new int[] { 4, 0, 0, 0 });
            numRows.ValueChanged += numRows_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(58, 130);
            label3.Name = "label3";
            label3.Size = new Size(66, 20);
            label3.TabIndex = 4;
            label3.Text = "Columns";
            // 
            // numColumns
            // 
            numColumns.Location = new Point(145, 128);
            numColumns.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            numColumns.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numColumns.Name = "numColumns";
            numColumns.Size = new Size(150, 27);
            numColumns.TabIndex = 5;
            numColumns.Value = new decimal(new int[] { 7, 0, 0, 0 });
            numColumns.ValueChanged += numColumns_ValueChanged;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(169, 211);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(58, 211);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // pnlPreview
            // 
            pnlPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlPreview.Location = new Point(311, 57);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(343, 336);
            pnlPreview.TabIndex = 8;
            // 
            // btnNewProfile
            // 
            btnNewProfile.Location = new Point(16, 292);
            btnNewProfile.Name = "btnNewProfile";
            btnNewProfile.Size = new Size(94, 29);
            btnNewProfile.TabIndex = 9;
            btnNewProfile.Text = "New";
            btnNewProfile.UseVisualStyleBackColor = true;
            btnNewProfile.Click += btnNewProfile_Click;
            // 
            // btnDuplicateProfile
            // 
            btnDuplicateProfile.Location = new Point(116, 292);
            btnDuplicateProfile.Name = "btnDuplicateProfile";
            btnDuplicateProfile.Size = new Size(94, 29);
            btnDuplicateProfile.TabIndex = 10;
            btnDuplicateProfile.Text = "Duplicate";
            btnDuplicateProfile.UseVisualStyleBackColor = true;
            btnDuplicateProfile.Click += btnDuplicateProfile_Click;
            // 
            // btnDeleteProfile
            // 
            btnDeleteProfile.Location = new Point(16, 327);
            btnDeleteProfile.Name = "btnDeleteProfile";
            btnDeleteProfile.Size = new Size(94, 29);
            btnDeleteProfile.TabIndex = 11;
            btnDeleteProfile.Text = "Delete";
            btnDeleteProfile.UseVisualStyleBackColor = true;
            btnDeleteProfile.Click += btnDeleteProfile_Click;
            // 
            // btnRenameProfile
            // 
            btnRenameProfile.Location = new Point(116, 327);
            btnRenameProfile.Name = "btnRenameProfile";
            btnRenameProfile.Size = new Size(76, 27);
            btnRenameProfile.TabIndex = 12;
            btnRenameProfile.Text = "Rename";
            btnRenameProfile.UseVisualStyleBackColor = true;
            btnRenameProfile.Click += btnRenameProfile_Click;
            // 
            // txtToggleShortcut
            // 
            txtToggleShortcut.Location = new Point(27, 395);
            txtToggleShortcut.Name = "txtToggleShortcut";
            txtToggleShortcut.ReadOnly = true;
            txtToggleShortcut.Size = new Size(125, 27);
            txtToggleShortcut.TabIndex = 13;
            // 
            // btnChangeShortcut
            // 
            btnChangeShortcut.Location = new Point(169, 395);
            btnChangeShortcut.Name = "btnChangeShortcut";
            btnChangeShortcut.Size = new Size(94, 29);
            btnChangeShortcut.TabIndex = 14;
            btnChangeShortcut.Text = "Change";
            btnChangeShortcut.UseVisualStyleBackColor = true;
            btnChangeShortcut.Click += btnChangeShortcut_Click;
            // 
            // SettingsForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(682, 453);
            Controls.Add(btnChangeShortcut);
            Controls.Add(txtToggleShortcut);
            Controls.Add(btnRenameProfile);
            Controls.Add(btnDeleteProfile);
            Controls.Add(btnDuplicateProfile);
            Controls.Add(btnNewProfile);
            Controls.Add(pnlPreview);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(numColumns);
            Controls.Add(label3);
            Controls.Add(numRows);
            Controls.Add(label2);
            Controls.Add(cmbProfiles);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Touchpad Numpad Settings";
            Load += SettingsForm_Load;
            ((System.ComponentModel.ISupportInitialize)numRows).EndInit();
            ((System.ComponentModel.ISupportInitialize)numColumns).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbProfiles;
        private Label label2;
        private NumericUpDown numRows;
        private Label label3;
        private NumericUpDown numColumns;
        private Button btnCancel;
        private Button btnSave;
        private Panel pnlPreview;
        private Button btnNewProfile;
        private Button btnDuplicateProfile;
        private Button btnDeleteProfile;
        private Button btnRenameProfile;
        private TextBox txtToggleShortcut;
        private Button btnChangeShortcut;
    }
}