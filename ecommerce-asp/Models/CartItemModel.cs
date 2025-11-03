namespace ecommerce_asp.Models
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quanlity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Quanlity * Price; }
        }
        public CartItemModel()
        {

        }
        public string Image { get; set; }
        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Price = product.Price;
            Quanlity = 1;
            Image = product.Image;
        }
    }
}
