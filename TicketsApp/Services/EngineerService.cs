using TicketsApp.Interfaces;
using TicketsApp.Utilities;

namespace TicketsApp.Services;

public class EngineerService(HttpClient httpClient) : IEngineerService
{
    public async Task<HttpResponseMessage> GetEngineers()
    {
        return await httpClient.GetAsync(
            EngineerApiRoutes.GetEngineersUrl());
    }
}