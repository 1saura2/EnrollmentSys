using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ABKS_project.Models.EcommerceContent
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        public int ProductCategoryId {get; set; }

        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
