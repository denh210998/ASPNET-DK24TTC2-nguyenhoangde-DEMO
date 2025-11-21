using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ecommerce_asp.Models.ViewModels
{
    public class CheckoutPageViewModel
    {
        [BindNever]
        [ValidateNever]
        public CartItemViewModel Cart { get; set; } = new();
        public CheckoutInputViewModel Checkout { get; set; } = new();
    }
}
