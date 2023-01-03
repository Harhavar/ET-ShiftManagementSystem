using MimeKit.Text;
using MimeKit;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace ShiftManagementServises.Servises
{
    public interface IEmailServices
    {
        void sendEmail(Email Request);
    }
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
           _configuration = configuration;
        }
        public void sendEmail(Email Request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailHost").Value));
            email.To.Add(MailboxAddress.Parse(Request.to));
            email.Subject = Request.subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = Request.body };

            using var SMTP = new SmtpClient();
            SMTP.Connect(_configuration.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            SMTP.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value);
            SMTP.Send(email);
            SMTP.Disconnect(true);
        }
    }
}
