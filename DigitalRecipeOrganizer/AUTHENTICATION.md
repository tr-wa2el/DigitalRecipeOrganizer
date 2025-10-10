# Authentication System - Digital Recipe Organizer

## Overview
The Digital Recipe Organizer now includes a complete user authentication system that secures recipes per user account.

## Features Implemented

### 1. **User Registration**
- New users can create accounts with username, email, and password
- Password validation (minimum 6 characters)
- Email format validation
- Unique username and email enforcement
- Secure password hashing using PBKDF2 with SHA256

### 2. **User Login**
- Username and password authentication
- Show/hide password option
- Session management
- Secure password verification

### 3. **User Session Management**
- Tracks currently logged-in user
- User-specific recipe access
- Logout functionality

### 4. **Recipe Security**
- Each recipe is associated with a user
- Users can only view and edit their own recipes
- User-filtered search and display

## New Files Created

### Forms
- `Forms/LoginForm.cs` - Login interface
- `Forms/LoginForm.Designer.cs` - Login form designer
- `Forms/RegisterForm.cs` - Registration interface
- `Forms/RegisterForm.Designer.cs` - Registration form designer

### Utilities
- `Utilities/PasswordHelper.cs` - Secure password hashing and verification
- `Utilities/UserSession.cs` - User session management

## Updated Files

### Models
- `Models/Recipe.cs` - Added `UserId` foreign key and `User` navigation property
- `Models/User.cs` - Added `Recipes` collection navigation property

### Data Layer
- `Data/RecipeDbContext.cs` - Configured User-Recipe relationship with foreign key constraint

### Application Logic
- `Program.cs` - Shows login form before main application
- `Form1.cs` - Filters recipes by current user, displays username, added logout functionality
- `Forms/RecipeEditorForm.cs` - Associates new recipes with current user
- `Utilities/DatabaseSeeder.cs` - Associates sample recipes with current user

## Security Features

### Password Hashing
- Uses PBKDF2 (Password-Based Key Derivation Function 2)
- SHA256 hash algorithm
- 10,000 iterations
- 16-byte random salt
- 32-byte (256-bit) key size
- Salt and hash stored together in Base64 format

### Database Security
- Passwords are **never** stored in plain text
- Only password hashes are stored in the database
- Unique constraints on username and email
- Foreign key constraints for data integrity
- Restrict delete behavior (cannot delete user if they have recipes)

## Database Schema Changes

### Users Table
```sql
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);
```

### Recipes Table (Updated)
```sql
ALTER TABLE Recipes
ADD UserId INT NOT NULL;

ALTER TABLE Recipes
ADD CONSTRAINT FK_Recipes_Users
FOREIGN KEY (UserId) REFERENCES Users(UserId)
ON DELETE RESTRICT;
```

## Database Migration

### Option 1: Automatic Migration (Recommended for New Installations)
The application will automatically create the database with the new schema on first run.

### Option 2: Manual Migration (For Existing Databases)

**Important:** If you have existing recipes in your database, you need to:

1. **Backup your database first!**

2. **Create a default user:**
```sql
INSERT INTO Users (Username, PasswordHash, Email, CreatedDate, IsActive)
VALUES ('admin', 
        'AQAAAAEAACcQAAAAEDummyHashHere==', 
        'admin@example.com', 
        GETDATE(), 
        1);
```

3. **Update existing recipes:**
```sql
-- Get the UserId of the default user
DECLARE @DefaultUserId INT = (SELECT UserId FROM Users WHERE Username = 'admin');

-- Add UserId column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Recipes') AND name = 'UserId')
BEGIN
    ALTER TABLE Recipes ADD UserId INT;
END

-- Update all existing recipes to belong to default user
UPDATE Recipes SET UserId = @DefaultUserId WHERE UserId IS NULL;

-- Make UserId required
ALTER TABLE Recipes ALTER COLUMN UserId INT NOT NULL;

-- Add foreign key constraint
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Recipes_Users')
BEGIN
    ALTER TABLE Recipes
    ADD CONSTRAINT FK_Recipes_Users
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
    ON DELETE RESTRICT;
END
```

4. **Create a real admin account:** Login to the application and register a new user through the UI (this will create a properly hashed password).

### Option 3: Entity Framework Migration
```powershell
# In Package Manager Console
Add-Migration AddUserAuthentication
Update-Database
```

## How to Use

