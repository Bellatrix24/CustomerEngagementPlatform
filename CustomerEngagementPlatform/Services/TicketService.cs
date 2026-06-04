using CustomerEngagementPlatform.Models;
using CustomerEngagementPlatform.Repositories;

namespace CustomerEngagementPlatform.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _ticketRepository.GetAllTicketsAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _ticketRepository.GetTicketByIdAsync(id);
        }
    }
}