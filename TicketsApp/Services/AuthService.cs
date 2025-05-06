using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Utilities;

namespace TicketsApp.Services;

/// <summary>
/// Provides functionality for user authentication, including managing login requests and handling API responses.
/// </summary>
/// <remarks>
/// This service facilitates the authentication processes by communicating with the authentication API,
/// managing serialized data, and integrating user-related state into the application.
/// </remarks>
public class AuthService(HttpClient httpClient, JsonSerializerOptions serializerOptions, IAppState appState, IPostApiResponseService postApiResponseService) : IAuthService
{
    /// <summary>
    /// Authenticates a user based on the provided login request and initiates user session management.
    /// </summary>
    /// <param name="loginRequest">An object containing user credentials, such as email and password, for authentication.</param>
    /// <returns>
    /// An <see cref="ApiErrorResponse"/> object describing the login error if authentication fails;
    /// otherwise, returns null on successful user authentication.
    /// </returns>
    public async Task<ApiErrorResponse?> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var apiResponse = await AuthenticateAndSetTokenAsync(loginRequest);

            if (!apiResponse.Success)
            {
                return apiResponse.Error;
            }

            await RetrieveAndSetUserDataAsync();
            return null;
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync($"Error occurred during login: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// Authenticates a user using the provided login request, sets the API token
    /// for subsequent requests, and returns the result of the operation.
    /// </summary>
    /// <param name="loginRequest">An instance of <see cref="LoginRequest"/> that contains the user's
    /// login credentials, including email and password.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a <see cref="PostApiResponse"/> object which indicates whether the authentication
    /// was successful, and includes an error response if authentication fails.
    /// </returns>
    private async Task<PostApiResponse> AuthenticateAndSetTokenAsync(LoginRequest loginRequest)
    {
        var jsonPayload = JsonSerializer.Serialize(loginRequest, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(AuthApiRoutes.LoginUrl(), content);
        var apiResponse = await postApiResponseService.ProcessResponse(response);

        if (apiResponse.Success)
        {
            var token = await ParseApiToken(response);
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                appState.ApiToken = token;
            }
        }

        return apiResponse;
    }

    /// <summary>
    /// Extracts and returns the API token from the HTTP response message
    /// if available in the response content.
    /// </summary>
    /// <param name="response">The HTTP response message containing the API token.</param>
    /// <returns>
    /// The extracted API token as a string if available; otherwise, null.
    /// </returns>
    private async Task<string?> ParseApiToken(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseContent);

        if (doc.RootElement.TryGetProperty("data", out var dataElement) &&
            dataElement.TryGetProperty("token", out var tokenElement))
        {
            return tokenElement.GetString();
        }

        return null;
    }

    /// <summary>
    /// Retrieves the authenticated user's data from the server and updates the application state with the user's information.
    /// </summary>
    /// <remarks>
    /// This method sends a request to the predefined user information API endpoint, processes the server's response,
    /// and updates the application's state with the retrieved user details, if available. It is invoked after successful user authentication.
    /// </remarks>
    /// <returns>
    /// A task representing the asynchronous operation for retrieving and setting user data.
    /// Does not return a value upon successful completion.
    /// </returns>
    private async Task RetrieveAndSetUserDataAsync()
    {
        var response = await httpClient.GetAsync(UserApiRoutes.GetUserInfo());

        if (response.IsSuccessStatusCode)
        {
            var user = await ParseUserData(response);
            if (user != null)
            {
                appState.CurrentUser = user;
            }
        }
    }

    /// <summary>
    /// Parses the user data from an HTTP response and constructs a <see cref="User"/> object.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/> containing the raw HTTP response with user data.</param>
    /// <returns>
    /// A constructed <see cref="User"/> object if the response contains valid user data; otherwise, null.
    /// </returns>
    private async Task<User?> ParseUserData(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement.GetProperty("data");
        var attributes = root.GetProperty("attributes");

        var email = attributes.GetProperty("email").GetString();
        var id = root.GetProperty("id").GetInt32();
        var isEngineer = attributes.GetProperty("is_engineer").GetBoolean();
        var name = attributes.GetProperty("name").GetString();

        return email != null && name != null 
            ? new User(email, id, isEngineer, name) 
            : null; 
    }
}