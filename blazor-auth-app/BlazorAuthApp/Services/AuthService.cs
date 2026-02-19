using BlazorAuthApp.Models;

namespace BlazorAuthApp.Services;

public class AuthService
{
    private const string USERS_KEY = "users";
    private const string CURRENT_USER_KEY = "currentUser";
    
    private readonly LocalStorageService _localStorage;
    
    public User? CurrentUser { get; private set; }
    
    public event Action? OnAuthStateChanged;

    public AuthService(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        await LoadCurrentUserAsync();
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var users = await GetUsersAsync();
        
        if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            return false;

        // Generate a new ID (simple incrementing based on existing users)
        int newId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        
        var newUser = new User { Id = newId, Username = username, Password = password };
        users.Add(newUser);
        await SaveUsersAsync(users);
        
        await LoginAsync(username, password);
        return true;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var users = await GetUsersAsync();
        var user = users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
            u.Password == password);

        if (user != null)
        {
            CurrentUser = user;
            await SaveCurrentUserAsync(user);
            OnAuthStateChanged?.Invoke();
            return true;
        }

        return false;
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

        var users = await GetUsersAsync();
        users.RemoveAll(u => u.Username.Equals(CurrentUser.Username, StringComparison.OrdinalIgnoreCase));
        await SaveUsersAsync(users);
        
        await LogoutAsync();
        return true;
    }

    public bool IsAuthenticated() => CurrentUser != null;

    private async Task<List<User>> GetUsersAsync()
    {
        var users = await _localStorage.GetItemAsync<List<User>>(USERS_KEY);
        return users ?? new List<User>();
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        await _localStorage.SetItemAsync(USERS_KEY, users);
    }

    private async Task LoadCurrentUserAsync()
    {
        CurrentUser = await _localStorage.GetItemAsync<User>(CURRENT_USER_KEY);
    }

    private async Task SaveCurrentUserAsync(User user)
    {
        await _localStorage.SetItemAsync(CURRENT_USER_KEY, user);
    }
}
