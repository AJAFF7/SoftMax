#!/bin/bash

# Kill any existing processes
pkill -f "dotnet run" 2>/dev/null
sleep 2

# Start API
cd /home/dev/SoftMax/blazor-auth-app/BlazorAuthApp.Api
echo "Starting API..."
dotnet run --urls "http://localhost:5230" &
API_PID=$!

# Wait for API to start
sleep 5

# Start Blazor
cd /home/dev/SoftMax/blazor-auth-app/BlazorAuthApp
echo "Starting Blazor..."
dotnet run --urls "http://localhost:5210" &
BLAZOR_PID=$!

echo "API PID: $API_PID"
echo "Blazor PID: $BLAZOR_PID"
echo "Services started!"
echo "API: http://localhost:5230"
echo "Blazor: http://localhost:5210"