### First Time Setup
1. **Start the Application** - Login form will appear
2. **Click "Register"** to create a new account
3. **Fill in the registration form:**
   - Username (minimum 3 characters)
   - Email (valid email format)
   - Password (minimum 6 characters)
   - Confirm Password
4. **Click "Register"** - Your account will be created
5. **Login** with your new credentials

### Daily Use
1. **Login** with your username and password
2. **Create recipes** - They are automatically associated with your account
3. **View recipes** - Only your recipes are displayed
4. **Search recipes** - Search is limited to your recipes
5. **Logout** - Click File > Logout when done

### Creating Additional Users
- Multiple users can use the same application
- Each user has their own separate recipe collection
- Recipes are private to each user account

## User Interface Changes

### Login Form
- Clean, modern interface with color-coded header
- Username and password fields
- Show password checkbox
- Register button for new users
- Cancel button to exit

### Main Application
- Title bar now shows: "Digital Recipe Organizer - Welcome, [Username]!"
- New File menu item: "Logout"
- All recipes filtered by current user

### Registration Form
- Username field with validation
- Email field with format validation
- Password field with strength requirement
- Confirm password field
- Show password checkbox
- Visual feedback for validation errors

## Security Best Practices Implemented

1. **Password Storage:** 
   - Never store plain text passwords
   - Use strong, industry-standard hashing (PBKDF2)
   - Individual salt for each password

2. **Input Validation:**
   - Username and email uniqueness
   - Email format validation
   - Password strength requirements
   - Proper error messages without revealing sensitive info

3. **Session Management:**
   - User session cleared on logout
   - No persistent authentication tokens (for security)
   - Requires login each time application starts

4. **Database Integrity:**
   - Foreign key constraints
   - Unique constraints on username/email
   - Restrict delete to prevent orphaned recipes

## Future Enhancements

Potential features for future versions:
- [ ] Password reset functionality
- [ ] Remember me option
- [ ] Password strength indicator
- [ ] User profile management
- [ ] Change password option
- [ ] Email verification
- [ ] Two-factor authentication (2FA)
- [ ] Recipe sharing between users
- [ ] Public vs private recipes
- [ ] Password expiration policies
- [ ] Account lockout after failed attempts
- [ ] Audit logging for security events

## Troubleshooting

### Problem: "Cannot insert NULL value into column 'UserId'"
**Solution:** This means you have existing recipes. Follow the manual migration steps above.

### Problem: "Login failed" even with correct credentials
**Solution:** 
1. Check that the username matches exactly (case-sensitive)
2. Ensure the password is correct
3. Verify the user account is active (`IsActive = true`)

### Problem: "Username already exists"
**Solution:** Choose a different username. Each username must be unique.

### Problem: Cannot see any recipes after login
**Solution:** This is normal for new users. Click File > New Recipe to create your first recipe.

### Problem: Lost password
**Solution:** Currently, there's no password reset feature. You would need to:
1. Register a new account, OR
2. Have a database administrator reset your password manually

## Testing the Authentication

### Test User Accounts
For testing purposes, you can create multiple users:

1. **User 1:**
   - Username: testuser1
   - Email: test1@example.com
   - Password: password123

2. **User 2:**
   - Username: testuser2
   - Email: test2@example.com
   - Password: password123

3. Login with each user and create different recipes
4. Verify that User 1 cannot see User 2's recipes
5. Test logout and login with different users

## Code Examples

### Checking if User is Logged In
```csharp
if (UserSession.IsLoggedIn)
{
    var currentUserId = UserSession.CurrentUser.UserId;
    var username = UserSession.CurrentUser.Username;
}
```

### Filtering Recipes by Current User
```csharp
var userRecipes = await _dbContext.Recipes
    .Where(r => r.UserId == UserSession.CurrentUser.UserId)
    .ToListAsync();
```

### Associating a New Recipe with Current User
```csharp
var newRecipe = new Recipe
{
    Title = "My Recipe",
    UserId = UserSession.CurrentUser.UserId,
    // ... other properties
};
_dbContext.Recipes.Add(newRecipe);
await _dbContext.SaveChangesAsync();
```

## Support

For issues or questions:
1. Check this README first
2. Review the code comments in the authentication files
3. Check the error logs in the application directory
4. Verify database connection string in `RecipeDbContext.cs`

---

**Authentication System Version:** 1.0  
**Last Updated:** 2025  
**Status:** ? Complete and tested
