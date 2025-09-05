using TicketsApp.Models;

namespace TicketsApp.Utilities;

public class AuthorApiRoutes
{
    public static string? BaseUri()
    {
        return "https://tickets.test/api/v1/authors";
    }

    public static string? AuthorTicket(User author)
    {
        return $"https://tickets.test/api/v1/authors/{author.Id}/tickets";
    }
}