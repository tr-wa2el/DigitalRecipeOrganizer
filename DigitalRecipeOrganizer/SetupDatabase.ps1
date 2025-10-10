# Database Setup Script for Digital Recipe Organizer

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Digital Recipe Organizer - Database Setup" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Check if dotnet ef tools are installed
Write-Host "Checking for Entity Framework tools..." -ForegroundColor Yellow
$efTools = dotnet tool list --global | Select-String "dotnet-ef"

if (-not $efTools) {
    Write-Host "Installing Entity Framework tools..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    Write-Host "Entity Framework tools installed successfully!" -ForegroundColor Green
} else {
    Write-Host "Entity Framework tools already installed." -ForegroundColor Green
}

Write-Host ""
Write-Host "Creating database migration..." -ForegroundColor Yellow

# Navigate to project directory
$projectPath = "DigitalRecipeOrganizer"

# Create migration
dotnet ef migrations add InitialCreate --project $projectPath --startup-project $projectPath

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migration created successfully!" -ForegroundColor Green
    Write-Host ""
    
    # Apply migration to database
    Write-Host "Applying migration to database..." -ForegroundColor Yellow
    dotnet ef database update --project $projectPath --startup-project $projectPath
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database created successfully!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Setup completed! You can now run the application." -ForegroundColor Cyan
    } else {
        Write-Host "Error applying migration. Please check your SQL Server connection." -ForegroundColor Red
        Write-Host "Connection String: Server=localhost\\SQLEXPRESS;Database=DigitalRecipeOrganizer;Trusted_Connection=True;Encrypt=False" -ForegroundColor Yellow
    }
} else {
    Write-Host "Error creating migration. Please check for errors above." -ForegroundColor Red
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
