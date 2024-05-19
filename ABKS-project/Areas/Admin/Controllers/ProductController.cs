using Microsoft.AspNetCore.Mvc;

namespace ABKS_project.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
