using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using ABKS_project.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using Khalti;

namespace ABKS_project.Areas.Ecommerce.Controllers
{
    [Area("Ecommerce")]
    [Route("Ecommerce/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly productContext _context;
        private readonly ICartRepository _cartRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(productContext context, ICartRepository cartRepo, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartRepo = cartRepo;
            _httpContextAccessor = httpContextAccessor;
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
                        return Json(new { success = true, paymentUrl = khaltiPaymentResult.payment_url });
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        var errorMessage = "Khalti payment initiation failed.";
                        Console.WriteLine(errorMessage);
                        return BadRequest(errorMessage);
                    }
                }

                await transaction.CommitAsync();
                TempData["Order_Success"] = "Order Have Been Placed Successfully";


                return RedirectToAction("GetOrdersByUserId", "UserOrder", new { area = "Ecommerce" });


            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                // Log dbEx.InnerException.Message to inspect inner details
                Console.WriteLine($"Database update exception: {dbEx.InnerException?.Message ?? dbEx.Message}");
                return StatusCode(500, dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Exception during checkout: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<dynamic> InitiateKhaltiPayment(Order order)
        {
            var url = "https://a.khalti.com/api/v2/epayment/initiate/";

            var amountInPaisa = (int)(order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice));

            var payload = new
            {
                return_url = "https://localhost:7200/Ecommerce/Home/KhaltiCallback",
                website_url = "https://localhost:7200/",
                amount = amountInPaisa,
                purchase_order_id = $"Order-{order.Id}",
                purchase_order_name = $"Order-{order.Id}",
                customer_info = new
                {
                    name = order.Name,
                    email = order.Email,
                    phone = order.MobileNumber
                }
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Key test_secret_key_a50a3b1e01744d60ba828170b7d61962");

            try
            {
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(responseContent);
                }
                else
                {
                    // Handle error response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Khalti API error: {response.StatusCode} - {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Khalti payment initiation failed: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> KhaltiCallback(string token, string txn_id, string amount, string status)
        {
            if (status == "Completed")
            {
                var orderId = int.Parse(token.Replace("Order-", ""));
                var order = await _context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.IsPaid = true;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("OrderSuccess", new { orderId });
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
