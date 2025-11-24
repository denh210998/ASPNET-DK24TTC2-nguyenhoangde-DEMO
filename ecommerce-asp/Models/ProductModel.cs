using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ecommerce_asp.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
        [MinLength(4, ErrorMessage = "Tên sản phẩm phải ít nhất 4 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
        [MinLength(4, ErrorMessage = "Mô tả phải ít nhất 4 ký tự")]
        public string Description { get; set; }

        public string? Slug { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        [Range(1, 999999999, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập số lượng tồn kho")]
        [Range(0, 999999, ErrorMessage = "Số lượng phải >= 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public CategoryModel? Category { get; set; }


        public string? Image { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Yêu cầu chọn hình ảnh")]
        public IFormFile ImageUpload { get; set; }
    }
}
