using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IAppState
{
    public User? CurrentUser { get; set; }
    public string? ApiToken { get; set; }
}
