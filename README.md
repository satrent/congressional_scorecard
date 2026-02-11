# Congressional Scorecard

A web application to track and score congress members based on their voting records.

## Project Structure
- `api/`: .NET 8.0 Web API and Data Seeder
- `ui/`: Angular 17+ Frontend

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- Node.js & npm
- AWS Account (DynamoDB access)

### 1. Database Setup (DynamoDB)
The application uses DynamoDB. You can seed the database using the `DbSeeder` tool.

**Note:** The seeder is a console app and reads credentials from environment variables.

```powershell
# Set your AWS credentials
$env:AWS_ACCESS_KEY_ID="YOUR_ACCESS_KEY"
$env:AWS_SECRET_ACCESS_KEY="YOUR_SECRET_KEY"
$env:AWS_REGION="us-east-2"

# Run the seeder
cd api/DbSeeder
dotnet run
```

### 2. API Setup (ScorecardAPI)
The API manages its own secrets using .NET User Secrets.

```powershell
cd api/ScorecardAPI

# Initialize user secrets
dotnet user-secrets init

# Set your AWS secrets
dotnet user-secrets set "AWS:AccessKey" "YOUR_ACCESS_KEY"
dotnet user-secrets set "AWS:SecretKey" "YOUR_SECRET_KEY"
dotnet user-secrets set "AWS:Region" "us-east-2"

# Run the API
dotnet run
```

### 3. UI Setup (Angular)
```bash
cd ui
npm install
ng serve
```

Navigate to `http://localhost:4200` to view the app.
