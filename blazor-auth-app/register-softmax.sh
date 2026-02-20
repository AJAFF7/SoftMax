#!/bin/bash

# Register SoftMax assistant

echo "Registering SoftMax Assistant..."

curl -X POST https://blz.ajs-engineer.com/api/assistants/register \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "SoftMax Assistant",
    "email": "softmax@portal.com",
    "username": "softmax",
    "password": "SoftMax123!",
    "phoneNumber": "+1234567890"
  }'

echo ""
echo ""
echo "Registration complete! Getting QR code..."
echo "QR code will be saved to softmax-qrcode.png"

# Download the QR code
curl -X GET https://blz.ajs-engineer.com/api/assistants/qrcode/softmax@portal.com \
  -o softmax-qrcode.png

echo ""
echo "QR code saved to softmax-qrcode.png"
echo ""
echo "SoftMax Assistant has been registered with:"
echo "  Email: softmax@portal.com"
echo "  Username: softmax"
echo "  Password: SoftMax123!"
echo ""
echo "Use softmax-qrcode.png for QR code login"
