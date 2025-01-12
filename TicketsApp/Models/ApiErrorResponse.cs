namespace TicketsApp.Models;

public class ApiErrorResponse(List<ApiError> errors)
{
    public List<ApiError> Errors { get; set; } = errors;
}