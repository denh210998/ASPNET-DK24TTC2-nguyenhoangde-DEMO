using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce_asp.Models
{
    public class OrderItemModel
    {
        [Key]
        public int Id { get; set; }


        public int OrderId { get; set; }
        public OrderModel Order { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Total => Quantity * Price;
    }
}
