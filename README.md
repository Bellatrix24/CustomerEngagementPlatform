# Customer Engagement Platform

## Description
A simple ASP.NET Core MVC project for managing customers and support tickets as part of Wipro capstone training.

## Technologies Used
- ASP.NET Core MVC (Web framework)
- ASP.NET Core Identity (Authentication and Authorization)
- Entity Framework Core (ORM)
- SQL Server LocalDB (Database)
- JWT Authentication (API security)
- Swagger/OpenAPI (API documentation)
- Razor Views (Server-side rendering)
- Bootstrap (UI styling)
- jQuery AJAX (Dynamic page updates)
- Docker (Container support)
- GitHub Actions (CI build pipeline)

## Main Features
- Customer registration and login
- Staff login
- Customer portal to view raised tickets
- Raise support tickets with status and priority
- Staff dashboard showing ticket statistics
- Customer CRUD (Staff access only)
- Ticket CRUD (Staff access only)
- Ticket assignment and status tracking
- AJAX ticket filtering on status and priority with real-time search
- JWT-secured APIs for integration
- Swagger API testing interface
- Unit tests for ticket model and logic

## Project Structure
- **CustomerEngagementPlatform**: Main ASP.NET Core MVC project.
  - **Controllers**: MVC controllers for page routing and API controllers.
  - **Models**: Database models (Customer, Ticket, etc.).
  - **Views**: Razor view templates for the user interface.
  - **Data**: DbContext and migrations.
  - **Repositories**: Database access repository classes.
  - **Services**: Business logic classes for handling tickets.
  - **Areas/Identity**: Identity UI pages and login logic.
  - **wwwroot**: Static resources (JS, CSS, images).
  - **Documentation**: Folder containing database scripts.
- **CustomerEngagementPlatform.Tests**: Unit test project containing tests.
- **.github/workflows**: GitHub Actions build pipeline workflows.

## How to Run Locally

### Step 1: Install Prerequisites
Ensure you have the following installed on your machine:
- Visual Studio 2022
- .NET 8 SDK
- SQL Server LocalDB or SQL Server Express

### Step 2: Open the Solution File
Double-click `CustomerEngagementPlatform.slnx` or `CustomerEngagementPlatform.sln` to open it in Visual Studio.

### Step 3: Restore Packages
Open the terminal inside the repository root and run:
```bash
dotnet restore
```

### Step 4: Update Database
Create the database and tables by running migrations. In the Package Manager Console run:
```powershell
Update-Database
```
Or in a standard terminal run:
```bash
dotnet ef database update --project CustomerEngagementPlatform
```

### Step 5: Run the Project
Start the application from terminal:
```bash
dotnet run --project CustomerEngagementPlatform
```
Or press the HTTPS / IIS Express run button in Visual Studio.

### Step 6: Open Browser
Navigate to the URL shown in the terminal output (typically `http://localhost:5009` or `https://localhost:7198`).

## Demo Login Details

### Staff Login:
- **Email:** staff@demo.com
- **Password:** Staff@123

### Customer Login:
- Register a new customer account using the **Register** page link on the top right.

## Swagger API Testing

1. Run the project locally.
2. Open `/swagger` in your browser (e.g. `http://localhost:5009/swagger`).
3. Under the auth section, use `POST /api/auth/login` and input the staff credentials:
   ```json
   {
     "email": "staff@demo.com",
     "password": "Staff@123"
   }
   ```
4. Execute the request and copy the returned JWT token.
5. Click **Authorize** on the top right of the Swagger page.
6. Enter `Bearer <token>` (replacing `<token>` with the copied token) and click **Authorize**.
7. Test the protected GET API endpoints:
   - `GET /api/customers`
   - `GET /api/tickets`

## Running Tests
To run the automated unit tests, navigate to the solution folder and run:
```bash
dotnet test
```
Tests are available in the `CustomerEngagementPlatform.Tests` project.

## GitHub Actions
The CI pipeline configuration file is located at `.github/workflows/ci.yml`.
This workflow automatically triggers on commits and pull requests to restore packages, build the solution, and run unit tests.

## Docker
A `Dockerfile` is included in the project for containerized build and execution support.

## Notes
- The application uses two roles: **Customer** and **Staff**.
- Public registrations automatically create users with the **Customer** role.
- The **Staff** account (`staff@demo.com`) is seeded automatically on start if it does not exist.
- SQL Server database schema is initialized and updated using Entity Framework Core Code-First migrations.

---
**Submitted By**
Saumya Singh
Wipro Capstone Project
June 2026
