using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
namespace ABKS_project.Models.EcommerceContent
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        [Required]



        public string UserId { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

}
