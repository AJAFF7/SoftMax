#!/bin/bash

# Register assistant Jaff with email jaff@gmail.com

echo "Registering assistant Jaff..."

curl -X POST http://localhost:5230/api/assistants/register \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Jaff Assistant",
    "email": "jaff@gmail.com",
    "username": "jaff",
    "password": "Jaff123!",
    "phoneNumber": "+1234567890"
  }'

echo ""
echo ""
echo "Registration complete! Getting QR code..."
echo "QR code will be saved to jaff-qrcode.png"

# Download the QR code
curl -X GET http://localhost:5230/api/assistants/qrcode/jaff@gmail.com \
  -o jaff-qrcode.png

echo "QR code saved to jaff-qrcode.png"
echo ""
echo "Assistant Jaff has been registered with:"
echo "  Email: jaff@gmail.com"
echo "  Username: jaff"
echo "  Password: Jaff123!"
echo ""
echo "Use jaff-qrcode.png for QR code login"
