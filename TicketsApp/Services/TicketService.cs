using System.Text;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Utilities;
namespace TicketsApp.Services;

/// <summary>
/// Provides functionality to manage tickets, including retrieving ticket details
/// with associated data and adding comments to a ticket.
/// </summary>
public class TicketService(HttpClient httpClient, ITicketParser ticketParser, JsonSerializerOptions serializerOptions, IPostApiResponseService postApiResponseService) : ITicketService
{
    /// <summary>
    /// Retrieves a detailed ticket with additional related information, such as comments, author, and engineers, based on the specified ticket.
    /// </summary>
    /// <param name="ticket">The ticket for which detailed information is required.</param>
    /// <returns>
    /// A <see cref="TicketWithIncludes"/> object containing the ticket details with included related data,
    /// or <c>null</c> if the operation is unsuccessful or the response status is not successful.
    /// </returns>
    public async Task<TicketWithIncludes?> GetTicketWithIncludes(Ticket ticket)
    {
        var response =
            await httpClient.GetAsync(
                TicketApiRoutes.GetTicketWithIncludesUri(ticket.Id, "comment,author,engineer"));

        if (response.IsSuccessStatusCode)
        {
            return await ticketParser.ParseTicketWithIncludes(response);
        }

        return null;
    }

    /// <summary>
    /// Adds a comment to the specified ticket.
    /// </summary>
    /// <param name="comment">The comment to be added to the ticket.</param>
    /// <param name="ticket">The ticket to which the comment will be added.</param>
    /// <returns>A <see cref="PostApiResponse"/> containing the success status and any error details if the operation fails.</returns>
    public async Task<PostApiResponse> AddComment(string comment, Ticket ticket)
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

        var response = await httpClient.PostAsync(TicketApiRoutes.AddComment(ticket.Id), content);

        return await postApiResponseService.ProcessResponse(response);
    }
}