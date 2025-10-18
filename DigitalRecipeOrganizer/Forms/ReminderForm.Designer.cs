namespace DigitalRecipeOrganizer.Forms
{
    partial class ReminderForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelReminders;
        private Label lblNoReminders;
        private Button btnClose;
        private Label lblHeader;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblHeader = new Label();
            panelReminders = new Panel();
            lblNoReminders = new Label();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblHeader
            // 
            lblHeader.BackColor = Color.FromArgb(52, 152, 219);
            lblHeader.Dock = DockStyle.Top;
            lblHeader.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeader.ForeColor = Color.White;
            lblHeader.Location = new Point(0, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(600, 50);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "  Recipe Reminders";
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelReminders
            // 
            panelReminders.AutoScroll = true;
            panelReminders.BackColor = Color.FromArgb(236, 240, 241);
            panelReminders.Location = new Point(12, 60);
            panelReminders.Name = "panelReminders";
            panelReminders.Size = new Size(576, 450);
            panelReminders.TabIndex = 1;
            // 
            // lblNoReminders
            // 
            lblNoReminders.Font = new Font("Segoe UI", 12F);
            lblNoReminders.ForeColor = Color.FromArgb(127, 140, 141);
            lblNoReminders.Location = new Point(12, 200);
            lblNoReminders.Name = "lblNoReminders";
            lblNoReminders.Size = new Size(576, 100);
            lblNoReminders.TabIndex = 2;
            lblNoReminders.Text = "No scheduled recipes for the next 7 days.\r\n\r\nEdit a recipe and set a scheduled date to receive reminders!";
            lblNoReminders.TextAlign = ContentAlignment.MiddleCenter;
            lblNoReminders.Visible = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(52, 152, 219);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(250, 520);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 35);
            btnClose.TabIndex = 3;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // ReminderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 570);
            Controls.Add(btnClose);
            Controls.Add(lblNoReminders);
            Controls.Add(panelReminders);
            Controls.Add(lblHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReminderForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recipe Reminders";
            ResumeLayout(false);
        }
    }
}
