# Quick Start Guide - Digital Recipe Organizer

## Getting Started in 3 Steps

### Step 1: Verify Prerequisites
- ? Visual Studio 2022 or later installed
- ? SQL Server Express LocalDB installed (comes with Visual Studio)
- ? .NET 9.0 SDK installed

### Step 2: Setup Database

Choose one of these options:

#### Option A: Automatic Setup (Recommended)
1. Simply run the application (F5 in Visual Studio)
2. The database will be created automatically
3. You'll be prompted to load sample recipes (optional)

#### Option B: Using PowerShell Script
1. Right-click `SetupDatabase.ps1` in Solution Explorer
2. Select "Open With" > "PowerShell"
3. Follow the prompts

#### Option C: Manual SQL Script
1. Open SQL Server Management Studio or Azure Data Studio
2. Connect to `localhost\SQLEXPRESS`
3. Open `DatabaseSetup.sql`
4. Execute the script

### Step 3: Run the Application
1. Press F5 in Visual Studio
2. The main dashboard will open
3. Start creating your recipes!

## First-Time Usage

### Creating Your First Recipe
1. Click **File > New Recipe** or press `Ctrl+N`
2. Enter recipe details:
   - **Title**: "My First Recipe"
   - **Category**: Select from dropdown
   - **Ingredients**: List ingredients (one per line recommended)
   - **Instructions**: Use the formatting toolbar (B/I/U buttons)
   - **Times**: Enter prep and cook times
   - **Servings**: Number of servings
3. Click **Save**

### Exploring Features

#### Search & Filter
- Type keywords in the search box
- Select a category filter
- Click **Search** or press Enter
- Results appear instantly in the grid

#### Edit a Recipe
- Double-click any recipe in the list
- Or select and press `Ctrl+E`
- Make changes and save

#### Schedule Meals
- When editing a recipe, check "Schedule Date"
- Pick a date for meal planning
- Scheduled recipes show in the main list

#### Export Recipes
- Open a recipe in the editor
- Click **Export Recipe**
- Save as .txt file
- Share with friends!

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+N` | New Recipe |
| `Ctrl+E` | Edit Recipe |
| `Delete` | Delete Selected Recipe |
| `Alt+F4` | Exit Application |
| `Enter` | Search (in search box) |

## Tips & Tricks

### ?? Formatting Instructions
Use the toolbar buttons while typing instructions:
- **B** = Bold text
- **I** = Italic text
- **U** = Underline text

### ?? Smart Search
Search works across:
- Recipe titles
- Ingredients
- Categories

### ?? Meal Planning
- Schedule recipes for specific dates
- Plan your week's meals in advance
- Filter by scheduled date

### ?? Organizing Recipes
Use categories effectively:
- **MainCourse**: Dinner entrées
- **Dessert**: Sweet treats
- **Beverage**: Drinks and smoothies
- **Breakfast**: Morning meals
- **Salad**: Fresh salads
- **Soup**: Soups and stews
- **Appetizer**: Starters
- **Snack**: Quick bites
- **Sauce**: Sauces and dressings
- **Other**: Everything else

## Troubleshooting

### Database Connection Error
**Problem**: "Cannot connect to database"
**Solution**: 
1. Check if SQL Server Express is running
2. Verify connection string in `Data/RecipeDbContext.cs`
3. Try running as Administrator

### Application Won't Start
**Problem**: Application crashes on startup
**Solution**:
1. Clean and rebuild solution (`Ctrl+Shift+B`)
2. Delete `bin` and `obj` folders
3. Restore NuGet packages
4. Check .NET 9.0 is installed

### Recipes Not Showing
**Problem**: Recipe list is empty after creating recipes
**Solution**:
1. Click **Clear** in search panel
2. Restart the application
3. Check database has data (run query: `SELECT * FROM Recipes`)

## Sample Data

On first run, you'll be asked if you want sample recipes. These include:
- ?? Classic Chocolate Chip Cookies (Dessert)
- ?? Spaghetti Carbonara (Main Course)
- ?? Fresh Fruit Smoothie (Beverage)
- ?? Caesar Salad (Salad)
- ?? Tomato Basil Soup (Soup)
- ?? Fluffy Pancakes (Breakfast)

## Need Help?

### Documentation
- Full README.md in project root
- Inline code comments
- XML documentation for classes

### Database Schema
```
Recipes
?? RecipeId (Primary Key)
?? Title
?? Category
?? Ingredients
?? Instructions
?? PrepTimeMinutes
?? CookTimeMinutes
?? Servings
?? CreatedDate
?? LastModifiedDate
?? ScheduledDate
?? Notes
```

## Next Steps

Once you're comfortable with basics:
1. Import existing recipes from text files
2. Export favorite recipes to share
3. Schedule your weekly meal plan
4. Organize recipes by category
5. Use search to find recipes by ingredient

---

**Enjoy organizing your recipes! ??????????**
