using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        _ = ((HomePageViewModel)BindingContext).RefreshAsync();
        base.OnAppearing();
    }
}