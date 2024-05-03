using ABKS_project.Models;
using ABKS_project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                var existingUser = context.Users.FirstOrDefault(u => u.Email == usr.Email);
             
                if (existingUser != null)
                {
                    if ((bool)existingUser.IsVerified)
                    {
                       
                        TempData["Register_Check"] = "User is Registered and Verified.";
                        return View();
                    }
                    else
                    {
                      
                        TempData["Register_Check"] = "User is Registered and Yet to be Verified.";
                        return View();
                    }
                   
                }

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

                    TempData["Register_Success"] = "You are registered.";
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
