using DigitalRecipeOrganizer.Utilities;
using DigitalRecipeOrganizer.Forms;
using DigitalRecipeOrganizer.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalRecipeOrganizer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Setup global exception handlers
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            try
            {
                // Check and migrate database if needed
                CheckAndMigrateDatabase();

                // Show login form first
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // User logged in successfully, show main form
                        Application.Run(new Form1());
                    }
                    else
                    {
                        // User cancelled login, exit application
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "Application startup");
                MessageBox.Show(
                    $"A critical error occurred:\n\n{ex.Message}\n\nPlease check the error log for details.",
                    "Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void CheckAndMigrateDatabase()
        {
            try
            {
                using var context = new RecipeDbContext();
                
                // Ensure database exists
                context.Database.EnsureCreated();

                // Check if migration is needed (Users table exists but Recipes doesn't have UserId)
                var usersTableExists = context.Database.CanConnect() && 
                                      context.Users.Any();

                if (usersTableExists)
                {
                    // Try to access recipes with UserId
                    try
                    {
                        _ = context.Recipes.FirstOrDefault();
                    }
                    catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Message.Contains("Invalid column name 'UserId'"))
                    {
                        // Migration needed
                        var result = MessageBox.Show(
                            "Database update required!\n\n" +
                            "The database needs to be updated to support user authentication.\n" +
                            "This will assign all existing recipes to your user account.\n\n" +
                            "Would you like to update the database now?",
                            "Database Update Required",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            var connectionString = context.Database.GetConnectionString();
                            var migrationTask = DatabaseMigrator.MigrateToAuthenticationAsync(connectionString!);
                            migrationTask.Wait();

                            if (migrationTask.Result)
                            {
                                MessageBox.Show(
                                    "Database updated successfully!\n\nPlease restart the application.",
                                    "Success",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                Application.Exit();
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                "Database update cancelled.\n\n" +
                                "You can manually run the migration script:\n" +
                                "Scripts/QuickFix_AddUserId.sql",
                                "Update Cancelled",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            Application.Exit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "Database Migration Check");
                // Continue anyway, let the app handle database errors
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ErrorLogger.LogError(e.Exception, "UI Thread Exception");
            MessageBox.Show(
                $"An error occurred:\n\n{e.Exception.Message}\n\nThe application will continue running.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ErrorLogger.LogError(ex, "Unhandled Exception");
                MessageBox.Show(
                    $"A critical error occurred:\n\n{ex.Message}\n\nThe application will now close.",
                    "Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}