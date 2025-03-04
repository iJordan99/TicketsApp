using CommunityToolkit.Mvvm.ComponentModel;

namespace TicketsApp.Models;

public partial class Comment : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string? _text;
    [ObservableProperty] private User? _user;
}