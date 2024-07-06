using ABKS_project.Models;
using ABKS_project.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using BCrypt.Net;
using Newtonsoft.Json;
using ABKS_project.Models.MetaData;
using ABKS_project.Utilities;

namespace ABKS_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly abksContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly EmailService _emailService;

        public HomeController(abksContext context, IWebHostEnvironment env, EmailService emailService)
        {
            _context = context;
            _env = env;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentEvaluation()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ReadMore()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Contact(string name, string email, string subject, string message)
        {
            string body = $"Name: {name}<br/>Email: {email}<br/>Subject: {subject}<br/><br/>{message}";

            string toEmail = "bhishmapoudel408@gmail.com";

            try
            {
                _emailService.SendEmail(toEmail, subject, body);

                TempData["Feedback_success"] = "Your message has been sent succcessfully, Thank you for you time.";

                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {

                TempData["Feedback_error"] = $"Failed to send message. Error: {ex.Message}";

                return RedirectToAction("Contact");
            }
        }

            public IActionResult Images()
        {
            return View();
        } 
        public IActionResult Videos()
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
        public IActionResult Login(LoginViewModel user)
        {
            var userFromDb = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (userFromDb != null)
            {
                var myuser = _context.Credentials
                    .Include(c => c.User)
                    .FirstOrDefault(x => x.UserId == userFromDb.UserId);

                if (myuser != null && BCrypt.Net.BCrypt.Verify(user.Password, myuser.Password))
                {
                    var userType = _context.Roles.FirstOrDefault(u => u.RoleId == myuser.RoleId)?.RoleName;
                    if (userType != null)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, userFromDb.FirstName),
                    new Claim(ClaimTypes.Email, userFromDb.Email),
                    new Claim(ClaimTypes.Role, userType),
                    new Claim("UserId", myuser.UserId.ToString())
                };
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30) 
                        };

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                     
/*                        HttpContext.Session.SetString("UserId", myuser.UserId.ToString());
                        HttpContext.Session.SetInt32("UserIdExpire", 30);*/

                        return RedirectToAction("RedirectToDashboard");
                    }
                }
            }

            TempData["Login_Check"] = "Login User Not Found!";
            return View();
        }





        public IActionResult RedirectToDashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
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
            var lastBatch = _context.Batches.OrderByDescending(b => b.BatchId).FirstOrDefault();
            if (lastBatch != null && lastBatch.IsActive==false)
            {
                return View();
            }
            else
            {
                TempData["Batch_Check"] = "Registration is not available at the moment. Please wait for new session to start.";
                return View();
            }

        }



        [HttpPost]
        public IActionResult Register(UserViewModel usr)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == usr.Email);

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

                string folder = Path.Combine(_env.WebRootPath, "Documents/Citizenships");
                fileName = Guid.NewGuid().ToString() + "_" + usr.CitizenshipPdf.FileName;
                string filePath = Path.Combine(folder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    usr.CitizenshipPdf.CopyTo(fileStream);
                }

                var userViewModelForTempData = new UserViewModelForTempData
                {
                  FirstName = usr.FirstName,
                  LastName = usr.LastName,
                  Email = usr.Email,
                  Age = usr.Age,
                  ContactNumber = usr.ContactNumber,
                  Education = usr.Education,
                  Citizenship = fileName
                };

                string serializedUser = JsonConvert.SerializeObject(userViewModelForTempData);

                string encodedUser = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serializedUser));

                _emailService.SendEmail(usr.Email, "Registration Confirmation",
                $"Please confirm your registration by clicking this <a href=\"{Url.Action("ConfirmEmail", "Home", new { encodedUser }, Request.Scheme)}\">link</a>.");


                /*SendVerificationEmail(usr.Email, encodedUser);*/

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
                    var existingUser = _context.Users.FirstOrDefault(u => u.Email == userViewModelForTempData.Email);

                    if (existingUser == null)
                    {
                        var newUser = new User
                        {
                            FirstName = userViewModelForTempData.FirstName,
                            LastName = userViewModelForTempData.LastName,
                            Email = userViewModelForTempData.Email,
                            Age = userViewModelForTempData.Age,
                            ContactNumber = userViewModelForTempData.ContactNumber,
                            Education = userViewModelForTempData.Education,
                            Citizenship = userViewModelForTempData.Citizenship,
                            IsVerified = false,
                            IsActive = false
                        };

                        _context.Users.Add(newUser);
                        _context.SaveChanges();

                        TempData["Register_Success"] = "You are registered. ";
                        return RedirectToAction("RegisterConfirmation");
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



        /*        private void SendVerificationEmail(string userEmail, string encodedUser)
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
                }*/
        public IActionResult RegisterConfirmation()
        {

            return View();
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


        public IActionResult ViewUserProfile()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var user = _context.Users
                    .Include(u => u.UserBatches)
                        .ThenInclude(ub => ub.Batch)
                    .FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return NotFound();
                }

                var userProfileViewModel = new UserProfileViewModel
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ContactNumber = user.ContactNumber,
                    Education = user.Education,
                    Citizenship = user.Citizenship,
                    Batches = user.UserBatches.Select(ub => ub.Batch).ToList()
                };

                return View(userProfileViewModel);
            }
            else
            {
                return Unauthorized();
            }
        }

        public IActionResult EditUserProfile(Guid id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            var userProfileViewModel = new UserProfileViewModel
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                Education = user.Education,
                Citizenship = user.Citizenship
            };

            return View(userProfileViewModel);
        }

        [HttpPost]
        public IActionResult EditUserProfile(UserProfileViewModel model)
        {

            var user = _context.Users.Find(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.ContactNumber = model.ContactNumber;
            user.Education = model.Education;


            _context.Users.Update(user);

            _context.SaveChanges();

            return RedirectToAction("ViewUserProfile", new { id = user.UserId });

        }



        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    var credential = _context.Credentials.FirstOrDefault(c => c.UserId == userId);

                    if (credential == null || !BCrypt.Net.BCrypt.Verify(model.OldPassword, credential.Password))
                    {
                        ModelState.AddModelError("", "Old password is incorrect.");
                        return RedirectToAction("ViewUserProfile");
                    }

                    credential.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                    _context.SaveChanges();

                    TempData["PasswordChangeSuccess"] = "Password changed successfully!";
                    return RedirectToAction("ViewUserProfile");
                }
                else
                {
                    return Unauthorized();
                }
            }

            return RedirectToAction("ViewUserProfile");
        }
    }
}
