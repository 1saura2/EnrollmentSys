using ABKS_project.Models;
using ABKS_project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace ABKS_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly abksContext context;
        private readonly IWebHostEnvironment env;

        public HomeController(abksContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserViewModel usr)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (usr.Photo != null)
                {
                    string folder = Path.Combine(env.WebRootPath, "Images");
                    fileName = Guid.NewGuid().ToString() + "_" + usr.Photo.FileName;
                    string filePath = Path.Combine(folder, fileName);
                    usr.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                    User user = new User()
                    {
                        FullName = usr.FullName,
                        Email = usr.Email,
                        Age = usr.Age,
                        ContactNumber = usr.ContactNumber,
                        Education = usr.Education,
                        CitizenshipPhoto = fileName,
                        IsVerified = false
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                    TempData["Register_Success"] = "Registered successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(usr);
        }


        public IActionResult Product()
        {
            return View();
        }
    }
}
