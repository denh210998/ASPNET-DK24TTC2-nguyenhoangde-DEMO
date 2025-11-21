using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        public string? SessionId { get; set; }

        public string? UserId { get; set; }  // để sau này nếu login thì merge cart

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<CartItemDBModel> CartItems { get; set; } = new();
    }
}
