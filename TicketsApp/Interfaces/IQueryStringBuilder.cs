namespace TicketsApp.Interfaces;

public interface IQueryStringBuilder
{
    string BuildQueryString<T>(T parameter) where T : class;
}