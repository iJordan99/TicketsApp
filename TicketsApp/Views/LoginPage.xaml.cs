using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}