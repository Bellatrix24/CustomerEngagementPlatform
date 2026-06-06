using CustomerEngagementPlatform.Models;

namespace CustomerEngagementPlatform.Tests
{
    public class TicketModelTests
    {
        [Fact]
        public void Ticket_DefaultStatus_ShouldBeOpen()
        {
            var ticket = new Ticket();

            Assert.Equal("Open", ticket.Status);
        }

        [Fact]
        public void Ticket_DefaultPriority_ShouldBeMedium()
        {
            var ticket = new Ticket();

            Assert.Equal("Medium", ticket.Priority);
        }

        [Fact]
        public void Ticket_CanSetSubject()
        {
            var ticket = new Ticket
            {
                Subject = "Login issue"
            };

            Assert.Equal("Login issue", ticket.Subject);
        }
    }
}