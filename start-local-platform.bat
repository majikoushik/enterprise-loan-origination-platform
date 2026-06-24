@echo off
setlocal

set "REPO_ROOT=%~dp0"
cd /d "%REPO_ROOT%"

title Enterprise Loan Origination Platform Launcher

echo.
echo Enterprise Loan Origination Platform
echo Starting backend services with dotnet run and Angular with npm start.
echo.

where dotnet >nul 2>&1
if errorlevel 1 (
    echo ERROR: dotnet was not found on PATH.
    echo Install the .NET SDK, then run this file again.
    echo.
    pause
    exit /b 1
)

where npm >nul 2>&1
if errorlevel 1 (
    echo ERROR: npm was not found on PATH.
    echo Install Node.js and npm, then run this file again.
    echo.
    pause
    exit /b 1
)

echo This launcher uses SQL Server LocalDB from the service appsettings files.
echo If a backend window closes on startup, confirm SQL Server LocalDB is installed.
echo.
echo Frontend: http://localhost:4200
echo Customer API Swagger: http://localhost:7101/swagger
echo Loan Application API Swagger: http://localhost:7102/swagger
echo Eligibility API Swagger: http://localhost:7103/swagger
echo Notification API Swagger: http://localhost:5004/swagger
echo Audit API Swagger: http://localhost:5005/swagger
echo.

start "Audit API - 5005" /D "%REPO_ROOT%" cmd /k "set ASPNETCORE_ENVIRONMENT=Development&& dotnet run --project src\services\Audit.Api\Audit.Api.csproj --urls http://localhost:5005"
start "Customer API - 7101" /D "%REPO_ROOT%" cmd /k "set ASPNETCORE_ENVIRONMENT=Development&& dotnet run --project src\services\Customer.Api\Customer.Api.csproj --urls http://localhost:7101"
start "Notification Worker API - 5004" /D "%REPO_ROOT%" cmd /k "set ASPNETCORE_ENVIRONMENT=Development&& dotnet run --project src\services\Notification.Worker\Notification.Worker.csproj --urls http://localhost:5004"
start "Loan Application API - 7102" /D "%REPO_ROOT%" cmd /k "set ASPNETCORE_ENVIRONMENT=Development&& dotnet run --project src\services\LoanApplication.Api\LoanApplication.Api.csproj --urls http://localhost:7102"
start "Eligibility API - 7103" /D "%REPO_ROOT%" cmd /k "set ASPNETCORE_ENVIRONMENT=Development&& dotnet run --project src\services\Eligibility.Api\Eligibility.Api.csproj --urls http://localhost:7103"

if not exist "src\web\loan-portal-angular\node_modules" (
    echo Angular node_modules not found. Installing frontend packages before startup.
    start "Angular Frontend - 4200" /D "%REPO_ROOT%\src\web\loan-portal-angular" cmd /k "npm install&& npm start"
) else (
    start "Angular Frontend - 4200" /D "%REPO_ROOT%\src\web\loan-portal-angular" cmd /k "npm start"
)

timeout /t 8 /nobreak >nul
start "" "http://localhost:4200"

echo Startup command launched.
echo Close each service terminal or press Ctrl+C in it to stop that process.
echo.
pause
