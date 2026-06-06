using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.Models;
using CustomerEngagementPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerEngagementPlatform.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerPortalController : Controller
    {
        private readonly ILogger<CustomerPortalController> _logger;
        private readonly ApplicationDbContext _context;

        public CustomerPortalController(
            ILogger<CustomerPortalController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.Identity?.Name;
            var customer = await _context.Customers
                .Include(c => c.Tickets)
                .FirstOrDefaultAsync(c => c.Email == email);

            var tickets = customer?.Tickets.OrderByDescending(t => t.CreatedAt).ToList() ?? new List<Ticket>();
            return View(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTicket()
        {
            var email = User.Identity?.Name ?? string.Empty;
            var model = new CreateTicketViewModel { Email = email };

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer != null)
            {
                model.Name = customer.Name;
                model.Phone = customer.Phone;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket(CreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = string.Empty,
                        CreatedAt = DateTime.Now
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }

                var ticket = new Ticket
                {
                    Subject = model.Subject,
                    Description = model.Description,
                    Priority = model.Priority,
                    Status = "Open",
                    CreatedAt = DateTime.Now,
                    CustomerId = customer.Id
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ticket raised successfully! Check status updates in 'My Tickets'.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
