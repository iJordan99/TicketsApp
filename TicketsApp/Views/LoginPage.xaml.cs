using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.ViewModels;

namespace TicketsApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void Email_Changed(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (BindingContext is LoginPageViewModel viewModel) viewModel.EntryEmail = e.NewTextValue ?? string.Empty;
    }

    private void Password_Changed(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (BindingContext is LoginPageViewModel viewModel) viewModel.EntryPassword = e.NewTextValue ?? string.Empty;
    }
}