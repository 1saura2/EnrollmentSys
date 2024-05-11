using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABKS_project.Models.EcommerceContent
{
    [Table("Order")]
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? OrderEmail { get; set; }
        [Required]
        public string? OrderMobNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string? OrderAddress { get; set; }
        [Required]
        [MaxLength(30)]
        public string? OrderPaymentMethod { get; set; }
        public bool IsPaid { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }

}
