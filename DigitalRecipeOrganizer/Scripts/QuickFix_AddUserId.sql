-- Quick Fix: Add UserId column to existing Recipes table
-- Run this script in SQL Server Management Studio or Visual Studio

USE DigitalRecipeOrganizer;
GO

PRINT 'Starting quick fix for UserId column...';
GO

-- Check if UserId column exists
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID('Recipes') 
               AND name = 'UserId')
BEGIN
    PRINT 'Adding UserId column to Recipes table...';
    
    -- Check if we have any users
    IF NOT EXISTS (SELECT * FROM Users)
    BEGIN
        PRINT 'ERROR: No users found in database!';
        PRINT 'Please create a user first by registering through the application.';
        PRINT 'Or run the full MigrateToAuthentication.sql script to create a default admin user.';
        RETURN;
    END
    
    -- Get the first user's ID as default
    DECLARE @DefaultUserId INT = (SELECT TOP 1 UserId FROM Users ORDER BY UserId);
    
    PRINT 'Default User ID: ' + CAST(@DefaultUserId AS NVARCHAR(10));
    
    -- Add the column (nullable first)
    ALTER TABLE Recipes ADD UserId INT NULL;
    PRINT 'Column added (nullable).';
    
    -- Assign all existing recipes to the first user
    UPDATE Recipes SET UserId = @DefaultUserId WHERE UserId IS NULL;
    PRINT 'Existing recipes assigned to user.';
    
    -- Make the column required
    ALTER TABLE Recipes ALTER COLUMN UserId INT NOT NULL;
    PRINT 'Column set to NOT NULL.';
    
    -- Add foreign key constraint if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
                   WHERE name = 'FK_Recipes_Users' 
                   AND parent_object_id = OBJECT_ID('Recipes'))
    BEGIN
        ALTER TABLE Recipes
        ADD CONSTRAINT FK_Recipes_Users
        FOREIGN KEY (UserId) REFERENCES Users(UserId)
        ON DELETE NO ACTION;
        PRINT 'Foreign key constraint added.';
    END
    
    PRINT '? SUCCESS! UserId column added to Recipes table.';
    PRINT 'Recipe count: ' + CAST((SELECT COUNT(*) FROM Recipes) AS NVARCHAR(10));
    PRINT 'All recipes assigned to user: ' + (SELECT Username FROM Users WHERE UserId = @DefaultUserId);
END
ELSE
BEGIN
    PRINT 'UserId column already exists in Recipes table.';
    PRINT 'No changes needed.';
END
GO
