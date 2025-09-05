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
}