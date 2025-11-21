using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required, EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
