# Start API
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd api\ScorecardAPI; dotnet run"

# Start UI
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd ui; npm start"
