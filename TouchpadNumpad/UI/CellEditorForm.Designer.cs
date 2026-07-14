namespace TouchpadNumpad.UI
{
    partial class CellEditorForm
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
            Label = new Label();
            cmbAction = new ComboBox();
            Value = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            cmbValue = new ComboBox();
            lblCurrentCell = new Label();
            action = new Label();
            SuspendLayout();
            // 
            // Label
            // 
            Label.AutoSize = true;
            Label.Location = new Point(374, 49);
            Label.Name = "Label";
            Label.Size = new Size(86, 20);
            Label.TabIndex = 0;
            Label.Text = "Current Cell";
            // 
            // cmbAction
            // 
            cmbAction.FormattingEnabled = true;
            cmbAction.Items.AddRange(new object[] { "Key", "Text" });
            cmbAction.Location = new Point(379, 151);
            cmbAction.Name = "cmbAction";
            cmbAction.Size = new Size(151, 28);
            cmbAction.TabIndex = 2;
            cmbAction.SelectedIndexChanged += cmbAction_SelectedIndexChanged;
            // 
            // Value
            // 
            Value.AutoSize = true;
            Value.Location = new Point(379, 206);
            Value.Name = "Value";
            Value.Size = new Size(45, 20);
            Value.TabIndex = 3;
            Value.Text = "Value";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(436, 290);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(550, 290);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cmbValue
            // 
            cmbValue.FormattingEnabled = true;
            cmbValue.Location = new Point(379, 229);
            cmbValue.Name = "cmbValue";
            cmbValue.Size = new Size(151, 28);
            cmbValue.TabIndex = 7;
            cmbValue.SelectedIndexChanged += cmbValue_SelectedIndexChanged;
            // 
            // lblCurrentCell
            // 
            lblCurrentCell.AutoSize = true;
            lblCurrentCell.Location = new Point(384, 80);
            lblCurrentCell.Name = "lblCurrentCell";
            lblCurrentCell.Size = new Size(57, 20);
            lblCurrentCell.TabIndex = 8;
            lblCurrentCell.Text = "Current";
            // 
            // action
            // 
            action.AutoSize = true;
            action.Location = new Point(379, 128);
            action.Name = "action";
            action.Size = new Size(52, 20);
            action.TabIndex = 9;
            action.Text = "Action";
            // 
            // CellEditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(action);
            Controls.Add(lblCurrentCell);
            Controls.Add(cmbValue);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(Value);
            Controls.Add(cmbAction);
            Controls.Add(Label);
            Name = "CellEditorForm";
            Text = "Edit Cell";
            Load += CellEditorForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Label;
        private ComboBox cmbAction;
        private Label Value;
        private Button btnSave;
        private Button btnCancel;
        private ComboBox cmbValue;
        private Label lblCurrentCell;
        private Label action;
    }
}