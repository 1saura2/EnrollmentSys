using ABKS_project.Areas.Ecommerce.Models;

namespace ABKS_project.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(Checkout model);
        Task<bool> RemoveAllItems(int productId);
    }
}
