#!/bin/bash
set -e

echo "🚀 Starting API container..."
echo "Waiting for PostgreSQL to be ready..."

# Wait for postgres to be ready with retry logic
max_retries=30
counter=0
until /app/efbundle --connection "Host=postgres;Port=5432;Database=blazorauthdb;Username=blazoruser;Password=BlazorPass123!" 2>/dev/null || [ $counter -eq $max_retries ]; do
  counter=$((counter+1))
  echo "⏳ Waiting for database connection... (attempt $counter/$max_retries)"
  sleep 2
done

if [ $counter -eq $max_retries ]; then
  echo "⚠️  Could not connect to database after $max_retries attempts"
  echo "🌐 Starting API server anyway..."
else
  echo "✅ Database migrations applied successfully!"
fi

echo "🌐 Starting API server..."

# Start the application
exec dotnet BlazorAuthApp.Api.dll
