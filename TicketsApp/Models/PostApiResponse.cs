namespace TicketsApp.Models;

public class PostApiResponse(bool success, ApiErrorResponse? error)
{
    public bool Success { get; set; } = success;
    public ApiErrorResponse? Error { get; set; } = error;
}