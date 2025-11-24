using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        // Thông tin khách hàng
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string Note { get; set; }

        public decimal TotalAmount { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
