using DigitalRecipeOrganizer.Models;

namespace DigitalRecipeOrganizer.Forms
{
    public partial class ReminderForm : Form
    {
        public ReminderForm(List<Recipe> recipes)
        {
            InitializeComponent();
            LoadReminders(recipes);
        }

        private void LoadReminders(List<Recipe> recipes)
        {
            if (recipes == null || !recipes.Any())
            {
                lblNoReminders.Visible = true;
                panelReminders.Visible = false;
                return;
            }

            lblNoReminders.Visible = false;
            panelReminders.Visible = true;

            var today = DateTime.Today;
            var todayRecipes = recipes.Where(r => r.ScheduledDate!.Value.Date == today).ToList();
            var upcomingRecipes = recipes.Where(r => r.ScheduledDate!.Value.Date > today).ToList();

            int yPosition = 10;

            // Today's recipes
            if (todayRecipes.Any())
            {
                var lblToday = new Label
                {
                    Text = "SCHEDULED FOR TODAY",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(231, 76, 60),
                    Location = new Point(10, yPosition),
                    AutoSize = true
                };
                panelReminders.Controls.Add(lblToday);
                yPosition += 35;

                foreach (var recipe in todayRecipes)
                {
                    AddRecipePanel(recipe, "Today", yPosition);
                    yPosition += 110;
                }

                yPosition += 10;
            }

            // Upcoming recipes
            if (upcomingRecipes.Any())
            {
                var lblUpcoming = new Label
                {
                    Text = "UPCOMING RECIPES",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(52, 152, 219),
                    Location = new Point(10, yPosition),
                    AutoSize = true
                };
                panelReminders.Controls.Add(lblUpcoming);
                yPosition += 35;

                foreach (var recipe in upcomingRecipes.Take(5))
                {
                    var daysUntil = (recipe.ScheduledDate!.Value.Date - today).Days;
                    string when = daysUntil == 1 ? "Tomorrow" : $"In {daysUntil} days";
                    
                    AddRecipePanel(recipe, when, yPosition);
                    yPosition += 110;
                }
            }
        }

        private void AddRecipePanel(Recipe recipe, string when, int yPosition)
        {
            var recipePanel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(550, 95),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Title
            var lblTitle = new Label
            {
                Text = recipe.Title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 5),
                AutoSize = true,
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            // When
            var lblWhen = new Label
            {
                Text = when,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Location = new Point(10, 28),
                AutoSize = true,
                ForeColor = Color.FromArgb(127, 140, 141)
            };

            // Category
            var lblCategory = new Label
            {
                Text = $"Category: {recipe.Category}",
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, 48),
                AutoSize = true
            };

            // Time info
            var lblTime = new Label
            {
                Text = $"Prep: {recipe.PrepTimeMinutes} min | Cook: {recipe.CookTimeMinutes} min",
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, 68),
                AutoSize = true,
                ForeColor = Color.FromArgb(41, 128, 185)
            };

            // Servings
            var lblServings = new Label
            {
                Text = $"Servings: {recipe.Servings}",
                Font = new Font("Segoe UI", 9),
                Location = new Point(300, 68),
                AutoSize = true,
                ForeColor = Color.FromArgb(39, 174, 96)
            };

            recipePanel.Controls.AddRange(new Control[] { lblTitle, lblWhen, lblCategory, lblTime, lblServings });
            panelReminders.Controls.Add(recipePanel);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
