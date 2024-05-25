using ABKS_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BCrypt.Net;
using ABKS_project.Areas.Product.Models;


namespace ABKS_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly productContext _context;

        public OrderController(productContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrderFetch()
        {
            var Order=_context.Orders.ToList(); 
            return View(Order);
        }
    }
}
