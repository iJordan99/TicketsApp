using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IAuthService
{
    Task<ApiErrorResponse?> LoginAsync<T>(LoginRequest loginRequest);
}