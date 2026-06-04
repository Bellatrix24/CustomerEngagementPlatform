using System.ComponentModel.DataAnnotations;

namespace CustomerEngagementPlatform.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Open";

        [StringLength(30)]
        public string Priority { get; set; } = "Medium";

        [StringLength(80)]
        public string AssignedAgent { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}