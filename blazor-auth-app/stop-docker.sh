#!/bin/bash

echo "🔄 Stopping all Docker containers..."
docker-compose down

if [ "$1" == "--clean" ] || [ "$1" == "-c" ]; then
    echo "🗑️  Removing all volumes (database data will be lost)..."
    docker-compose down -v
    echo "✅ Clean shutdown complete!"
else
    echo "✅ Shutdown complete!"
    echo ""
    echo "💡 To remove database volumes, use: ./stop-docker.sh --clean"
fi
