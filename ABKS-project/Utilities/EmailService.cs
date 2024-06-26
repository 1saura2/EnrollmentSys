using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ABKS_project.Utilities
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body, bool isHtml = true)
        {
            string smtpServer = _configuration["EmailSettings:SmtpServer"];
            int port = int.Parse(_configuration["EmailSettings:Port"]);
            string senderEmail = _configuration["EmailSettings:SenderEmail"];
            string senderPassword = _configuration["EmailSettings:SenderPassword"];

            using (SmtpClient client = new SmtpClient(smtpServer, port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "ABKS TEAM"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                mailMessage.To.Add(toEmail);

                client.Send(mailMessage);
            }
        }
    }
}
