using System.ComponentModel.DataAnnotations;

namespace CustomerEngagementPlatform.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(150)]
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}