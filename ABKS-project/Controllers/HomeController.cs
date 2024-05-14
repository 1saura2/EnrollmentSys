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
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Newtonsoft.Json;

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
                    var claims = new[]
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
                    if ((bool)!existingUser.IsVerified)
                    {
                       
                        TempData["Email_Confirmation_Message"] = "User Already Register and not Verified yet";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["Register_Check"] = "User Already Registered and Verified.";
                        return RedirectToAction("Register");
                    }
                }

                string fileName = "";

                string folder = Path.Combine(_env.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "_" + usr.Photo.FileName;
                string filePath = Path.Combine(folder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    usr.Photo.CopyTo(fileStream);
                }

                var userViewModelForTempData = new UserViewModelForTempData
                {
                    FullName = usr.FullName,
                    Email = usr.Email,
                    Age = usr.Age,
                    ContactNumber = usr.ContactNumber,
                    Education = usr.Education,
                    CitizenshipPhoto = fileName
                };

                string serializedUser = JsonConvert.SerializeObject(userViewModelForTempData);

                string encodedUser = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serializedUser));

                SendVerificationEmail(usr.Email, encodedUser);

                TempData["Email_Confirmation_Message"] = "Please check your email for registration confirmation.";

                return RedirectToAction("Register");
            }
            return View(usr);
        }


        public IActionResult ConfirmEmail(string encodedUser)
        {
            if (!string.IsNullOrEmpty(encodedUser))
            {
                string serializedUser = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUser));

                var userViewModelForTempData = JsonConvert.DeserializeObject<UserViewModelForTempData>(serializedUser);

                if (userViewModelForTempData != null && !string.IsNullOrEmpty(userViewModelForTempData.Email))
                {
                    var existingUser = context.Users.FirstOrDefault(u => u.Email == userViewModelForTempData.Email);

                    if (existingUser == null)
                    {
                        var newUser = new User
                        {
                            FullName = userViewModelForTempData.FullName,
                            Email = userViewModelForTempData.Email,
                            Age = userViewModelForTempData.Age,
                            ContactNumber = userViewModelForTempData.ContactNumber,
                            Education = userViewModelForTempData.Education,
                            CitizenshipPhoto = userViewModelForTempData.CitizenshipPhoto,
                            IsVerified = false,
                            IsActive = false
                        };

                        context.Users.Add(newUser);
                        context.SaveChanges();

                        TempData["Register_Success"] = "You are registered.";
                        return RedirectToAction("Login"); 
                    }
                    else
                    {
                        TempData["Confirmation_Message"] = "User Already Exists.";
                        return RedirectToAction("Register"); 
                    }
                }
            }

            TempData["Confirmation_Message"] = "Invalid verification link.";
            return RedirectToAction("Register"); 
        }



        private void SendVerificationEmail(string userEmail, string encodedUser)
        {
            string smtpServer = "smtp.gmail.com";
            int port = 587;
            string senderEmail = "atul.baral8421@gmail.com";
            string senderPassword = "nemm arey koqy bmvm\r\n";

            using (SmtpClient client = new SmtpClient(smtpServer, port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderEmail, "ABKS TEAM");
                mailMessage.To.Add(userEmail);
                mailMessage.Subject = "Registration Confirmation";
                mailMessage.Body = $"Please confirm your registration by clicking this <a href=\"{Url.Action("ConfirmEmail", "Home", new { encodedUser }, Request.Scheme)}\">link</a>.";

                mailMessage.IsBodyHtml = true;

                client.Send(mailMessage);
            }
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
