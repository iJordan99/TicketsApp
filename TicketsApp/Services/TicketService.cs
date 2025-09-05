using System.Text;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Utilities;

namespace TicketsApp.Services;

/// <summary>
///     Provides functionality to manage tickets, including retrieving ticket details
///     with associated data and adding comments to a ticket.
/// </summary>
public class TicketService(
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions,
    IQueryStringBuilder queryStringBuilder) : ITicketService
{
    /// <summary>
    ///     Adds a comment to the specified ticket.
    /// </summary>
    /// <param name="comment">The comment to be added to the ticket.</param>
    /// <param name="ticket">The ticket to which the comment will be added.</param>
    /// <returns>A <see cref="PostApiResponse" /> containing the success status and any error details if the operation fails.</returns>
    public async Task<HttpResponseMessage> AddComment(string comment, Ticket ticket)
    {
        var payload = new
        {
            data = new
            {
                attributes = new
                {
                    comment
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        return await httpClient.PostAsync(TicketApiRoutes.AddComment(ticket), content);
    }

    public async Task<HttpResponseMessage> GetTickets(TicketQueryParameter? parameters)
    {
        var baseUri = "https://tickets.test/api/v1/tickets";
        var queryString = queryStringBuilder.BuildQueryString(parameters);
        var uri = string.IsNullOrEmpty(queryString) ? baseUri : $"{baseUri}?{queryString}";

        return await httpClient.GetAsync(uri);
    }

    /// <summary>
    ///     Retrieves a detailed ticket with additional related information, such as comments, author, and engineers, based on
    ///     the specified ticket.
    /// </summary>
    /// <param name="ticket">The ticket for which detailed information is required.</param>
    /// <param name="includes">data resources to include in the request</param>
    /// <returns>
    ///     A <see cref="TicketWithIncludes" /> object containing the ticket details with included related data,
    ///     or <c>null</c> if the operation is unsuccessful or the response status is not successful.
    /// </returns>
    public async Task<HttpResponseMessage> GetTicketWithIncludes(Ticket ticket, string includes = "")
    {
        return await httpClient.GetAsync(
            TicketApiRoutes.TicketWithIncludes(ticket, includes));
    }

    public async Task<HttpResponseMessage> UpdateTicket(Ticket ticket, User user, (string Key, string Value)[] data)
    {
        var attributes = new Dictionary<string, object>();
        foreach (var (key, value) in data) attributes[key] = value;

        var payload = new
        {
            data = new
            {
                attributes
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        return await httpClient.PatchAsync(TicketApiRoutes.TicketById(ticket), content);
    }

    public async Task<HttpResponseMessage> DeleteTicket(Ticket ticket)
    {
        return await httpClient.DeleteAsync(TicketApiRoutes.TicketById(ticket));
    }

    public async Task<HttpResponseMessage> CreateTicket(Ticket ticket, User user)
    {
        var payload = new
        {
            data = new
            {
                attributes = new
                {
                    title = ticket.Title,
                    description = ticket.Description,
                    priority = ticket.Priority,
                    type = ticket.Type,
                    status = ticket.Status,
                    error_code = ticket.ErrorCode,
                    reproduction_step = ticket.ReproductionStep
                },
                relationships = new
                {
                    author = new
                    {
                        data = new
                        {
                            id = user.Id
                        }
                    }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        return await httpClient.PostAsync(TicketApiRoutes.BaseTicket(), content);
    }
}