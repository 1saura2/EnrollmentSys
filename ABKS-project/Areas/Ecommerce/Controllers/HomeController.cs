using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using ABKS_project.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using Khalti;
using System.Diagnostics;

namespace ABKS_project.Areas.Ecommerce.Controllers
{
    [Area("Ecommerce")]
    [Route("Ecommerce/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly productContext _context;
        private readonly ICartRepository _cartRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public HomeController(productContext context, ICartRepository cartRepo, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _cartRepo = cartRepo;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListProduct()
        {
            var productsInStock = _context.Products
                                          .Include(p => p.ProductCategory) 
                                          .Where(p => p.InStock == true)
                                          .ToList();
            return View(productsInStock);
        }



        [HttpGet]
        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }
        public IActionResult Checkout()
        {
            return View();
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue("UserId");
        }

        private async Task<ShoppingCart> GetCart(string userId)
        {
            return await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IActionResult> RemoveItems(int productId)
        {
            var cartCount = await _cartRepo.RemoveAllItems(productId);
            return RedirectToAction("GetUserCart");
        }


        [HttpGet]
        public IActionResult DoCheckOut()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DoCheckout(Checkout model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User is not logged-in");

                var cart = await GetCart(userId);
                if (cart == null)
                    return BadRequest("Invalid cart");

                var cartDetail = _context.CartDetails.Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    return BadRequest("Cart is empty");

                var pendingRecord = _context.OrderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord == null)
                    return BadRequest("Order status does not have Pending status");

                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatusId = pendingRecord.Id
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                }

                _context.CartDetails.RemoveRange(cartDetail);
                await _context.SaveChangesAsync();

                if (model.PaymentMethod == "Khalti")
                {
                    var khaltiPaymentResult = await InitiateKhaltiPayment(order);
                    if (khaltiPaymentResult != null && khaltiPaymentResult.payment_url != null)
                    {
                        await transaction.CommitAsync();
                        return View("Redirect", khaltiPaymentResult.payment_url);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return BadRequest("Khalti payment initiation failed.");
                    }
                }

                await transaction.CommitAsync();
                TempData["Order_Success"] = "Order Have Been Placed Successfully";

                return RedirectToAction("GetOrdersByUserId", "UserOrder", new { area = "Ecommerce" });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Database update exception occurred.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Exception occurred during checkout.");
            }
        }
        private async Task<dynamic> InitiateKhaltiPayment(Order order)
        {
            var khaltiKey = _configuration["KhaltiSettings:Key"];
            var khaltiUrl = _configuration["KhaltiSettings:Url"];

            var amountInPaisa = (int)(order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice));

            var payload = new
            {
                return_url = "https://localhost:7200/Ecommerce/Home/KhaltiCallback/",
                website_url = "https://localhost:7200/",
                amount = amountInPaisa,
                purchase_order_id = $"Order-{order.Id}",
                purchase_order_name = $"Order-{order.Id}",
                customer_info = new
                {
                    name = order.Name,
                    email = order.Email,
                    phone = order.MobileNumber
                },
            };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Key {khaltiKey}");

            try
            {
                var response = await client.PostAsync(khaltiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return responseObject;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<IActionResult> KhaltiCallback(string pidx, string transaction_id, string tidx, string amount, string total_amount, string mobile, string status, string purchase_order_id, string purchase_order_name)
        {
            if (status == "Completed")
            {
                var orderId = int.Parse(purchase_order_id.Replace("Order-", ""));

                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.IsPaid = true;
                    await _context.SaveChangesAsync();

                    TempData["Order_Success"] = "Order Have Been Placed Successfully";

                    return RedirectToAction("GetOrdersByUserId", "UserOrder", new { area = "Ecommerce" });
                }
            }

            return RedirectToAction("OrderFailed");
        }



        public IActionResult OrderSuccess(int orderId)
        {
            return View(orderId);
        }

        public IActionResult OrderFailed()
        {
            return View();
        }


    }
}
