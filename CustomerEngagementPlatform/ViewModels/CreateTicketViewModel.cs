using System.ComponentModel.DataAnnotations;

namespace CustomerEngagementPlatform.ViewModels
{
    public class CreateTicketViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(80)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(120)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority is required")]
        [StringLength(30)]
        public string Priority { get; set; } = "Medium";
    }
}
