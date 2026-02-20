#!/bin/bash

# Check which service to view logs for
SERVICE=$1

if [ -z "$SERVICE" ]; then
    echo "📋 Viewing all service logs (press Ctrl+C to exit)..."
    echo ""
    docker-compose logs -f
else
    echo "📋 Viewing logs for: $SERVICE (press Ctrl+C to exit)..."
    echo ""
    docker-compose logs -f $SERVICE
fi
