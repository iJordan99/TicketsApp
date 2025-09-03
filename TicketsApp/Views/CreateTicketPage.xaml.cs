using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class CreateTicketPage : ContentPage
{
    public CreateTicketPage(CreateTicketPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}