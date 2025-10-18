using DigitalRecipeOrganizer.Data;
using DigitalRecipeOrganizer.Models;
using DigitalRecipeOrganizer.Forms;
using DigitalRecipeOrganizer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using DigitalRecipeOrganizer;

namespace DigitalRecipeOrganizer
{
    public partial class Form1 : Form
    {
        private RecipeDbContext _dbContext;
        private BindingList<Recipe> _recipes;
        private System.Windows.Forms.Timer? _reminderTimer;
        private bool _hasShownTodayReminder = false;

        public Form1()
        {
            InitializeComponent();
            _dbContext = new RecipeDbContext();
            _recipes = new BindingList<Recipe>();
            
            // Ensure database is created
            _dbContext.Database.EnsureCreated();
            
            // Initialize filter combobox
            cmbFilterCategory.SelectedIndex = 0;
            
            // Display logged-in user
            if (UserSession.IsLoggedIn)
            {
                this.Text = $"Digital Recipe Organizer - Welcome, {UserSession.CurrentUser!.Username}!";
            }
            
            // Optionally seed sample data
            SeedSampleDataIfNeeded();
            
            LoadRecipes();
            SetupDataGridView();
            
            // Initialize reminder system
            InitializeReminderSystem();
        }

        /// <summary>
        /// Initialize the recipe reminder system
        /// </summary>
        private void InitializeReminderSystem()
        {
            // Show reminders immediately on startup
            _ = CheckAndShowReminders();

            // Setup timer to check for reminders every hour
            _reminderTimer = new System.Windows.Forms.Timer();
            _reminderTimer.Interval = 3600000; // 1 hour = 3600000 milliseconds
            _reminderTimer.Tick += async (s, e) => await CheckAndShowReminders();
            _reminderTimer.Start();
        }

        /// <summary>
        /// Check for scheduled recipes and show reminders
        /// </summary>
        private async Task CheckAndShowReminders()
        {
            if (!UserSession.IsLoggedIn)
                return;

            try
            {
                // Get upcoming recipes (next 7 days)
                var upcomingRecipes = await RecipeReminderService.GetUpcomingRecipesAsync(
                    _dbContext,
                    UserSession.CurrentUser!.UserId,
                    7);

                if (upcomingRecipes.Any() && !_hasShownTodayReminder)
                {
                    RecipeReminderService.ShowScheduledRecipesNotification(upcomingRecipes);
                    _hasShownTodayReminder = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "Reminder check");
            }
        }

        private async void SeedSampleDataIfNeeded()
        {
            try
            {
                var recipeCount = await _dbContext.Recipes.CountAsync();
                
                if (recipeCount == 0)
                {
                    var result = MessageBox.Show(
                        "Would you like to load sample recipes to get started?",
                        "Welcome to Digital Recipe Organizer",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        await DatabaseSeeder.SeedSampleRecipes(_dbContext);
                        MessageBox.Show(
                            "Sample recipes have been added successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error seeding sample data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void SetupDataGridView()
        {
            dgvRecipes.AutoGenerateColumns = false;
            dgvRecipes.DataSource = _recipes;

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RecipeId",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                HeaderText = "Recipe Name",
                Width = 200
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Category",
                HeaderText = "Category",
                Width = 120
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ingredients",
                HeaderText = "Ingredients",
                Width = 250
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrepTimeMinutes",
                HeaderText = "Prep Time (min)",
                Width = 100
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CookTimeMinutes",
                HeaderText = "Cook Time (min)",
                Width = 100
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Servings",
                HeaderText = "Servings",
                Width = 80
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ScheduledDate",
                HeaderText = "Scheduled",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }
            });

            dgvRecipes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecipes.MultiSelect = false;
            dgvRecipes.AllowUserToAddRows = false;
            dgvRecipes.ReadOnly = true;
        }

        private async void LoadRecipes()
        {
            try
            {
                if (!UserSession.IsLoggedIn)
                    return;

                // Load only recipes for the current user
                var recipes = await _dbContext.Recipes
                    .Where(r => r.UserId == UserSession.CurrentUser!.UserId)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToListAsync();

                _recipes.Clear();
                foreach (var recipe in recipes)
                {
                    _recipes.Add(recipe);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recipes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var editorForm = new RecipeEditorForm(_dbContext);
            if (editorForm.ShowDialog() == DialogResult.OK)
            {
                LoadRecipes();
            }
        }

        private void editRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedRecipe();
        }

        private async void deleteRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvRecipes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a recipe to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRecipe = (Recipe)dgvRecipes.SelectedRows[0].DataBoundItem;

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{selectedRecipe.Title}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _dbContext.Recipes.Remove(selectedRecipe);
                    await _dbContext.SaveChangesAsync();
                    LoadRecipes();
                    MessageBox.Show("Recipe deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting recipe: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UserSession.Logout();
                this.Close();
            }
        }

        private void dgvRecipes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedRecipe();
            }
        }

