using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Services;

/// <summary>
/// A service for processing HTTP responses and converting them into 
/// <see cref="PostApiResponse"/> objects. Handles both success and failure scenarios.
/// </summary>
public class PostApiResponseService(IErrorParser errorParser) : IPostApiResponseService
{

    /// <summary>
    /// Processes an HTTP response and returns a structured PostApiResponse object.
    /// </summary>
    /// <param name="response">The HTTP response message to process.</param>
    /// <returns>A PostApiResponse object indicating success or containing parsed error details.</returns>
    public async Task<PostApiResponse> ProcessResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return new PostApiResponse(true, null);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var error = await ParseErrorAsync(responseContent);

        return new PostApiResponse(false, error);
    }

    /// <summary>
    /// Parses the response content to extract error details.
    /// </summary>
    /// <param name="responseContent">The JSON string content of the HTTP response.</param>
    /// <returns>An <see cref="ApiErrorResponse"/> object with the extracted error details.</returns>
    private Task<ApiErrorResponse?> ParseErrorAsync(string responseContent)
    {
        using var jsonDoc = JsonDocument.Parse(responseContent);
        var rootElement = jsonDoc.RootElement;
        return Task.FromResult(errorParser.Parse(rootElement));
    }
}