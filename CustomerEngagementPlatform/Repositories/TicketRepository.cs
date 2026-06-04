using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerEngagementPlatform.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Customer)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}