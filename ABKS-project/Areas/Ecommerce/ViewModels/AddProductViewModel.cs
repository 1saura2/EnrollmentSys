namespace ABKS_project.Areas.Ecommerce.ViewModels
{
    public class AddProductViewModel
    {
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public IFormFile Photo { get; set; }
        public int? ProductCategoryId { get; set; }
        public bool InStock { get; set; }
    }
}
