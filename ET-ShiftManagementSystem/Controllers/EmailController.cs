using ET_ShiftManagementSystem.Models;
using ET_ShiftManagementSystem.Servises;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using ET_ShiftManagementSystem.Entities;

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
            try
            {

            
            emailServices.sendEmail(request);

            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
