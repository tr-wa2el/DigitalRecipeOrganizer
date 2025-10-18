using DigitalRecipeOrganizer.Data;
using DigitalRecipeOrganizer.Models;
using DigitalRecipeOrganizer.Forms;
using Microsoft.EntityFrameworkCore;

namespace DigitalRecipeOrganizer.Utilities
{
    /// <summary>
    /// Handles recipe reminders and notifications for scheduled recipes
    /// </summary>
    public class RecipeReminderService
    {
        /// <summary>
        /// Get recipes scheduled for today
        /// </summary>
        public static async Task<List<Recipe>> GetTodayScheduledRecipesAsync(RecipeDbContext dbContext, int userId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await dbContext.Recipes
                .Where(r => r.UserId == userId &&
                           r.ScheduledDate.HasValue &&
                           r.ScheduledDate.Value.Date >= today &&
                           r.ScheduledDate.Value.Date < tomorrow)
                .OrderBy(r => r.ScheduledDate)
                .ToListAsync();
        }

        /// <summary>
        /// Get upcoming recipes (today + next 7 days)
        /// </summary>
        public static async Task<List<Recipe>> GetUpcomingRecipesAsync(RecipeDbContext dbContext, int userId, int daysAhead = 7)
        {
            var today = DateTime.Today;
            var endDate = today.AddDays(daysAhead);

            return await dbContext.Recipes
                .Where(r => r.UserId == userId &&
                           r.ScheduledDate.HasValue &&
                           r.ScheduledDate.Value.Date >= today &&
                           r.ScheduledDate.Value.Date <= endDate)
                .OrderBy(r => r.ScheduledDate)
                .ToListAsync();
        }

        /// <summary>
        /// Show notification for scheduled recipes using custom form
        /// </summary>
        public static void ShowScheduledRecipesNotification(List<Recipe> recipes)
        {
            if (recipes == null || !recipes.Any())
                return;

            using var reminderForm = new ReminderForm(recipes);
            reminderForm.ShowDialog();
        }

        /// <summary>
        /// Show notification for scheduled recipes using simple MessageBox
        /// </summary>
        public static void ShowScheduledRecipesNotificationSimple(List<Recipe> recipes)
        {
            if (recipes == null || !recipes.Any())
                return;

            var today = DateTime.Today;
            var todayRecipes = recipes.Where(r => r.ScheduledDate!.Value.Date == today).ToList();
            var upcomingRecipes = recipes.Where(r => r.ScheduledDate!.Value.Date > today).ToList();

            string message = "";

            // Today's recipes
            if (todayRecipes.Any())
            {
                message += ">>> SCHEDULED FOR TODAY <<<\n\n";
                foreach (var recipe in todayRecipes)
                {
                    message += $"* {recipe.Title}\n";
                    message += $"  Prep: {recipe.PrepTimeMinutes}min | Cook: {recipe.CookTimeMinutes}min\n";
                    message += $"  Servings: {recipe.Servings}\n";
                    message += $"  Category: {recipe.Category}\n\n";
                }
            }

            // Upcoming recipes
            if (upcomingRecipes.Any())
            {
                if (todayRecipes.Any())
                    message += "\n" + new string('-', 50) + "\n\n";

                message += ">>> UPCOMING RECIPES <<<\n\n";
                foreach (var recipe in upcomingRecipes.Take(5)) // Show max 5 upcoming
                {
                    var daysUntil = (recipe.ScheduledDate!.Value.Date - today).Days;
                    string when = daysUntil == 1 ? "Tomorrow" : $"In {daysUntil} days";
                    
                    message += $"* {recipe.Title} ({when})\n";
                    message += $"  Scheduled: {recipe.ScheduledDate.Value:dddd, MMM dd, yyyy}\n";
                    message += $"  Prep: {recipe.PrepTimeMinutes}min | Cook: {recipe.CookTimeMinutes}min\n\n";
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(
                    message,
                    "Recipe Reminders",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Format recipe details for notification
        /// </summary>
        public static string FormatRecipeNotification(Recipe recipe)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Recipe: {recipe.Title}");
            sb.AppendLine($"Category: {recipe.Category}");
            sb.AppendLine($"Prep Time: {recipe.PrepTimeMinutes} minutes");
            sb.AppendLine($"Cook Time: {recipe.CookTimeMinutes} minutes");
            sb.AppendLine($"Servings: {recipe.Servings}");
            
            if (!string.IsNullOrEmpty(recipe.Notes))
            {
                sb.AppendLine($"Notes: {recipe.Notes}");
            }

            return sb.ToString();
        }
    }
}
