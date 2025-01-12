using CommunityToolkit.Mvvm.ComponentModel;
using TicketsApp.Interfaces;

namespace TicketsApp.ViewModels;

public class BaseViewModel : ObservableObject
{
    protected readonly IAppState AppState;
    
    protected BaseViewModel(IAppState appState)
    {
        this.AppState = appState;
    } 
}