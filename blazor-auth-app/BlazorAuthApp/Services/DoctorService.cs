using System.Net.Http.Json;
using BlazorAuthApp.Models;

namespace BlazorAuthApp.Services;

public class DoctorService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;
    private const string ApiUrl = "http://localhost:5230/api";

    public DoctorService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<List<Doctor>> GetDoctorsAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<Doctor>>($"{ApiUrl}/doctors");
            return response ?? new List<Doctor>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching doctors: {ex.Message}");
            return new List<Doctor>();
        }
    }

    public async Task<Doctor?> GetDoctorByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<Doctor>($"{ApiUrl}/doctors/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching doctor: {ex.Message}");
            return null;
        }
    }

    public async Task<List<string>> GetSpecializationsAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<string>>($"{ApiUrl}/doctors/specializations");
            return response ?? new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching specializations: {ex.Message}");
            return new List<string>();
        }
    }

    public async Task<List<Doctor>> SearchDoctorsAsync(string? specialization = null)
    {
        try
        {
            var url = $"{ApiUrl}/doctors/search";
            if (!string.IsNullOrEmpty(specialization))
            {
                url += $"?specialization={Uri.EscapeDataString(specialization)}";
            }
            
            var response = await _http.GetFromJsonAsync<List<Doctor>>(url);
            return response ?? new List<Doctor>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching doctors: {ex.Message}");
            return new List<Doctor>();
        }
    }
}
