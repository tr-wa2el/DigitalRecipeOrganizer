-- Digital Recipe Organizer Database Setup Script
-- Run this script in SQL Server Management Studio or Azure Data Studio

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DigitalRecipeOrganizer')
BEGIN
    CREATE DATABASE DigitalRecipeOrganizer;
    PRINT 'Database DigitalRecipeOrganizer created successfully.';
END
ELSE
BEGIN
    PRINT 'Database DigitalRecipeOrganizer already exists.';
END
GO

USE DigitalRecipeOrganizer;
GO

-- Create Recipes table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Recipes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Recipes] (
        [RecipeId] INT IDENTITY(1,1) PRIMARY KEY,
        [Title] NVARCHAR(200) NOT NULL,
        [Category] NVARCHAR(50) NOT NULL,
        [Ingredients] NVARCHAR(MAX) NOT NULL,
        [Instructions] NVARCHAR(MAX) NOT NULL,
        [PrepTimeMinutes] INT NOT NULL DEFAULT 0,
        [CookTimeMinutes] INT NOT NULL DEFAULT 0,
        [Servings] INT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [LastModifiedDate] DATETIME2 NULL,
        [ScheduledDate] DATETIME2 NULL,
        [Notes] NVARCHAR(MAX) NULL
    );
    
    -- Create indexes for better performance
    CREATE INDEX IX_Recipes_Category ON [dbo].[Recipes] ([Category]);
    CREATE INDEX IX_Recipes_Title ON [dbo].[Recipes] ([Title]);
    CREATE INDEX IX_Recipes_ScheduledDate ON [dbo].[Recipes] ([ScheduledDate]);
    
    PRINT 'Recipes table created successfully.';
END
ELSE
BEGIN
    PRINT 'Recipes table already exists.';
END
GO

-- Create Users table (for future authentication feature)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserId] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(100) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(MAX) NOT NULL,
        [Email] NVARCHAR(200) NOT NULL UNIQUE,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [IsActive] BIT NOT NULL DEFAULT 1
    );
    
    -- Create unique indexes
    CREATE UNIQUE INDEX IX_Users_Username ON [dbo].[Users] ([Username]);
    CREATE UNIQUE INDEX IX_Users_Email ON [dbo].[Users] ([Email]);
    
    PRINT 'Users table created successfully.';
END
ELSE
BEGIN
    PRINT 'Users table already exists.';
END
GO

-- Insert sample recipes (optional)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Recipes])
BEGIN
    INSERT INTO [dbo].[Recipes] 
        ([Title], [Category], [Ingredients], [Instructions], [PrepTimeMinutes], [CookTimeMinutes], [Servings], [Notes], [CreatedDate])
    VALUES
        (
            'Classic Chocolate Chip Cookies',
            'Dessert',
            '2 1/4 cups all-purpose flour
1 tsp baking soda
1 tsp salt
1 cup butter, softened
3/4 cup granulated sugar
3/4 cup packed brown sugar
2 large eggs
2 tsp vanilla extract
2 cups chocolate chips',
            'Preheat oven to 375°F. Mix flour, baking soda and salt in bowl. Beat butter and sugars until creamy. Add eggs and vanilla. Gradually blend in flour mixture. Stir in chocolate chips. Bake 9-11 minutes.',
            15,
            10,
            48,
            'Makes about 4 dozen cookies. Can freeze dough for later use.',
            DATEADD(day, -30, GETDATE())
        ),
        (
            'Spaghetti Carbonara',
            'MainCourse',
            '400g spaghetti
200g pancetta or bacon, diced
4 large eggs
100g Parmesan cheese, grated
2 cloves garlic, minced
Salt and black pepper to taste
Fresh parsley for garnish',
            'Cook spaghetti according to package directions. While pasta cooks, fry pancetta until crispy. Beat eggs with Parmesan cheese. Drain pasta, reserving 1 cup pasta water. Toss hot pasta with pancetta and garlic. Remove from heat and quickly stir in egg mixture. Add pasta water as needed for creamy consistency. Season with salt and pepper, garnish with parsley.',
            10,
            15,
            4,
            'Use freshly grated Parmesan for best results. Work quickly when adding eggs to prevent scrambling.',
            DATEADD(day, -25, GETDATE())
        ),
        (
            'Fresh Fruit Smoothie',
            'Beverage',
            '1 banana
1 cup frozen mixed berries
1 cup Greek yogurt
1/2 cup orange juice
1 tbsp honey
1/2 cup ice cubes
Fresh mint leaves (optional)',
            'Add all ingredients to blender. Blend on high until smooth. Taste and adjust sweetness with honey if needed. Pour into glasses and serve immediately. Garnish with fresh mint if desired.',
            5,
            0,
            2,
            'Can substitute almond milk for a dairy-free version. Add protein powder for post-workout nutrition.',
            DATEADD(day, -20, GETDATE())
        );
    
    PRINT 'Sample recipes inserted successfully.';
END
GO

-- Verify tables were created
SELECT 
    t.name AS TableName,
    COUNT(c.column_id) AS ColumnCount
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE t.name IN ('Recipes', 'Users')
GROUP BY t.name;
GO

PRINT 'Database setup completed successfully!';
