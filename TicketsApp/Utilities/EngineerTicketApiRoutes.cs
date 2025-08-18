using TicketsApp.Models;

namespace TicketsApp.Utilities;

public static class EngineerTicketApiRoutes
{
    public static string? EngineerAssignedTickets(int? page)
    {
        return $"https://tickets.test/api/v1/engineer/tickets?page={page}";
    }

    public static string AssignEngineer(Ticket ticket)
    {
        return $"https://tickets.test/api/v1/tickets/{ticket.Id}/engineer";
    }

    public static string RemoveEngineer(Ticket ticket, User engineer)
    {
        return $"https://tickets.test/api/v1/tickets/{ticket.Id}/engineer/{engineer.Id}";
    }
}