using CustomerEngagementPlatform.Models;

namespace CustomerEngagementPlatform.Repositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int id);
    }
}