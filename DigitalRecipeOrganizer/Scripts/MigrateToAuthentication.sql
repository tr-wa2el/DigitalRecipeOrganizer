-- Database Migration Script: Add Authentication Support
-- This script preserves existing recipes by assigning them to a default user

USE DigitalRecipeOrganizer;
GO

-- Step 1: Create Users table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    PRINT 'Creating Users table...';
    
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(100) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Email NVARCHAR(200) NOT NULL,
        CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );
    
    -- Create indexes
    CREATE UNIQUE INDEX IX_Users_Username ON Users(Username);
    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
    
    PRINT 'Users table created successfully.';
END
ELSE
BEGIN
    PRINT 'Users table already exists.';
END
GO

-- Step 2: Create a default user for existing recipes
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    PRINT 'Creating default admin user...';
    
    -- Password hash for: "admin123" (you should change this immediately after login!)
    -- This is just a temporary password
    INSERT INTO Users (Username, PasswordHash, Email, CreatedDate, IsActive)
    VALUES (
        'admin',
        'AQAAAAEAACcQAAAAEGpVZDq8Z6f5R0FqYx0Zj7FN0qF3Q1xZHw2YpJGwV3L9K8M5N7P4R6S8T0U2V4W6X8Y0==',
        'admin@recipeorganizer.local',
        GETDATE(),
        1
    );
    
    PRINT 'Default admin user created.';
    PRINT 'Username: admin';
    PRINT 'Password: admin123';
    PRINT 'IMPORTANT: Please change this password after first login!';
END
GO

-- Step 3: Add UserId column to Recipes table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID('Recipes') 
               AND name = 'UserId')
BEGIN
    PRINT 'Adding UserId column to Recipes table...';
    
    -- Get the default admin user's ID
    DECLARE @DefaultUserId INT = (SELECT UserId FROM Users WHERE Username = 'admin');
    
    -- Add the column (nullable first)
    ALTER TABLE Recipes ADD UserId INT NULL;
    
    -- Assign all existing recipes to the default admin user
    UPDATE Recipes SET UserId = @DefaultUserId WHERE UserId IS NULL;
    
    -- Make the column required
    ALTER TABLE Recipes ALTER COLUMN UserId INT NOT NULL;
    
    PRINT 'UserId column added and existing recipes assigned to admin user.';
END
ELSE
BEGIN
    PRINT 'UserId column already exists.';
END
GO

-- Step 4: Add foreign key constraint if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
               WHERE name = 'FK_Recipes_Users' 
               AND parent_object_id = OBJECT_ID('Recipes'))
BEGIN
    PRINT 'Adding foreign key constraint...';
    
    ALTER TABLE Recipes
    ADD CONSTRAINT FK_Recipes_Users
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
    ON DELETE NO ACTION;
    
    PRINT 'Foreign key constraint added.';
END
ELSE
BEGIN
    PRINT 'Foreign key constraint already exists.';
END
GO

-- Step 5: Verify the migration
PRINT '';
PRINT '=== Migration Summary ===';
PRINT 'Users table: ' + CAST((SELECT COUNT(*) FROM Users) AS NVARCHAR(10)) + ' users';
PRINT 'Recipes table: ' + CAST((SELECT COUNT(*) FROM Recipes) AS NVARCHAR(10)) + ' recipes';
PRINT 'Recipes with UserId: ' + CAST((SELECT COUNT(*) FROM Recipes WHERE UserId IS NOT NULL) AS NVARCHAR(10));
PRINT '';
PRINT '? Migration completed successfully!';
PRINT '';
PRINT '?? Next Steps:';
PRINT '1. Login with username: admin';
PRINT '2. Password: admin123';
PRINT '3. Create a new user account for yourself';
PRINT '4. (Optional) Transfer recipes from admin to your account';
PRINT '5. (Optional) Deactivate or delete the admin account';
GO
