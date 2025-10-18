using DigitalRecipeOrganizer.Data;
using DigitalRecipeOrganizer.Models;
using DigitalRecipeOrganizer.Controls;
using DigitalRecipeOrganizer.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DigitalRecipeOrganizer.Forms
{
    public partial class RecipeEditorForm : Form
    {
        private Recipe? _currentRecipe;
        private readonly RecipeDbContext _dbContext;
        private bool _isNewRecipe;

        public RecipeEditorForm(RecipeDbContext dbContext, Recipe? recipe = null)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _currentRecipe = recipe;
            _isNewRecipe = recipe == null;

            if (_currentRecipe != null)
            {
                LoadRecipe();
            }
        }

        private void LoadRecipe()
        {
            if (_currentRecipe == null) return;

            txtTitle.Text = _currentRecipe.Title;
            categorySelector.SelectedCategory = Enum.Parse<RecipeCategory>(_currentRecipe.Category);
            txtIngredients.Text = _currentRecipe.Ingredients;

            // Handle both RTF and plain text formats
            if (!string.IsNullOrEmpty(_currentRecipe.Instructions) &&
                _currentRecipe.Instructions.StartsWith(@"{\rtf", StringComparison.OrdinalIgnoreCase))
            {
                rtbInstructions.Rtf = _currentRecipe.Instructions;
            }
            else
            {
                rtbInstructions.Text = _currentRecipe.Instructions;
            }

            numPrepTime.Value = _currentRecipe.PrepTimeMinutes;
            numCookTime.Value = _currentRecipe.CookTimeMinutes;
            // Ensure servings is at least 1 (the minimum value for the NumericUpDown control)
            numServings.Value = Math.Max(1, _currentRecipe.Servings);
            txtNotes.Text = _currentRecipe.Notes ?? string.Empty;

            if (_currentRecipe.ScheduledDate.HasValue)
            {
                dtpScheduled.Value = _currentRecipe.ScheduledDate.Value;
                chkScheduled.Checked = true;
            }

            this.Text = $"Edit Recipe: {_currentRecipe.Title}";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateRecipe()) return;

            try
            {
                if (_currentRecipe == null)
                {
                    _currentRecipe = new Recipe();
                }

                _currentRecipe.Title = txtTitle.Text.Trim();
                _currentRecipe.Category = categorySelector.SelectedCategory.ToString();
                _currentRecipe.Ingredients = txtIngredients.Text;
                _currentRecipe.Instructions = rtbInstructions.Rtf;
                _currentRecipe.PrepTimeMinutes = (int)numPrepTime.Value;
                _currentRecipe.CookTimeMinutes = (int)numCookTime.Value;
                _currentRecipe.Servings = (int)numServings.Value;
                _currentRecipe.Notes = txtNotes.Text;
                _currentRecipe.ScheduledDate = chkScheduled.Checked ? dtpScheduled.Value : null;

                if (_isNewRecipe)
                {
                    _currentRecipe.CreatedDate = DateTime.Now;
                    _currentRecipe.UserId = UserSession.CurrentUser!.UserId; // Associate with current user
                    _dbContext.Recipes.Add(_currentRecipe);
                }
                else
                {
                    _currentRecipe.LastModifiedDate = DateTime.Now;
                }

                await _dbContext.SaveChangesAsync();

                MessageBox.Show("Recipe saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving recipe: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateRecipe()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a recipe title.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtIngredients.Text))
            {
                MessageBox.Show("Please enter ingredients.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIngredients.Focus();
                return false;
            }

            return true;
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            if (rtbInstructions.SelectionFont != null)
            {
                FontStyle style = rtbInstructions.SelectionFont.Style ^ FontStyle.Bold;
                rtbInstructions.SelectionFont = new Font(rtbInstructions.SelectionFont, style);
            }
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            if (rtbInstructions.SelectionFont != null)
            {
                FontStyle style = rtbInstructions.SelectionFont.Style ^ FontStyle.Italic;
                rtbInstructions.SelectionFont = new Font(rtbInstructions.SelectionFont, style);
            }
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            if (rtbInstructions.SelectionFont != null)
            {
                FontStyle style = rtbInstructions.SelectionFont.Style ^ FontStyle.Underline;
                rtbInstructions.SelectionFont = new Font(rtbInstructions.SelectionFont, style);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files (*.*)|*.*",
                DefaultExt = "txt",
                FileName = txtTitle.Text.Trim() + ".txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string content = $"Recipe: {txtTitle.Text}\n";
                    content += $"Category: {categorySelector.SelectedCategory}\n";
                    content += $"Prep Time: {numPrepTime.Value} minutes\n";
                    content += $"Cook Time: {numCookTime.Value} minutes\n";
                    content += $"Servings: {numServings.Value}\n\n";
                    content += $"INGREDIENTS:\n{txtIngredients.Text}\n\n";
                    content += $"INSTRUCTIONS:\n{rtbInstructions.Text}\n";

                    if (!string.IsNullOrEmpty(txtNotes.Text))
                    {
                        content += $"\nNOTES:\n{txtNotes.Text}";
                    }

                    File.WriteAllText(sfd.FileName, content);

                    MessageBox.Show("Recipe exported successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting recipe: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dtpScheduled_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chkScheduled_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
