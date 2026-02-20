#!/bin/bash

echo "🚀 Starting Blazor Auth App with Docker..."
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

# Stop any existing containers
echo "🛑 Stopping existing containers..."
docker-compose down

# Build and start all services
echo ""
echo "🔨 Building and starting containers..."
docker-compose up --build -d

# Wait for services to be ready
echo ""
echo "⏳ Waiting for services to start..."
sleep 5

# Check if all services are running
echo ""
echo "📊 Checking service status..."
docker-compose ps

echo ""
echo "✅ All services are starting!"
echo ""
echo "🌐 Access the application:"
echo "   - Blazor App: http://localhost"
echo "   - API: http://localhost:8080"
echo "   - PostgreSQL: localhost:5432"
echo ""
echo "📝 View logs:"
echo "   docker-compose logs -f"
echo ""
echo "🛑 Stop all services:"
echo "   docker-compose down"
echo ""
