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

namespace ABKS_project.Areas.Admin.Controllers
{
   /* [Authorize(Policy = "AdminOnly")]*/
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly abksContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(abksContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListUnverified()
        {
            var users = _context.Users.Where(u => u.IsVerified == false).ToList();
            return View(users);
        }

        public IActionResult ListActive()
        {
            var users = _context.Users.Where(u => u.IsActive == true).ToList();
            return View(users);
        }

        public IActionResult ListInActive()
        {
            var users = _context.Users.Where(u => u.IsActive == false).ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult AcceptUser(Guid userId)
        {
            var user = _context.Users.Include(u => u.Credentials).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                user.IsVerified = true;
                user.IsActive = true;

                _context.SaveChanges();
                var password = "123";

                var newUserCredential = new Credential
                {
                    UserId = user.UserId,
                    Password = BCrypt.Net.BCrypt.HashPassword(password), // Default password
                    RoleId = 2 // Assuming 2 is the User role ID
                };

                _context.Credentials.Add(newUserCredential);
                _context.SaveChanges();

                SendWelcomeEmail(user.Email, password); // Send welcome email with default password
            }

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

        [HttpPost]
        public IActionResult RejectUser(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                SendRejectionEmail(user.Email); // Send rejection email
                DeleteUser(user.UserId); // Delete user
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
                mailMessage.From = new MailAddress(senderEmail, "ABKS TEAM");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Regarding Your Form Submission";
                mailMessage.Body = $"Dear user,\n\nWe regret to inform you that your form submission has been rejected. If you believe there was an error, please fill the form again with correct information or contact support for assistance.\n\nBest regards,\nThe Platform Team";

                client.Send(mailMessage);
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(Guid userId)
        {
            var user = _context.Users.Find(userId);


            string fileName = user.Citizenship;

            _context.Users.Remove(user);

            var credential = _context.Credentials.FirstOrDefault(c => c.UserId == userId);
            if (credential != null)
            {
                _context.Credentials.Remove(credential);
            }

            _context.SaveChanges();

            if (!string.IsNullOrEmpty(fileName))
            {
                string pdfPath = Path.Combine(_env.WebRootPath, "Documents/Citizenships", fileName);

                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
            }

            return RedirectToAction(nameof(ListActive));
        }


        public IActionResult StartNewSession()
        {
            var activeUsers = _context.Users.Where(u => u.IsActive == true).ToList();

            foreach (var user in activeUsers)
            {
                user.IsActive = false;
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(ListActive));
        }

        [HttpPost]
        public IActionResult ReEnrollUser(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(ListUnverified));
        }
    }
}
