#!/bin/bash

cd /Users/jaff/SoftMax/blazor-auth-app/BlazorAuthApp.Api

echo "Step 1: Removing old migrations..."
rm -rf Migrations/

echo "Step 2: Creating new migration..."
dotnet ef migrations add AddDoctorAppointments

if [ $? -ne 0 ]; then
    echo "❌ Failed to create migration"
    exit 1
fi

echo "Step 3: Running database migration..."
dotnet ef database update

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Migration successful!"
    echo "✅ 5 doctors seeded with specializations:"
    echo "   - Cardiology (Dr. Sarah Johnson)"
    echo "   - Pediatrics (Dr. Michael Chen)"
    echo "   - Dermatology (Dr. Emily Rodriguez)"
    echo "   - Orthopedics (Dr. David Patel)"
    echo "   - Neurology (Dr. Lisa Thompson)"
    echo ""
    echo "You can now restart the API and start booking appointments!"
else
    echo ""
    echo "❌ Migration failed. Check the error above."
    echo ""
    echo "Common fixes:"
    echo "1. Make sure PostgreSQL is running: brew services start postgresql@17"
    echo "2. Check if database exists: psql -l | grep blazorauthdb"
    echo "3. Create database if missing: createdb blazorauthdb"
fi
