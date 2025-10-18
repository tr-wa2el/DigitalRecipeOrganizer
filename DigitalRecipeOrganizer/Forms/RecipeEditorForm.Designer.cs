using DigitalRecipeOrganizer.Controls;

namespace DigitalRecipeOrganizer.Forms
{
    partial class RecipeEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtTitle;
        private CategorySelector categorySelector;
        private TextBox txtIngredients;
        private RichTextBox rtbInstructions;
        private NumericUpDown numPrepTime;
        private NumericUpDown numCookTime;
        private NumericUpDown numServings;
        private DateTimePicker dtpScheduled;
        private CheckBox chkScheduled;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;
        private Button btnBold;
        private Button btnItalic;
        private Button btnUnderline;
        private Button btnExport;
        private Label lblTitle;
        private Label lblIngredients;
        private Label lblInstructions;
        private Label lblPrepTime;
        private Label lblCookTime;
        private Label lblServings;
        private Label lblNotes;
        private Panel panelFormatting;

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
            lblTitle = new Label();
            txtTitle = new TextBox();
            categorySelector = new CategorySelector();
            lblIngredients = new Label();
            txtIngredients = new TextBox();
            lblInstructions = new Label();
            rtbInstructions = new RichTextBox();
            panelFormatting = new Panel();
            btnBold = new Button();
            btnItalic = new Button();
            btnUnderline = new Button();
            lblPrepTime = new Label();
            numPrepTime = new NumericUpDown();
            lblCookTime = new Label();
            numCookTime = new NumericUpDown();
            lblServings = new Label();
            numServings = new NumericUpDown();
            chkScheduled = new CheckBox();
            dtpScheduled = new DateTimePicker();
            lblNotes = new Label();
            txtNotes = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            btnExport = new Button();
            panelFormatting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPrepTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCookTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numServings).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(14, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(41, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Title:";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(114, 16);
            txtTitle.Margin = new Padding(3, 4, 3, 4);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(457, 27);
            txtTitle.TabIndex = 1;
            // 
            // categorySelector
            // 
            categorySelector.Location = new Point(114, 60);
            categorySelector.Margin = new Padding(3, 5, 3, 5);
            categorySelector.Name = "categorySelector";
            categorySelector.Size = new Size(251, 40);
            categorySelector.TabIndex = 2;
            // 
            // lblIngredients
            // 
            lblIngredients.AutoSize = true;
            lblIngredients.Location = new Point(14, 113);
            lblIngredients.Name = "lblIngredients";
            lblIngredients.Size = new Size(86, 20);
            lblIngredients.TabIndex = 3;
            lblIngredients.Text = "Ingredients:";
            // 
            // txtIngredients
            // 
            txtIngredients.Location = new Point(114, 113);
            txtIngredients.Margin = new Padding(3, 4, 3, 4);
            txtIngredients.Multiline = true;
            txtIngredients.Name = "txtIngredients";
            txtIngredients.ScrollBars = ScrollBars.Vertical;
            txtIngredients.Size = new Size(457, 132);
            txtIngredients.TabIndex = 4;
            // 
            // lblInstructions
            // 
            lblInstructions.AutoSize = true;
            lblInstructions.Location = new Point(14, 260);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(87, 20);
            lblInstructions.TabIndex = 5;
            lblInstructions.Text = "Instructions:";
            // 
            // rtbInstructions
            // 
            rtbInstructions.Location = new Point(114, 308);
            rtbInstructions.Margin = new Padding(3, 4, 3, 4);
            rtbInstructions.Name = "rtbInstructions";
            rtbInstructions.Size = new Size(457, 199);
            rtbInstructions.TabIndex = 7;
            rtbInstructions.Text = "";
            // 
            // panelFormatting
            // 
            panelFormatting.Controls.Add(btnBold);
            panelFormatting.Controls.Add(btnItalic);
            panelFormatting.Controls.Add(btnUnderline);
            panelFormatting.Location = new Point(114, 260);
            panelFormatting.Margin = new Padding(3, 4, 3, 4);
            panelFormatting.Name = "panelFormatting";
            panelFormatting.Size = new Size(457, 40);
            panelFormatting.TabIndex = 6;
            // 
            // btnBold
            // 
            btnBold.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBold.Location = new Point(3, 4);
            btnBold.Margin = new Padding(3, 4, 3, 4);
            btnBold.Name = "btnBold";
            btnBold.Size = new Size(46, 31);
            btnBold.TabIndex = 0;
            btnBold.Text = "B";
            btnBold.UseVisualStyleBackColor = true;
            btnBold.Click += btnBold_Click;
            // 
            // btnItalic
            // 
            btnItalic.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            btnItalic.Location = new Point(56, 4);
            btnItalic.Margin = new Padding(3, 4, 3, 4);
            btnItalic.Name = "btnItalic";
            btnItalic.Size = new Size(46, 31);
            btnItalic.TabIndex = 1;
            btnItalic.Text = "I";
            btnItalic.UseVisualStyleBackColor = true;
            btnItalic.Click += btnItalic_Click;
            // 
            // btnUnderline
            // 
            btnUnderline.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            btnUnderline.Location = new Point(109, 4);
            btnUnderline.Margin = new Padding(3, 4, 3, 4);
            btnUnderline.Name = "btnUnderline";
            btnUnderline.Size = new Size(46, 31);
            btnUnderline.TabIndex = 2;
            btnUnderline.Text = "U";
            btnUnderline.UseVisualStyleBackColor = true;
            btnUnderline.Click += btnUnderline_Click;
            // 
            // lblPrepTime
            // 
            lblPrepTime.AutoSize = true;
            lblPrepTime.Location = new Point(594, 20);
            lblPrepTime.Name = "lblPrepTime";
            lblPrepTime.Size = new Size(118, 20);
            lblPrepTime.TabIndex = 8;
            lblPrepTime.Text = "Prep Time (min):";
            // 
            // numPrepTime
            // 
            numPrepTime.Location = new Point(709, 16);
            numPrepTime.Margin = new Padding(3, 4, 3, 4);
            numPrepTime.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numPrepTime.Name = "numPrepTime";
            numPrepTime.Size = new Size(91, 27);
            numPrepTime.TabIndex = 9;
            // 
            // lblCookTime
            // 
            lblCookTime.AutoSize = true;
            lblCookTime.Location = new Point(594, 60);
            lblCookTime.Name = "lblCookTime";
            lblCookTime.Size = new Size(122, 20);
            lblCookTime.TabIndex = 10;
            lblCookTime.Text = "Cook Time (min):";
            // 
            // numCookTime
            // 
            numCookTime.Location = new Point(709, 56);
            numCookTime.Margin = new Padding(3, 4, 3, 4);
            numCookTime.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numCookTime.Name = "numCookTime";
            numCookTime.Size = new Size(91, 27);
            numCookTime.TabIndex = 11;
            // 
            // lblServings
            // 
            lblServings.AutoSize = true;
            lblServings.Location = new Point(594, 100);
            lblServings.Name = "lblServings";
            lblServings.Size = new Size(67, 20);
            lblServings.TabIndex = 12;
            lblServings.Text = "Servings:";
            // 
            // numServings
            // 
            numServings.Location = new Point(709, 96);
            numServings.Margin = new Padding(3, 4, 3, 4);
            numServings.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numServings.Name = "numServings";
            numServings.Size = new Size(91, 27);
            numServings.TabIndex = 13;
            numServings.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // chkScheduled
            // 
            chkScheduled.AutoSize = true;
            chkScheduled.Location = new Point(594, 153);
            chkScheduled.Margin = new Padding(3, 4, 3, 4);
            chkScheduled.Name = "chkScheduled";
            chkScheduled.Size = new Size(130, 24);
            chkScheduled.TabIndex = 14;
            chkScheduled.Text = "Schedule Date:";
            chkScheduled.UseVisualStyleBackColor = true;
            chkScheduled.CheckedChanged += chkScheduled_CheckedChanged;
            // 
            // dtpScheduled
            // 
            dtpScheduled.Location = new Point(594, 187);
            dtpScheduled.Margin = new Padding(3, 4, 3, 4);
            dtpScheduled.Name = "dtpScheduled";
            dtpScheduled.Size = new Size(205, 27);
            dtpScheduled.TabIndex = 15;
            dtpScheduled.ValueChanged += dtpScheduled_ValueChanged;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(14, 527);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(51, 20);
            lblNotes.TabIndex = 16;
            lblNotes.Text = "Notes:";
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(114, 523);
            txtNotes.Margin = new Padding(3, 4, 3, 4);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(457, 79);
            txtNotes.TabIndex = 17;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(594, 523);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(103, 40);
            btnSave.TabIndex = 18;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(704, 523);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(103, 40);
            btnCancel.TabIndex = 19;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(594, 571);
            btnExport.Margin = new Padding(3, 4, 3, 4);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(213, 40);
            btnExport.TabIndex = 20;
            btnExport.Text = "Export Recipe";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // RecipeEditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(823, 627);
            Controls.Add(btnExport);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtNotes);
            Controls.Add(lblNotes);
            Controls.Add(dtpScheduled);
            Controls.Add(chkScheduled);
            Controls.Add(numServings);
            Controls.Add(lblServings);
            Controls.Add(numCookTime);
            Controls.Add(lblCookTime);
            Controls.Add(numPrepTime);
            Controls.Add(lblPrepTime);
            Controls.Add(rtbInstructions);
            Controls.Add(panelFormatting);
            Controls.Add(lblInstructions);
            Controls.Add(txtIngredients);
            Controls.Add(lblIngredients);
            Controls.Add(categorySelector);
            Controls.Add(txtTitle);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RecipeEditorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recipe Editor";
            panelFormatting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numPrepTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCookTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)numServings).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
