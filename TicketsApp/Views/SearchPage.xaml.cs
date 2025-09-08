using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class SearchPage : ContentPage
{
    private readonly SearchPageViewModel _viewModel;

    public SearchPage(SearchPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}