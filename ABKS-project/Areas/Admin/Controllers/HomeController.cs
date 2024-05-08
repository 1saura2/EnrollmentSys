using ABKS_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ABKS_project.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        abksContext context;
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

        public IActionResult ListUnverified()
        {
            var users = context.Users.Where(u => u.IsVerified == false).ToList();
            return View(users);
          
        }     
        public IActionResult ListActive()
        {
            var users = context.Users.Where(u => u.IsActive == true).ToList();
            return View(users);
          
        }     
        
        
        public IActionResult ListInActive()
        {
            var users = context.Users.Where(u => u.IsActive == false).ToList();
            return View(users);
          
        }


        [HttpPost]
        public IActionResult AcceptUser(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            var userEmail = user.Email;

            user.IsVerified = true;
            user.IsActive = true;
            context.Users.Update(user);
            context.SaveChanges();

            string password = "123";
            string hashedPassword = HashPassword(password);

            var newCredential = new Credential
            {
                Email = userEmail,
                Password = hashedPassword,
                RoleId = 2 
            };
            context.Credentials.Add(newCredential);
            context.SaveChanges();

            SendWelcomeEmail(userEmail, password);

            int registrationTypeId = 1; 

            var userRegistrationType = new UserRegistrationType
            {
                UserId = userId,
                RegistrationTypeId = registrationTypeId
            };
            context.UserRegistrationTypes.Add(userRegistrationType);
            context.SaveChanges();

            return RedirectToAction("ListUnverified", "Home");
        }


        private void SendWelcomeEmail(string email, string password)
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
                mailMessage.To.Add(email);
                mailMessage.Subject = "Welcome to the platform!";
                mailMessage.Body = $"Dear user,\n\nYour account has been accepted. Here are your login credentials:\nEmail: {email}\nPassword: {password}\n\nYou can now log in to your account using these credentials.\n\nBest regards,\nThe Platform Team";

          
                client.Send(mailMessage);
            }
        }



    private string HashPassword(string password)
    {
  
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        return hashedPassword;
    }


        [HttpPost]
        public IActionResult RejectUser(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var userEmail = user.Email;

                DeleteUser(user.UserId);

                SendRejectionEmail(userEmail);
            }

            return RedirectToAction("ListUnverified", "Home");
        }


        private void SendRejectionEmail(string email)
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
                mailMessage.From = new MailAddress(senderEmail,"ABKS TEAM");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Regarding Your Form Submission";
                mailMessage.Body = $"Dear user,\n\nWe regret to inform you that your form submission has been rejected. If you believe there was an error, please fill the form again with correct information or contact support for assistance.\n\nBest regards,\nThe Platform Team";

                client.Send(mailMessage);
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            var user = context.Users.Find(userId);
            var userEmail = user.Email;

            string fileName = user.CitizenshipPhoto;

            context.Users.Remove(user);

            var credential = context.Credentials.FirstOrDefault(c => c.Email == userEmail);
            if (credential != null)
            {
                context.Credentials.Remove(credential);
            }

            context.SaveChanges();

            if (!string.IsNullOrEmpty(fileName))
            {
                string imagePath = Path.Combine(_env.WebRootPath, "Images", fileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            return RedirectToAction(nameof(ListActive));
        }


        public IActionResult StartNewSession()
        {
            var activeUsers = context.Users.Where(u => (bool)u.IsActive).ToList();
            foreach (var user in activeUsers)
            {
                user.IsActive = false;
                context.Users.Update(user);
            }
            context.SaveChanges();

            return RedirectToAction("ListActive", "Home");
        }

        [HttpPost]
        public IActionResult ReEnrollUser(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                user.IsActive = true; 
                context.SaveChanges();
            }

            return RedirectToAction("ListUnverified", "Home");
        }




    }
}
