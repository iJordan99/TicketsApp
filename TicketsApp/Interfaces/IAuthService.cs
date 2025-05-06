using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface IAuthService
{
    Task<ApiErrorResponse?> LoginAsync(LoginRequest loginRequest);
}