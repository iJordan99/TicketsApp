namespace TicketsApp.Interfaces;

public interface IEngineerService
{
    Task<HttpResponseMessage> GetEngineers();
}