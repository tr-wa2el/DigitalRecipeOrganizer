-- Script to recreate the database with authentication support
-- WARNING: This will delete all existing data!

USE master;
GO

-- Drop the database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'DigitalRecipeOrganizer')
BEGIN
    ALTER DATABASE DigitalRecipeOrganizer SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DigitalRecipeOrganizer;
END
GO

-- The application will recreate it automatically on next run
PRINT 'Database dropped. Run the application to recreate it with the new schema.';
GO
