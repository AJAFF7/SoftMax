#!/bin/bash

echo "Creating Doctors and Appointments tables..."
psql -d blazorauthdb -f /Users/jaff/SoftMax/blazor-auth-app/BlazorAuthApp.Api/create-tables.sql

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Tables created successfully!"
    echo "✅ 5 doctors seeded:"
    echo "   - Dr. Sarah Johnson (Cardiology)"
    echo "   - Dr. Michael Chen (Pediatrics)"
    echo "   - Dr. Emily Rodriguez (Dermatology)"
    echo "   - Dr. David Patel (Orthopedics)"
    echo "   - Dr. Lisa Thompson (Neurology)"
    echo ""
    echo "Now you can start the API:"
    echo "  cd BlazorAuthApp.Api && dotnet watch"
    echo ""
    echo "And the Blazor app:"
    echo "  cd BlazorAuthApp && dotnet watch"
else
    echo "❌ Failed to create tables"
fi
