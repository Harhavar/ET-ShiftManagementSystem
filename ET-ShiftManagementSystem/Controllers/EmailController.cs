using ET_ShiftManagementSystem.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using ShiftManagementServises.Servises;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EmailController : Controller
    {
        private readonly IEmailServices emailServices;

        public EmailController(IEmailServices emailServices )
        {
            this.emailServices = emailServices;
        }
        [HttpPost]
        public IActionResult SendMail(Email request)
        {
            emailServices.sendEmail(request);

            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse("harshavardhan78143@gmail.com"));
            //email.To.Add(MailboxAddress.Parse("harshavardhan78143@gmail.com"));
            //email.Subject = "SRE Activation";
            //email.Body = new TextPart(TextFormat.Plain) { Text = body };

            //using var SMTP = new SmtpClient();
            //SMTP.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //SMTP.Authenticate("harshavardhan78143@gmail.com", "jrifllvwkowgswfo");
            //SMTP.Send(email);
            //SMTP.Disconnect(true);

            return Ok();
        }
    }
}
