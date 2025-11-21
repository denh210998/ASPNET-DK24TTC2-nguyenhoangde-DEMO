using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        // Lưu mật khẩu đã hash, không lưu plain text
        [Required]
        public string PasswordHash { get; set; }
    }
}
