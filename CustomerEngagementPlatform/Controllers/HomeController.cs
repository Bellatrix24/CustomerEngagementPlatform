using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.Models;
using CustomerEngagementPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CustomerEngagementPlatform.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalCustomers = _context.Customers.Count(),
                TotalTickets = _context.Tickets.Count(),
                OpenTickets = _context.Tickets.Count(t => t.Status == "Open"),
                ResolvedTickets = _context.Tickets.Count(t => t.Status == "Resolved")
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
                resolvedTickets = await _context.Tickets.CountAsync(t => t.Status == "Resolved")
            };

            return Json(stats);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}