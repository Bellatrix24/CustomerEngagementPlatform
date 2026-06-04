using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CustomerEngagementPlatform.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string statusFilter, string priorityFilter)
        {
            var tickets = _context.Tickets
                .Include(t => t.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(t =>
                    t.Subject.Contains(searchString) ||
                    t.Description.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                tickets = tickets.Where(t => t.Status == statusFilter);
            }

            if (!string.IsNullOrEmpty(priorityFilter))
            {
                tickets = tickets.Where(t => t.Priority == priorityFilter);
            }

            return View(await tickets.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Subject,Description,Status,Priority,AssignedAgent,CustomerId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.CreatedAt = DateTime.Now;
                ticket.UpdatedAt = null;

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", ticket.CustomerId);
            return View(ticket);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", ticket.CustomerId);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Subject,Description,Status,Priority,AssignedAgent,CreatedAt,CustomerId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.UpdatedAt = DateTime.Now;

                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", ticket.CustomerId);
            return View(ticket);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int? id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}