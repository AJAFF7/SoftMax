using BlazorAuthApp.Models;
using BlazorAuthApp.DTOs;
using System.Net.Http.Json;

namespace BlazorAuthApp.Services;

public class AuthService
{
    private const string CURRENT_USER_KEY = "currentUser";
    private readonly string ApiUrl;
    
    private readonly LocalStorageService _localStorage;
    private readonly HttpClient _http;
    
    public User? CurrentUser { get; private set; }
    
    public event Action? OnAuthStateChanged;

    public AuthService(LocalStorageService localStorage, HttpClient http, ApiConfiguration config)
    {
        _localStorage = localStorage;
        _http = http;
        ApiUrl = config.BaseUrl;
    }

    public async Task InitializeAsync()
    {
        await LoadCurrentUserAsync();
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        try
        {
            var request = new RegisterRequest
            {
                Username = username,
                Password = password
            };

            var response = await _http.PostAsJsonAsync($"{ApiUrl}/auth/register", request);
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResponse?.Success == true && authResponse.User != null)
            {
                CurrentUser = new User
                {
                    Id = authResponse.User.Id,
                    Username = authResponse.User.Username
                };
                await SaveCurrentUserAsync(CurrentUser);
                OnAuthStateChanged?.Invoke();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            Console.WriteLine($"Attempting login for user: {username}");
            var response = await _http.PostAsJsonAsync($"{ApiUrl}/auth/login", request);
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            Console.WriteLine($"Login response - Success: {authResponse?.Success}, User: {authResponse?.User?.Username}");

            if (authResponse?.Success == true && authResponse.User != null)
            {
                CurrentUser = new User
                {
                    Id = authResponse.User.Id,
                    Username = authResponse.User.Username
                };
                await SaveCurrentUserAsync(CurrentUser);
                Console.WriteLine($"User logged in: ID={CurrentUser.Id}, Username={CurrentUser.Username}");
                OnAuthStateChanged?.Invoke();
                return true;
            }

            Console.WriteLine($"Login failed: {authResponse?.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        await _localStorage.RemoveItemAsync(CURRENT_USER_KEY);
        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> DeleteCurrentUserAsync()
    {
        if (CurrentUser == null)
            return false;

        try
        {
            var response = await _http.DeleteAsync($"{ApiUrl}/auth/user/{CurrentUser.Username}");
            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResponse?.Success == true)
            {
                await LogoutAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete user error: {ex.Message}");
            return false;
        }
    }

    public bool IsAuthenticated() => CurrentUser != null;

    private async Task LoadCurrentUserAsync()
    {
        CurrentUser = await _localStorage.GetItemAsync<User>(CURRENT_USER_KEY);
    }

    private async Task SaveCurrentUserAsync(User user)
    {
        await _localStorage.SetItemAsync(CURRENT_USER_KEY, user);
    }
}
