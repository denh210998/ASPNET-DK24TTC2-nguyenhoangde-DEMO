using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models.ViewModels
{
    public class CheckoutInputViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public string Note { get; set; }
    }
}
