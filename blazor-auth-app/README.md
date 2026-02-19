# Blazor WebAssembly Authentication App

A simple Blazor WebAssembly application with user registration, login, and account management features.

## Features

- **User Registration**: Create a new account with username and password
- **User Login**: Sign in with existing credentials
- **Home Page**: Simple landing page with links to Login and Register
- **Dashboard Page** (after login):
  - Displays welcome message with username
  - Quick access links to Home, Weather, and Counter pages
  - Delete User button - removes the current user account
  - Logout button - signs out the current user
- **Login/Register Pages**: Include quick links to Weather and Counter
- **Local Storage**: User data is persisted in browser's localStorage

## Project Structure

```
BlazorAuthApp/
├── Models/
│   └── User.cs                 # User model
├── Services/
│   ├── AuthService.cs          # Authentication service
│   └── LocalStorageService.cs  # Local storage wrapper
├── Pages/
│   ├── Home.razor             # Landing page
│   ├── Dashboard.razor        # Main dashboard with navigation
│   ├── Login.razor            # Login page
│   ├── Register.razor         # Registration page
│   ├── Counter.razor          # Counter demo page
│   └── Weather.razor          # Weather demo page
└── Program.cs                 # Service configuration
```

## How to Run

1. Navigate to the BlazorAuthApp directory:
   ```bash
   cd BlazorAuthApp
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser and navigate to the URL shown in the terminal (typically `https://localhost:5001` or `http://localhost:5000`)

## Usage Flow

1. **Home Page** (`/`): Simple landing page with Login and Register buttons

2. **Register** (`/register`): Create a new account
   - After successful registration, you'll be redirected to the Dashboard
   - Quick links to Counter and Weather pages available

3. **Login** (`/login`): Sign in with your credentials
   - After successful login, you'll be redirected to the Dashboard
   - Quick links to Counter and Weather pages available

4. **Dashboard** (`/dashboard`): Main hub after authentication
   - Welcome message with your username
   - Quick access buttons to:
     - **Home** - Return to landing page
     - **Weather** - View weather forecasts
     - **Counter** - Interactive counter page
   - **Delete User** button - removes your account and redirects to registration
   - **Logout** button - signs you out and redirects to login

## Technical Details

- **Framework**: Blazor WebAssembly (.NET 10.0)
- **Authentication**: Client-side authentication using localStorage
- **State Management**: Service-based with event notifications
- **UI**: Bootstrap 5 for styling

## Security Note

This is a demo application. In a production environment, you should:
- Use server-side authentication
- Implement proper password hashing
- Use secure HTTP-only cookies
- Add input validation and sanitization
- Implement HTTPS
