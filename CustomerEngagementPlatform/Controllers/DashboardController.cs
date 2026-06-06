using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerEngagementPlatform.Controllers
{
    [Authorize(Roles = "Staff")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _context;

        public DashboardController(
            ILogger<DashboardController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var totalTickets = _context.Tickets.Count();
            var resolvedTickets = _context.Tickets.Count(t => t.Status == "Resolved");
            var resolutionRate = totalTickets > 0 ? (double)resolvedTickets / totalTickets * 100 : 0.0;

            var model = new DashboardViewModel
            {
                TotalCustomers = _context.Customers.Count(),
                TotalTickets = totalTickets,
                OpenTickets = _context.Tickets.Count(t => t.Status == "Open"),
                ResolvedTickets = resolvedTickets,
                InProgressTickets = _context.Tickets.Count(t => t.Status == "In Progress"),

                HighPriorityTickets = _context.Tickets.Count(t => t.Priority == "High"),
                MediumPriorityTickets = _context.Tickets.Count(t => t.Priority == "Medium"),
                LowPriorityTickets = _context.Tickets.Count(t => t.Priority == "Low"),

                ResolutionRate = Math.Round(resolutionRate, 1),

                RecentTickets = _context.Tickets
                    .Include(t => t.Customer)
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .ToList(),

                UnassignedTickets = _context.Tickets.Count(t => string.IsNullOrEmpty(t.AssignedAgent)),
                UnassignedTicketsList = _context.Tickets
                    .Include(t => t.Customer)
                    .Where(t => string.IsNullOrEmpty(t.AssignedAgent))
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = new
            {
                totalCustomers = await _context.Customers.CountAsync(),
                totalTickets = await _context.Tickets.CountAsync(),
                openTickets = await _context.Tickets.CountAsync(t => t.Status == "Open"),
                resolvedTickets = await _context.Tickets.CountAsync(t => t.Status == "Resolved"),
                unassignedTickets = await _context.Tickets.CountAsync(t => string.IsNullOrEmpty(t.AssignedAgent))
            };

            return Json(stats);
        }
    }
}
