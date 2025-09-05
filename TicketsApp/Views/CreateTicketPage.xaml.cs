using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class CreateTicketPage : ContentPage
{
    private readonly CreateTicketPageViewModel _viewModel;

    public CreateTicketPage(CreateTicketPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }
}