#!/bin/bash

echo "Testing API endpoints..."
echo ""

echo "1. Testing /api/doctors:"
curl -s http://localhost:5230/api/doctors

echo ""
echo ""
echo "2. Testing /api/doctors/specializations:"
curl -s http://localhost:5230/api/doctors/specializations

echo ""
echo ""
echo "3. Testing /api/doctors/1:"
curl -s http://localhost:5230/api/doctors/1
