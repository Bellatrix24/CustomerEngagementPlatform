using CustomerEngagementPlatform.Data;
using CustomerEngagementPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerEngagementPlatform.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.Include(c => c.Tickets).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Phone,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedAt = DateTime.Now;

                _context.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Email,Phone,Address,CreatedAt")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var hasActiveTickets = customer.Tickets.Any(t => t.Status == "Open" || t.Status == "In Progress");
            ViewBag.HasActiveTickets = hasActiveTickets;
            if (hasActiveTickets)
            {
                ViewBag.ErrorMessage = "Cannot delete this customer because they have active tickets. Please resolve or close their tickets first.";
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var customer = await _context.Customers
                .Include(c => c.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer != null)
            {
                var hasActiveTickets = customer.Tickets.Any(t => t.Status == "Open" || t.Status == "In Progress");
                if (hasActiveTickets)
                {
                    ViewBag.HasActiveTickets = true;
                    ViewBag.ErrorMessage = "Cannot delete this customer because they have active tickets. Please resolve or close their tickets first.";
                    return View(customer);
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int? id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}