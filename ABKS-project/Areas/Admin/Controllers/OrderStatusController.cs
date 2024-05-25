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
    public class OrderStatusController : Controller
    {
        private readonly productContext _context;

        public OrderStatusController(productContext context)
        {
            _context = context;

        }

        public IActionResult OrderStatusFetch()
        {
            var OrderStatus=_context.OrderStatuses.ToList();    
            return View(OrderStatus);
        }
    }
}
