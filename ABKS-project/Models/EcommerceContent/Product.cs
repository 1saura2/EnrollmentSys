using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ABKS_project.Models.EcommerceContent
{
    [Table("Product")]
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(40)]
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public String? ProductImage { get; set; }
        [Required]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock {get;set;}

        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
    }
}
