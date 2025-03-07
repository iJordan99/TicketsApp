// using System.Text.Json;
// using TicketsApp.Interfaces;
// using TicketsApp.Models;
//
// namespace TicketsApp.Services;
//
// public class TicketService(HttpClient httpClient, IRestApiDataParser restApiDataParser) : ITicketService
// {
//     public async Task<TicketData?> GetTicketData(Ticket ticket)
//     {
//         var response =
//             await httpClient.GetAsync(
//                 $"https://tickets.test/api/v1/tickets/{ticket.Id}?include=comment,author,engineer");
//
//         if (response.IsSuccessStatusCode)
//         {
//             var json = await response.Content.ReadAsStringAsync();
//             using var doc = JsonDocument.Parse(json);
//
//             var root = doc.RootElement.GetProperty("data").GetProperty("includes");
//
//             var engineers = restApiDataParser.ParseIncludedData(root, "engineer", restApiDataParser.ParseUser);
//             var author = restApiDataParser.ParseIncludedData(root, "author", restApiDataParser.ParseUser)
//                 .FirstOrDefault();
//             var comments = restApiDataParser.ParseIncludedData(root, "comments", restApiDataParser.ParseComment);
//
//             return new TicketData(ticket, engineers, author, comments);
//         }
//
//         return null;
//     }
//
//     public Task AddComment(Comment newComment)
//     {
//         throw new NotImplementedException();
//     }
// }