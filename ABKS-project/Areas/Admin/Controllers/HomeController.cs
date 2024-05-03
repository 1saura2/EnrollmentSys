using ABKS_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ABKS_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        abksContext context;
        public HomeController(abksContext context)
        {
                this.context = context;
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
        public IActionResult ListVerified()
        {
            var users = context.Users.Where(u => u.IsVerified == true).ToList();
            return View(users);
          
        }


        [HttpPost]
        public IActionResult AcceptUser(int userId)
        {
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            var userEmail = user.Email;

            user.IsVerified = true;
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

                context.Users.Remove(user);
                context.SaveChanges();

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
            context.Users.Remove(user);

            var credential = context.Credentials.FirstOrDefault(c => c.Email == userEmail);
            if (credential != null)
            {
                context.Credentials.Remove(credential);
            }

            context.SaveChanges();

            return RedirectToAction(nameof(ListVerified));
        }


    }
}
