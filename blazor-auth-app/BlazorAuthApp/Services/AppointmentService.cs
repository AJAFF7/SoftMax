using System.Net.Http.Json;
using System.Net.Http.Headers;
using BlazorAuthApp.Models;

namespace BlazorAuthApp.Services;

public class AppointmentService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;
    private readonly string ApiUrl;

    public AppointmentService(HttpClient http, AuthService authService, ApiConfiguration config)
    {
        _http = http;
        _authService = authService;
        ApiUrl = config.BaseUrl;
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
                Console.WriteLine("CreateAppointment: CurrentUser is NULL");
                return (false, "You must be logged in to book an appointment");
            }

            Console.WriteLine($"CreateAppointment: User ID={user.Id}, Username={user.Username}");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}/appointments");
            httpRequest.Headers.Add("X-Username", user.Username);
            httpRequest.Content = JsonContent.Create(request);

            Console.WriteLine($"Sending request to {ApiUrl}/appointments with X-Username: {user.Username}");
            var response = await _http.SendAsync(httpRequest);
            
            Console.WriteLine($"Response status: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {responseContent}");
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Appointment booked successfully!");
            }

            // Try to parse error as JSON, otherwise use raw content
            if (!string.IsNullOrWhiteSpace(responseContent))
            {
                try
                {
                    var errorObj = System.Text.Json.JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                    if (errorObj?.Message != null)
                    {
                        return (false, errorObj.Message);
                    }
                }
                catch (System.Text.Json.JsonException)
                {
                    // Not JSON, return as-is
                    Console.WriteLine("Response is not valid JSON");
                }
                
                return (false, responseContent.Length > 200 ? responseContent.Substring(0, 200) : responseContent);
            }

            return (false, $"Request failed with status code {response.StatusCode}");
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Error: {httpEx.Message}");
            return (false, "Unable to connect to the server. Please ensure the API is running.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating appointment: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
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

    public async Task<(bool success, string message)> UpdateAppointmentStatusAsync(int id, string status)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"{ApiUrl}/appointments/{id}/status", new { Status = status });
            if (response.IsSuccessStatusCode)
            {
                return (true, "Status updated successfully!");
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, errorContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating appointment status: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<List<Appointment>> GetAllAppointmentsAsync()
    {
        try
        {
            var response = await _http.GetAsync($"{ApiUrl}/appointments");
            if (response.IsSuccessStatusCode)
            {
                var appointments = await response.Content.ReadFromJsonAsync<List<Appointment>>();
                return appointments ?? new List<Appointment>();
            }

            return new List<Appointment>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching all appointments: {ex.Message}");
            return new List<Appointment>();
        }
    }

    public async Task<(bool success, string message)> CheckInPatientAsync(int appointmentId, int assistantId)
    {
        try
        {
            var checkInData = new { AssistantId = assistantId };
            var response = await _http.PutAsJsonAsync($"{ApiUrl}/appointments/{appointmentId}/check-in", checkInData);
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Checked in successfully!");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, errorContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking in: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> FinishAppointmentAsync(int appointmentId)
    {
        try
        {
            var response = await _http.PutAsync($"{ApiUrl}/appointments/{appointmentId}/finish", null);
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Appointment marked as finished!");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, errorContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finishing appointment: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}
