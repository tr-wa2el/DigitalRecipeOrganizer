# Digital Recipe Organizer

A comprehensive Windows Forms application for managing your recipes with Entity Framework Code-First approach.

## Features

### Core Functionality
- **Multi-Document Interface (MDI)**: Main dashboard with recipe editor child windows
- **Recipe Management**: Create, edit, and delete recipes with rich formatting
- **Custom Category Control**: Select from predefined categories (Dessert, Main Course, Beverages, etc.)
- **Rich Text Editing**: Format recipe instructions with Bold, Italic, and Underline
- **DataGridView Display**: View all recipes in a structured list with sortable columns
- **Search & Filter**: Search recipes by name, ingredient, or category
- **Meal Planning**: Schedule recipes with DateTimePicker for meal prep reminders
- **Import/Export**: Save recipes to TXT files and import existing recipes
- **MenuStrip Navigation**: Easy access to all recipe management features

### Database Schema
The application uses SQL Server with the following entities:
- **Recipe**: Stores recipe information (title, category, ingredients, instructions, prep/cook time, servings, notes, scheduled date)
- **User**: Stores user authentication information (optional feature)

### Technical Stack
- **.NET 9.0** (Windows Forms)
- **Entity Framework Core 9.0.9**
- **SQL Server Express**
- **Code-First Approach**

## Setup Instructions

### Prerequisites
- Visual Studio 2022 or later
- SQL Server Express (LocalDB)
- .NET 9.0 SDK

### Database Setup

1. **Create the Database**
   
   The application will automatically create the database on first run using `EnsureCreated()`. However, for production use, it's recommended to use migrations:

   Open Package Manager Console in Visual Studio and run:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

2. **Connection String**
   
   The application uses the following connection string (configured in `RecipeDbContext.cs`):
   ```
   Server=localhost\\SQLEXPRESS;Database=DigitalRecipeOrganizer;Trusted_Connection=True;Encrypt=False
   ```

   If your SQL Server instance is different, update the connection string in `Data/RecipeDbContext.cs`.

### Running the Application

1. Open the solution in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5)

## Usage Guide

### Creating a New Recipe
1. Click **File > New Recipe** (or press Ctrl+N)
2. Fill in the recipe details:
   - Title
   - Category (using the custom category selector)
   - Ingredients
   - Instructions (with formatting toolbar)
   - Prep time, cook time, and servings
   - Optional notes
   - Optional scheduled date for meal planning
3. Click **Save**

### Editing a Recipe
1. Double-click a recipe in the list, or
2. Select a recipe and click **File > Edit Recipe** (Ctrl+E)
3. Make your changes and click **Save**

### Searching Recipes
1. Enter search terms in the search box (searches title and ingredients)
2. Select a category filter (optional)
3. Click **Search** or press Enter
4. Click **Clear** to reset the search

### Exporting a Recipe
1. Open a recipe in the editor
2. Click **Export Recipe**
3. Choose a location and filename
4. The recipe will be saved as a formatted text file

### Importing a Recipe
1. Click **Import Recipe** on the main dashboard
2. Select a text file containing recipe information
3. The recipe will be added to your collection

### Meal Planning
1. When creating or editing a recipe, check "Schedule Date"
2. Select a date using the DateTimePicker
3. The scheduled date will appear in the recipe list

## Project Structure

```
DigitalRecipeOrganizer/
?
??? Models/
?   ??? Recipe.cs           # Recipe entity
?   ??? User.cs             # User entity (for future authentication)
?
??? Data/
?   ??? RecipeDbContext.cs  # EF Core DbContext
?
??? Forms/
?   ??? RecipeEditorForm.cs         # Recipe editor dialog
?   ??? RecipeEditorForm.Designer.cs
?
??? Controls/
?   ??? CategorySelector.cs         # Custom category control
?   ??? CategorySelector.Designer.cs
?
??? Form1.cs                # Main MDI dashboard
??? Form1.Designer.cs
??? Program.cs              # Application entry point
```

## Database Migrations (Optional)

If you want to use Entity Framework migrations instead of `EnsureCreated()`:

1. Remove or comment out the `EnsureCreated()` line in `Form1.cs` constructor
2. Run these commands in Package Manager Console:

```powershell
# Create initial migration
Add-Migration InitialCreate

# Apply migration to database
Update-Database

# Add future migrations
Add-Migration [MigrationName]
Update-Database
```

## Keyboard Shortcuts

- **Ctrl+N**: New Recipe
- **Ctrl+E**: Edit Recipe
- **Delete**: Delete Selected Recipe
- **Alt+F4**: Exit Application
- **Enter**: Search (when search box is focused)

## Future Enhancements

Potential features to add:
- User authentication and personal recipe collections
- Recipe ratings and reviews
- Photo upload for recipes
- Print functionality
- Share recipes via email
- Nutritional information calculator
- Shopping list generator
- Recipe tags and advanced filtering
- Recipe import from popular websites

## Troubleshooting

### Database Connection Issues
- Verify SQL Server Express is installed and running
- Check the connection string in `RecipeDbContext.cs`
- Ensure Windows Authentication is enabled for SQL Server

### Build Errors
- Ensure all NuGet packages are restored
- Clean and rebuild the solution
- Check that .NET 9.0 SDK is installed

## License

This project is created for educational purposes.

## Support

For issues or questions, please refer to the project documentation or contact the development team.
