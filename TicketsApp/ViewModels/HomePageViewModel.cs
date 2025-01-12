using CommunityToolkit.Mvvm.ComponentModel;
using TicketsApp.Interfaces;

namespace TicketsApp.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    [ObservableProperty] private string? _welcomeText;
    
    public HomePageViewModel(IAppState appState) : base(appState)
    {
        WelcomeText = AppState.CurrentUser?.Name;
    }
}