using ABKS_project.Models.EcommerceContent;
using Humanizer.Localisation;

namespace ABKS_project.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public string STerm { get; set; } = "";
        public int GenreId { get; set; } = 0;
    }
}
