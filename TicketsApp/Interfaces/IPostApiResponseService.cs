using TicketsApp.Models;
namespace TicketsApp.Interfaces;

public interface IPostApiResponseService
{
    Task<PostApiResponse> ProcessResponse(HttpResponseMessage response);
}