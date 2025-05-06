namespace TicketsApp.Utilities;

public static class EngineerTicketApiRoutes
{
    public static string? GetEngineerAssignedTickets(int page)
    {
        return $"https://tickets.test/api/v1/engineer/tickets?page={page}";
    }
}