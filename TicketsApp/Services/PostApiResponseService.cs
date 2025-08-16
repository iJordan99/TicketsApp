using TicketsApp.Interfaces;
using TicketsApp.Models;

namespace TicketsApp.Services;

public class PostApiResponseService(IErrorParser errorParser) : IPostApiResponseService
{
    public async Task<PostApiResponse> ProcessResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return new PostApiResponse(true, null);

        var error = await errorParser.Parse(response);

        return new PostApiResponse(false, error);
    }
}