        private void EditSelectedRecipe()
        {
            if (dgvRecipes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a recipe to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRecipe = (Recipe)dgvRecipes.SelectedRows[0].DataBoundItem;

            using var editorForm = new RecipeEditorForm(_dbContext, selectedRecipe);
            if (editorForm.ShowDialog() == DialogResult.OK)
            {
                LoadRecipes();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch();
                e.Handled = true;
            }
        }

        private async void PerformSearch()
        {
            if (!UserSession.IsLoggedIn)
                return;

            string searchTerm = txtSearch.Text.Trim().ToLower();
            string selectedCategory = cmbFilterCategory.SelectedItem?.ToString() ?? "All";

            try
            {
                // Start with recipes for current user only
                var query = _dbContext.Recipes
                    .Where(r => r.UserId == UserSession.CurrentUser!.UserId);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(r =>
                        r.Title.ToLower().Contains(searchTerm) ||
                        r.Ingredients.ToLower().Contains(searchTerm) ||
                        r.Category.ToLower().Contains(searchTerm));
                }

                if (selectedCategory != "All")
                {
                    query = query.Where(r => r.Category == selectedCategory);
                }

                var results = await query.OrderByDescending(r => r.CreatedDate).ToListAsync();

                _recipes.Clear();
                foreach (var recipe in results)
                {
                    _recipes.Add(recipe);
                }

                lblResultCount.Text = $"Found {results.Count} recipe(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching recipes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterCategory.SelectedIndex = 0;
            LoadRecipes();
            lblResultCount.Text = "";
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show("You must be logged in to import recipes.", "Authentication Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Import Recipe"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string content = File.ReadAllText(ofd.FileName);
                    
                    // Convert plain text to RTF format using a temporary RichTextBox
                    string rtfContent;
                    using (var tempRtb = new RichTextBox())
                    {
                        tempRtb.Text = content;
                        rtfContent = tempRtb.Rtf;
                    }
                    
                    // Simple parsing logic - can be enhanced
                    var recipe = new Recipe
                    {
                        Title = Path.GetFileNameWithoutExtension(ofd.FileName),
                        Category = "Other",
                        Ingredients = content,
                        Instructions = rtfContent,
                        CreatedDate = DateTime.Now,
                        UserId = UserSession.CurrentUser!.UserId  // Associate with current user
                    };

                    _dbContext.Recipes.Add(recipe);
                    await _dbContext.SaveChangesAsync();
                    LoadRecipes();

                    MessageBox.Show("Recipe imported successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing recipe: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Digital Recipe Organizer\n\n" +
                "Version 1.0\n\n" +
                "A comprehensive application for managing your recipes.\n\n" +
                "Features:\n" +
                "- Create and edit recipes\n" +
                "- Categorize recipes\n" +
                "- Search by ingredient or category\n" +
                "- Schedule meal preparation\n" +
                "- Recipe reminders\n" +
                "- Export recipes to files",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// View scheduled recipes and reminders
        /// </summary>
        private async void viewRemindersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsLoggedIn)
                return;

            try
            {
                // Get upcoming recipes (next 7 days)
                var upcomingRecipes = await RecipeReminderService.GetUpcomingRecipesAsync(
                    _dbContext,
                    UserSession.CurrentUser!.UserId,
                    7);

                if (upcomingRecipes.Any())
                {
                    RecipeReminderService.ShowScheduledRecipesNotification(upcomingRecipes);
                }
                else
                {
                    MessageBox.Show(
                        "You have no scheduled recipes for the next 7 days.\n\n" +
                        "To schedule a recipe, edit it and set a scheduled date!",
                        "No Scheduled Recipes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "View Reminders");
                MessageBox.Show($"Error loading reminders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop and dispose the timer
            if (_reminderTimer != null)
            {
                _reminderTimer.Stop();
                _reminderTimer.Dispose();
            }
            
            _dbContext?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
