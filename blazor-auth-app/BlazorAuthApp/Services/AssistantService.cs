using System.Net.Http.Json;
using System.Text.Json;
using BlazorAuthApp.Models;

namespace BlazorAuthApp.Services
{
    public class AssistantService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorage;
        private readonly string ApiUrl;

        public AssistantService(HttpClient httpClient, LocalStorageService localStorage, ApiConfiguration config)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            ApiUrl = config.BaseUrl;
        }

        public async Task<(bool success, string message, Assistant? assistant)> RegisterAsync(AssistantRegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/assistants/register", request);

                if (response.IsSuccessStatusCode)
                {
                    var assistant = await response.Content.ReadFromJsonAsync<Assistant>();
                    if (assistant != null)
                    {
                        await _localStorage.SetItemAsync("currentAssistant", assistant);
                        return (true, "Registration successful!", assistant);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, Assistant? assistant)> LoginAsync(AssistantLoginRequest request)
        {
            try
            {
                Console.WriteLine($"[AssistantService] LoginAsync called");
                Console.WriteLine($"[AssistantService] Request Username: '{request.Username}'");
                Console.WriteLine($"[AssistantService] Request Password: '{request.Password}'");
                Console.WriteLine($"[AssistantService] Sending POST to: {ApiUrl}/assistants/login");
                
                var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/assistants/login", request);

                Console.WriteLine($"[AssistantService] Response StatusCode: {response.StatusCode}");
                Console.WriteLine($"[AssistantService] Response IsSuccess: {response.IsSuccessStatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var assistant = await response.Content.ReadFromJsonAsync<Assistant>();
                    if (assistant != null)
                    {
                        // Add login timestamp
                        var loginInfo = new AssistantLoginInfo
                        {
                            Assistant = assistant,
                            LoginTime = DateTime.Now,
                            LastActivity = DateTime.Now
                        };
                        
                        await _localStorage.SetItemAsync("currentAssistant", assistant);
                        await _localStorage.SetItemAsync("assistantLoginInfo", loginInfo);
                        Console.WriteLine($"[AssistantService] Login successful for: {assistant.Username}");
                        return (true, "Login successful!", assistant);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[AssistantService] Login failed - Status: {response.StatusCode}, Error: {errorContent}");
                return (false, errorContent, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AssistantService] Login exception: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"[AssistantService] Exception StackTrace: {ex.StackTrace}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, Assistant? assistant)> LoginByBarcodeAsync(string barcode)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/assistants/login-barcode", barcode);

                if (response.IsSuccessStatusCode)
                {
                    var assistant = await response.Content.ReadFromJsonAsync<Assistant>();
                    if (assistant != null)
                    {
                        // Add login timestamp
                        var loginInfo = new AssistantLoginInfo
                        {
                            Assistant = assistant,
                            LoginTime = DateTime.Now,
                            LastActivity = DateTime.Now
                        };
                        
                        await _localStorage.SetItemAsync("currentAssistant", assistant);
                        await _localStorage.SetItemAsync("assistantLoginInfo", loginInfo);
                        return (true, "Login successful!", assistant);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Barcode login error: {ex.Message}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, Assistant? assistant)> LoginByFaceAsync(string faceDescriptor)
        {
            try
            {
                Console.WriteLine($"[AssistantService] LoginByFaceAsync called");
                
                var requestData = new { FaceDescriptor = faceDescriptor };
                var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/assistants/login-face", requestData);

                Console.WriteLine($"[AssistantService] Face login response: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var assistant = await response.Content.ReadFromJsonAsync<Assistant>();
                    if (assistant != null)
                    {
                        // Add login timestamp
                        var loginInfo = new AssistantLoginInfo
                        {
                            Assistant = assistant,
                            LoginTime = DateTime.Now,
                            LastActivity = DateTime.Now
                        };
                        
                        await _localStorage.SetItemAsync("currentAssistant", assistant);
                        await _localStorage.SetItemAsync("assistantLoginInfo", loginInfo);
                        Console.WriteLine($"[AssistantService] Face login successful for: {assistant.Username}");
                        return (true, "Login successful!", assistant);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[AssistantService] Face login failed: {errorContent}");
                return (false, errorContent, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AssistantService] Face login error: {ex.Message}");
                return (false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> RegisterFaceAsync(string email, string faceDescriptor)
        {
            try
            {
                var requestData = new { Email = email, FaceDescriptor = faceDescriptor };
                var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/assistants/register-face", requestData);

                if (response.IsSuccessStatusCode)
                {
                    return (true, "Face registered successfully!");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Face registration error: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<Assistant?> GetCurrentAssistantAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<Assistant>("currentAssistant");
            }
            catch
            {
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("currentAssistant");
            await _localStorage.RemoveItemAsync("assistantLoginInfo");
        }

        public async Task<AssistantLoginInfo?> GetLoginInfoAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<AssistantLoginInfo>("assistantLoginInfo");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Assistant>> GetAllAssistantsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}/assistants");
                if (response.IsSuccessStatusCode)
                {
                    var assistants = await response.Content.ReadFromJsonAsync<List<Assistant>>();
                    return assistants ?? new List<Assistant>();
                }
                return new List<Assistant>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assistants: {ex.Message}");
                return new List<Assistant>();
            }
        }
    }
}
