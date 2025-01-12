using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp;

public class AppState : IAppState
{
    public string? ApiToken { get; set; }
    public User? CurrentUser { get; set; }

}