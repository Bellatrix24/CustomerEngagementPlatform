using CustomerEngagementPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CustomerEngagementPlatform.Controllers.Api
{
    [Route("api/tickets")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Staff")]
    public class TicketsApiController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsApiController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            var result = tickets.Select(t => new
            {
                t.Id,
                t.Subject,
                t.Description,
                t.Status,
                t.Priority,
                t.AssignedAgent,
                t.CreatedAt,
                t.UpdatedAt,
                CustomerName = t.Customer != null ? t.Customer.Name : "No Customer"
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var result = new
            {
                ticket.Id,
                ticket.Subject,
                ticket.Description,
                ticket.Status,
                ticket.Priority,
                ticket.AssignedAgent,
                ticket.CreatedAt,
                ticket.UpdatedAt,
                CustomerName = ticket.Customer != null ? ticket.Customer.Name : "No Customer"
            };

            return Ok(result);
        }
    }
}