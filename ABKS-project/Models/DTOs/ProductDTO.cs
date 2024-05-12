using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ABKS_project.Models.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }

        
        [Required]
        public double ProductPrice { get; set; }
        public string? ProductImage { get; set; }
        [Required]
        public int ProductCategoryId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? ProductCategoryList { get; set; }
    }
}
