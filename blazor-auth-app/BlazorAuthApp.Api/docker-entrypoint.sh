#!/bin/bash
set -e

echo "🚀 Starting API container..."
echo "⏳ Waiting for PostgreSQL to be ready..."

# Simple wait for postgres port to be open
sleep 5

echo "✅ Starting API server..."

# Start the application
exec dotnet BlazorAuthApp.Api.dll
