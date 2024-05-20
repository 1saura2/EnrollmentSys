﻿using ABKS_project.Models;
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
    public class UserController : Controller
    {
        private readonly abksContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(abksContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task<List<User>> GetFilteredUsers(bool isVerified, bool isActive, string roleName, int pageNumber, int pageSize, string? search = null)
        {
            var usersQuery = _context.Users
                .Where(u => u.IsVerified == isVerified && u.IsActive == isActive && !u.Credentials.Any(c => c.Role.RoleName == roleName));

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));
            }

            var totalCount = await usersQuery.CountAsync();

            var users = await usersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentFilter = search;

            return users;
        }

        public async Task<IActionResult> ListUnverified(int pageNumber = 1, int pageSize = 8, string? search = null)
        {
            var users = await GetFilteredUsers(isVerified: false, isActive: false, roleName: "User", pageNumber, pageSize, search);
            return View(users);
        }

        public async Task<IActionResult> ListActive(int pageNumber = 1, int pageSize = 8, string? search = null)
        {
            var users = await GetFilteredUsers(isVerified: true, isActive: true, roleName: "User", pageNumber, pageSize, search);
            return View(users);
        }

        public async Task<IActionResult> ListInactive(int pageNumber = 1, int pageSize = 8, string? search = null)
        {
            var users = await GetFilteredUsers(isVerified: true, isActive: false, roleName: "User", pageNumber, pageSize, search);
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
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    RoleId = 2 
                };

                _context.Credentials.Add(newUserCredential);
                _context.SaveChanges();

                SendWelcomeEmail(user.Email, password);
            }

            return RedirectToAction("ListUnverified", "User");
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
                SendRejectionEmail(user.Email); 
                DeleteUser(user.UserId); 
            }

            return RedirectToAction("ListUnverified", "User");
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
