using Microsoft.AspNetCore.Mvc;

namespace ABKS_project.Areas.Product.Controllers
{
    public class HomeController : Controller
    {
        [Area("Product")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
