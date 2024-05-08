using ABKS_project.Models;
using ABKS_project.Models.MetaData;
using ABKS_project.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace ABKS_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly abksContext context;
        private readonly IWebHostEnvironment _env;

        public HomeController(abksContext context, IWebHostEnvironment env)
        {
            this.context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("RedirectToDashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(ValidCredential user)
        {
            var myuser = context.Credentials.FirstOrDefault(x => x.Email == user.Email);

            if (myuser != null && BCrypt.Net.BCrypt.Verify(user.Password, myuser.Password))
            {
                var userType = context.Roles.FirstOrDefault(u => u.RoleId == myuser.RoleId)?.RoleName;
                if (userType != null)
                {
                   
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, userType)
            };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // persist cookie after browser is closed
                    };
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    var userId = context.Users.FirstOrDefault(u => u.Email == user.Email)?.UserId;
                    if (userId != null)
                    {
                        HttpContext.Session.SetInt32("UserId", userId.Value);
                    }

                    return RedirectToAction("RedirectToDashboard");
                }
            }
            else
            {
                TempData["Login_Check"] = "Login User Not Found!";
            }
            return View();
        }



        public IActionResult RedirectToDashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
        }




        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("RedirectToDashboard");
            }
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
                        TempData["Register_Check"] = "User Already Registered and Verified.";
                        return View();
                    }
                    else
                    {
                        TempData["Register_Check"] = "User Already Registered and Yet to be Verified.";
                        return View();
                    }
                }

                string fileName = "";
                if (usr.Photo != null)
                {
                    string folder = Path.Combine(_env.WebRootPath, "Images");
                    fileName = Guid.NewGuid().ToString() + "_" + usr.Photo.FileName;
                    string filePath = Path.Combine(folder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        usr.Photo.CopyTo(fileStream);
                    }

                    User user = new User()
                    {
                        FullName = usr.FullName,
                        Email = usr.Email,
                        Age = usr.Age,
                        ContactNumber = usr.ContactNumber,
                        Education = usr.Education,
                        CitizenshipPhoto = fileName,
                        IsVerified = false,
                        IsActive = false
                    };
                    context.Users.Add(user);
                    context.SaveChanges();

                    TempData["Register_Success"] = "You are registered.";
                    return RedirectToAction("Index");
                }
            }
            return View(usr);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }


        public IActionResult Product()
        {
            return View();
        }
    }
}