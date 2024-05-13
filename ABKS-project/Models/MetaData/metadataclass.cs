using System.ComponentModel.DataAnnotations;

namespace ABKS_project.Models.MetaData
{
    public class CredentialMetaData
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public partial class CartDetailMetaData
    {
        public int CartId { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public partial class OrderMetaData
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrderStatusId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? OrderName { get; set; }
        public string? OrderEmail { get; set; }
        public string? OrderMobNumber { get; set; }
        public string? OrderAddress { get; set; }
        public string? OrderPaymentMethod { get; set; }
        public bool? IsPaid { get; set; }
    }

    public partial class OrderDetailMetaData
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
        public int OrderId { get; set; }
    }
    public partial class OrderStatusMetaData
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
    }


    public partial class ProductMetaData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
    }

    public partial class ProductCategoryMetaData
    {
        public int ProductCategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
    public partial class ShoppingCartMetaData
    {
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public partial class StockMetaData
    {
        public int StockId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
