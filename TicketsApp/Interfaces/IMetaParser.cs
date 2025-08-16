using TicketsApp.Models;

namespace TicketsApp.Interfaces;

public interface IMetaParser
{
    Task<MetaData> Parse(HttpResponseMessage response);
}