using CustomerEngagementPlatform.Models;

namespace CustomerEngagementPlatform.Services
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int id);
    }
}