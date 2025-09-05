using System.Text;
using System.Text.Json;
using TicketsApp.Interfaces;
using TicketsApp.Models;
using TicketsApp.Utilities;

namespace TicketsApp.Services;

public class AuthorService(
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions,
    IQueryStringBuilder queryStringBuilder) : IAuthorService
{
    public async Task<HttpResponseMessage> GetAuthors(UserQueryParameter parameters)
    {
        var queryString = queryStringBuilder.BuildQueryString(parameters);
        var uri = string.IsNullOrEmpty(queryString)
            ? AuthorApiRoutes.BaseUri()
            : $"{AuthorApiRoutes.BaseUri()}?{queryString}";

        return await httpClient.GetAsync(uri);
    }

    public async Task<HttpResponseMessage> CreateTicket(Ticket ticket, User author)
    {
        var attributes = new Dictionary<string, object>
        {
            { "title", ticket.Title },
            { "description", ticket.Description },
            { "priority", ticket.Priority },
            { "type", ticket.Type },
            { "status", ticket.Status }
        };

        if (!string.IsNullOrEmpty(ticket.ErrorCode)) attributes.Add("error_code", ticket.ErrorCode);

        if (!string.IsNullOrEmpty(ticket.ReproductionStep))
            attributes.Add("reproduction_step", ticket.ReproductionStep);

        var payload = new
        {
            data = new
            {
                attributes
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload, serializerOptions);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        return await httpClient.PostAsync(AuthorApiRoutes.AuthorTicket(author), content);
    }
}