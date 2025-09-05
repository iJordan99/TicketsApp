using TicketsApp.Models;
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEngineers();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _viewModel.SelectedEngineers.Clear();

        foreach (var item in e.CurrentSelection)
            if (item is User engineer)
                _viewModel.SelectedEngineers.Add(engineer);
    }
}