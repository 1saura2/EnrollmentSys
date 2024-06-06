using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace ABKS_project.Repositories
{

    public class CartRepository : ICartRepository
    {
        public CartRepository() { }

        public Task<int> AddItem(int bookId, int qty)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCartItemCount(string userId = "")
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetUserCart()
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveItem(int bookId)
        {
            throw new NotImplementedException();
        }
    }
    }

