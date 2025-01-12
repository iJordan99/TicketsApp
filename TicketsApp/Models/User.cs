namespace TicketsApp.Models;

public class User
{
    public int Id;
    public required string Name;
    public required string Email;
    public bool IsAdmin;
    public bool IsEngineer;
}