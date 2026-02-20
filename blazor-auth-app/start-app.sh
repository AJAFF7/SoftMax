#!/bin/bash

# Get the directory where the script is located
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Start the API in the background
echo "Starting API on http://localhost:5230..."
cd "$SCRIPT_DIR/BlazorAuthApp.Api"
dotnet run --urls "http://localhost:5230" &
API_PID=$!

# Wait a bit for API to start
sleep 3

# Start the Blazor app
echo "Starting Blazor app on http://localhost:5210..."
cd "$SCRIPT_DIR/BlazorAuthApp"
dotnet run --urls "http://localhost:5210"

# Cleanup when Blazor app exits
kill $API_PID 2>/dev/null
