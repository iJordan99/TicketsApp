using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IErrorParser
{
    Task<ApiErrorResponse?> Parse(HttpResponseMessage response);
}