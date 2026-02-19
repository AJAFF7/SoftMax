#!/bin/bash

echo "=== Checking Database ==="
psql -d blazorauthdb -c "SELECT \"Id\", \"FirstName\", \"LastName\", \"Specialization\", \"IsAvailable\" FROM \"Doctors\";"

echo ""
echo "=== Checking if API is running on port 5230 ==="
lsof -i :5230

echo ""
echo "=== Checking if Blazor app is running on port 5210 ==="
lsof -i :5210
