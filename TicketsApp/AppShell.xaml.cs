using TicketsApp.Views;

namespace TicketsApp;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(TicketDetailsPage), typeof(TicketDetailsPage));
    }
}