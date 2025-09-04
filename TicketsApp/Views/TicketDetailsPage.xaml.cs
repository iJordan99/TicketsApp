using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class TicketDetailsPage : ContentPage
{
    private readonly TicketDetailsViewModel _viewModel;

    public TicketDetailsPage(TicketDetailsViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}