namespace TicketsApp.Models;

public class ApiErrorResponse
{

    public ApiErrorResponse(List<ApiError> errors)
    {
        Errors = errors;
    }

    public ApiErrorResponse(ApiError error)
    {
        Errors = new List<ApiError> { error };
    }

    public List<ApiError> Errors { get; set; }
}