using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.Parsers;

/// <summary>
/// Parses JSON error responses and extracts relevant error information into an <see cref="ApiErrorResponse"/> object.
/// </summary>
public class ErrorParser : IErrorParser
{
    /// Parses a JsonElement to extract error information and build an ApiErrorResponse object.
    /// <param name="element">The JsonElement to be parsed, typically representing an error response.</param>
    /// <returns>An ApiErrorResponse object containing the parsed error details, or null if parsing fails or the structure is not as expected.
    public ApiErrorResponse? Parse(JsonElement element)
    {
        if (element.TryGetProperty("errors", out var errorsElement) && errorsElement.ValueKind == JsonValueKind.Array)
        {
            var errors = new List<ApiError>();

            foreach (var errorJson in errorsElement.EnumerateArray())
            {
                var type = errorJson.GetProperty("type").GetString() ?? string.Empty;
                var status = errorJson.GetProperty("status").GetInt32();
                var message = errorJson.GetProperty("message").GetString() ?? string.Empty;

                errors.Add(new ApiError(type, status, message));
            }

            return new ApiErrorResponse(errors);
        }

        if (element.TryGetProperty("error", out var errorElement) && errorElement.ValueKind == JsonValueKind.Object)
        {
            var type = errorElement.GetProperty("type").GetString() ?? string.Empty;
            var status = errorElement.GetProperty("status").GetInt32();
            var message = errorElement.GetProperty("message").GetString() ?? string.Empty;

            return new ApiErrorResponse(new ApiError(type, status, message));
        }

        if (element.TryGetProperty("message", out var messageElement) &&
            element.TryGetProperty("status", out var statusElement))
        {
            var message = messageElement.GetString() ?? string.Empty;
            var status = statusElement.GetInt32();

            return new ApiErrorResponse(new ApiError("generic", status, message));
        }

        return null;
    }
}