using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketsApp.Interfaces;
using TicketsApp.Models;
namespace TicketsApp.ViewModels;

public partial class LoginPageViewModel(IAppState appState, IAuthService authService) : BaseViewModel(appState)
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _entryEmail;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _entryPassword;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        try
        {
            var authenticate = await authService.LoginAsync(new LoginRequest(EntryEmail, EntryPassword));
            if (authenticate != null && authenticate.Errors.Any())
            {
                var errorMessages = string.Join(Environment.NewLine, authenticate.Errors.Select(e => e.Message));
                await Shell.Current.DisplayAlert("Login Failed", errorMessages, "OK");
                return;
            }

            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            await Shell.Current
                .DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
        }
    }

    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(EntryEmail) && !string.IsNullOrEmpty(EntryPassword);
    }

    [RelayCommand]
    private async Task NavigateToRegisterPage()
    {
        await Shell.Current.GoToAsync("/RegisterPage");
    }
}