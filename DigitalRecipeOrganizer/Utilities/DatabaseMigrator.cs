using Microsoft.Data.SqlClient;

namespace DigitalRecipeOrganizer.Utilities
{
    /// <summary>
    /// Helper class to migrate existing database to support authentication
    /// </summary>
    public static class DatabaseMigrator
    {
        public static async Task<bool> MigrateToAuthenticationAsync(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Check if UserId column exists
                var checkColumnSql = @"
                    SELECT COUNT(*) 
                    FROM sys.columns 
                    WHERE object_id = OBJECT_ID('Recipes') 
                    AND name = 'UserId'";

                using (var checkCmd = new SqlCommand(checkColumnSql, connection))
                {
                    var columnExists = (int)await checkCmd.ExecuteScalarAsync() > 0;
                    if (columnExists)
                    {
                        // Column already exists, migration not needed
                        return true;
                    }
                }

                // Check if we have any users
                var checkUsersSql = "SELECT COUNT(*) FROM Users";
                int userCount = 0;
                
                using (var checkUsersCmd = new SqlCommand(checkUsersSql, connection))
                {
                    userCount = (int)await checkUsersCmd.ExecuteScalarAsync();
                }

                if (userCount == 0)
                {
                    // No users exist, cannot migrate
                    MessageBox.Show(
                        "Database migration required!\n\n" +
                        "No users found in the database.\n" +
                        "Please register a new user account first.",
                        "Migration Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

                // Get the first user's ID
                var getUserIdSql = "SELECT TOP 1 UserId FROM Users ORDER BY UserId";
                int defaultUserId;
                
                using (var getUserIdCmd = new SqlCommand(getUserIdSql, connection))
                {
                    defaultUserId = (int)await getUserIdCmd.ExecuteScalarAsync();
                }

                // Start migration
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Add UserId column (nullable first)
                        var addColumnSql = "ALTER TABLE Recipes ADD UserId INT NULL";
                        using (var cmd = new SqlCommand(addColumnSql, connection, transaction))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Assign all existing recipes to the first user
                        var updateRecipesSql = $"UPDATE Recipes SET UserId = {defaultUserId} WHERE UserId IS NULL";
                        using (var cmd = new SqlCommand(updateRecipesSql, connection, transaction))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Make column NOT NULL
                        var alterColumnSql = "ALTER TABLE Recipes ALTER COLUMN UserId INT NOT NULL";
                        using (var cmd = new SqlCommand(alterColumnSql, connection, transaction))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Add foreign key constraint
                        var addFKSql = @"
                            IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Recipes_Users')
                            BEGIN
                                ALTER TABLE Recipes
                                ADD CONSTRAINT FK_Recipes_Users
                                FOREIGN KEY (UserId) REFERENCES Users(UserId)
                                ON DELETE NO ACTION
                            END";
                        using (var cmd = new SqlCommand(addFKSql, connection, transaction))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();
                        
                        MessageBox.Show(
                            "Database migration completed successfully!\n\n" +
                            "All existing recipes have been assigned to the first user.\n" +
                            "Please restart the application.",
                            "Migration Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Database migration failed:\n\n{ex.Message}\n\n" +
                    "Please run the SQL migration script manually:\n" +
                    "Scripts/QuickFix_AddUserId.sql",
                    "Migration Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                ErrorLogger.LogError(ex, "Database Migration");
                return false;
            }
        }
    }
}
