using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce_asp.Models
{
    public class CartItemDBModel
    {
        [Key]
        public int Id { get; set; }

        public int CartModelId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Image { get; set; }

        public int Quanlity { get; set; }

        public decimal Price { get; set; }

        [NotMapped]
        public decimal Total => Quanlity * Price;

        public CartModel Cart { get; set; }

        public ProductModel Product { get; set; }
    }
}
