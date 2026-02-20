#!/bin/bash

echo "======================================"
echo "Testing Assistant Login System"
echo "======================================"
echo ""

# Test 1: Check if API is running
echo "1. Checking API..."
curl -s http://localhost:5230/api/assistants > /dev/null
if [ $? -eq 0 ]; then
    echo "   ✓ API is running"
else
    echo "   ✗ API is not running"
    exit 1
fi
echo ""

# Test 2: Get existing assistants
echo "2. Existing assistants:"
curl -s http://localhost:5230/api/assistants | grep -o '"username":"[^"]*"' | head -5
echo ""

# Test 3: Register a NEW test assistant with simple credentials
echo "3. Registering new test assistant..."
REGISTER_RESPONSE=$(curl -s -w "\n%{http_code}" -X POST http://localhost:5230/api/assistants/register \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Test Assistant",
    "email": "test@test.com",
    "username": "test",
    "password": "test123",
    "phoneNumber": "+9999999999"
  }')

HTTP_CODE=$(echo "$REGISTER_RESPONSE" | tail -n1)
RESPONSE_BODY=$(echo "$REGISTER_RESPONSE" | head -n-1)

if [ "$HTTP_CODE" = "201" ] || [ "$HTTP_CODE" = "200" ]; then
    echo "   ✓ New assistant registered successfully"
    echo "   Username: test"
    echo "   Password: test123"
    BARCODE=$(echo "$RESPONSE_BODY" | grep -o '"eTagBarcode":"[^"]*"' | cut -d'"' -f4)
    echo "   Barcode: $BARCODE"
elif [ "$HTTP_CODE" = "400" ]; then
    echo "   ⚠ Assistant 'test' already exists (that's OK)"
    echo "   Username: test"
    echo "   Password: test123"
else
    echo "   ✗ Registration failed with code: $HTTP_CODE"
    echo "   Response: $RESPONSE_BODY"
fi
echo ""

# Test 4: Try login with username "test"
echo "4. Testing login with username 'test'..."
LOGIN_RESPONSE=$(curl -s -w "\n%{http_code}" -X POST http://localhost:5230/api/assistants/login \
  -H "Content-Type: application/json" \
  -d '{"username": "test", "password": "test123"}')

HTTP_CODE=$(echo "$LOGIN_RESPONSE" | tail -n1)
RESPONSE_BODY=$(echo "$LOGIN_RESPONSE" | head -n-1)

if [ "$HTTP_CODE" = "200" ]; then
    echo "   ✓ Login with username SUCCESSFUL!"
    echo "   Response: $RESPONSE_BODY"
else
    echo "   ✗ Login FAILED with code: $HTTP_CODE"
    echo "   Response: $RESPONSE_BODY"
fi
echo ""

# Test 5: Try login with username "jaff"
echo "5. Testing login with username 'jaff'..."
LOGIN_RESPONSE=$(curl -s -w "\n%{http_code}" -X POST http://localhost:5230/api/assistants/login \
  -H "Content-Type: application/json" \
  -d '{"username": "jaff", "password": "Jaff123!"}')

HTTP_CODE=$(echo "$LOGIN_RESPONSE" | tail -n1)
RESPONSE_BODY=$(echo "$LOGIN_RESPONSE" | head -n-1)

if [ "$HTTP_CODE" = "200" ]; then
    echo "   ✓ Login with username SUCCESSFUL!"
    echo "   Response: $RESPONSE_BODY"
else
    echo "   ✗ Login FAILED with code: $HTTP_CODE"
    echo "   Response: $RESPONSE_BODY"
fi
echo ""

echo "======================================"
echo "CREDENTIALS TO USE IN BROWSER:"
echo "======================================"
echo "Option 1:"
echo "  Username: test"
echo "  Password: test123"
echo ""
echo "Option 2:"
echo "  Username: jaff"
echo "  Password: Jaff123!"
echo ""
echo "Go to: http://localhost:5210/assistant-login"
echo "======================================"
