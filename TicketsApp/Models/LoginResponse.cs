namespace TicketsApp.Models;

public class LoginResponse
{
    public LoginResponse(string token, string userName, string email)
    {
        Token = token;
        UserName = userName;
        Email = email;
    }

    public string Token { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}