namespace TicketsApp.Models;

public class MetaData(int currentPage, int lastPage, int total)
{
    public readonly int LastPage = lastPage;
    public readonly int Total = total;
    public int CurrentPage = currentPage;
}