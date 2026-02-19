using System.Net.Http.Json;
using System.Net.Http.Headers;
using BlazorAuthApp.Models;

namespace BlazorAuthApp.Services;

public class AppointmentService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;
    private const string ApiUrl = "http://localhost:5230/api";

    public AppointmentService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<List<Appointment>> GetUserAppointmentsAsync()
    {
        try
        {
            var user = _authService.CurrentUser;
            if (user == null) return new List<Appointment>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiUrl}/appointments/user/{user.Id}");
            request.Headers.Add("X-Username", user.Username);

            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var appointments = await response.Content.ReadFromJsonAsync<List<Appointment>>();
                return appointments ?? new List<Appointment>();
            }

            return new List<Appointment>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching appointments: {ex.Message}");
            return new List<Appointment>();
        }
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<Appointment>($"{ApiUrl}/appointments/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching appointment: {ex.Message}");
            return null;
        }
    }

    public async Task<(bool success, string message)> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        try
        {
            var user = _authService.CurrentUser;
            if (user == null)
            {
                return (false, "You must be logged in to book an appointment");
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}/appointments");
            httpRequest.Headers.Add("X-Username", user.Username);
            httpRequest.Content = JsonContent.Create(request);

            var response = await _http.SendAsync(httpRequest);
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Appointment booked successfully!");
            }

            var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return (false, errorContent?.Message ?? "Failed to book appointment");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating appointment: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<bool> CancelAppointmentAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"{ApiUrl}/appointments/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cancelling appointment: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateAppointmentStatusAsync(int id, string status)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"{ApiUrl}/appointments/{id}/status", new { Status = status });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating appointment status: {ex.Message}");
            return false;
        }
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}
