# Fix: "User not found" Error When Booking Appointments

## The Problem
Your browser has old user data from when the app used localStorage for authentication. Now the app uses PostgreSQL, so you need to register a NEW account in the database.

## Quick Fix (3 Steps)

### Step 1: Clear Old Data
1. Open your browser to: **http://localhost:5210/debug-auth**
2. Click the **"Clear All localStorage & Logout"** button
3. This removes all old user data from your browser

### Step 2: Register NEW Account  
1. Go to: **http://localhost:5210/register**
2. Enter a **new** username (minimum 3 characters)
3. Enter a **new** password (minimum 4 characters)
4. Click **Register**
5. You'll be automatically logged in and redirected to dashboard

### Step 3: Book Appointment
1. Go to **Find Doctors** (in the navbar)
2. Click **Book Appointment** on any doctor
3. Fill in the form and submit
4. ✅ It should work now!

## Why This Happened

- **Before**: Users were stored in browser localStorage (client-side only)
- **Now**: Users are stored in PostgreSQL database (server-side)
- **Issue**: Your old localStorage user doesn't exist in the database
- **Solution**: Register a fresh account that goes into PostgreSQL

## Verify It's Working

Check browser console (F12 → Console tab) when booking an appointment. You should see:
```
Attempting login for user: YOUR_USERNAME
User logged in: ID=X, Username=YOUR_USERNAME
CreateAppointment: User ID=X, Username=YOUR_USERNAME
Sending request to http://localhost:5230/api/appointments
Response status: 201 Created  <-- Success!
```

If you see "User not found", you're still using an old account - repeat Step 1 & 2.

## Current Application URLs

- **Frontend**: http://localhost:5210
- **API**: http://localhost:5230  
- **Database**: PostgreSQL in Docker (container: blazor-postgres)

## Pre-seeded Doctors

The database has 5 doctors ready:
1. Dr. Sarah Johnson - Cardiology ($150)
2. Dr. Michael Chen - Pediatrics ($120)
3. Dr. Emily Rodriguez - Dermatology ($130)
4. Dr. David Patel - Orthopedics ($160)
5. Dr. Lisa Thompson - Neurology ($180)
