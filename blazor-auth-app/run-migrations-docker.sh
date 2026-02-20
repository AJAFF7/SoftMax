#!/bin/bash

echo "🔄 Applying database migrations..."

# Wait for the API container to be ready
sleep 3

# Run migrations inside the API container
docker exec blazor-api dotnet ef database update

echo "✅ Migrations completed!"
