using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Services;

public class AuthService(HttpClient httpClient, JsonSerializerOptions serializerOptions, IAppState appState)
    : IAuthService
{
    private const string loginUrl = "https://tickets.test/api/login";

    public async Task<ApiErrorResponse?> LoginAsync<T>(LoginRequest loginRequest)
    {
        try
        {
            var errorResponse = await _SetApiToken(loginRequest);
            if (errorResponse != null)
            {
                return errorResponse;
            }
            await _SetUserData();
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<ApiErrorResponse?> _SetApiToken(LoginRequest loginRequest)
    {
        var jsonPayload = JsonSerializer.Serialize(loginRequest, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await httpClient.PostAsync(loginUrl, content);
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                return new ApiErrorResponse(new List<ApiError>
                {
                    new ApiError(
                        type: "Unauthorized",
                        status: 401,
                        message: "Invalid credentials."
                    )
                });
            case HttpStatusCode.OK:
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseContent);
                var apiToken = doc.RootElement.GetProperty("data").GetProperty("token").GetString();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
                appState.ApiToken = apiToken;
                break;
            }

            default:
                return await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        }
        return null;
    }
    
    private async Task _SetUserData()
    {
        HttpResponseMessage response = await httpClient.GetAsync("https://tickets.test/api/v1/user");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement.GetProperty("data");
            var attributes = root.GetProperty("attributes");

            appState.CurrentUser = new User
            {
                Id = root.GetProperty("id").GetInt32(),
                Name = attributes.GetProperty("name").GetString() ?? "",
                Email = attributes.GetProperty("email").GetString() ?? "",
                IsAdmin = attributes.GetProperty("is_admin").GetBoolean(),
                IsEngineer = attributes.GetProperty("is_engineer").GetBoolean()
            };
        }
    }
    
}