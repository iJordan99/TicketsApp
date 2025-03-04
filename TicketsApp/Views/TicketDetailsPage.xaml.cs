using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class TicketDetailsPage : ContentPage
{
    public TicketDetailsPage(TicketDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}