using CustomerEngagementPlatform.Models;
using System.Collections.Generic;

namespace CustomerEngagementPlatform.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ResolvedTickets { get; set; }
        public int InProgressTickets { get; set; }

        // Priority Stats
        public int HighPriorityTickets { get; set; }
        public int MediumPriorityTickets { get; set; }
        public int LowPriorityTickets { get; set; }

        // Service efficiency calculation
        public double ResolutionRate { get; set; }

        // Recent activity
        public List<Ticket> RecentTickets { get; set; } = new List<Ticket>();

        // Unassigned Tickets info
        public int UnassignedTickets { get; set; }
        public List<Ticket> UnassignedTicketsList { get; set; } = new List<Ticket>();
    }
}