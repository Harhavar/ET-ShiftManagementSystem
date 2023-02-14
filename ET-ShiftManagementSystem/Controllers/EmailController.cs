using ET_ShiftManagementSystem.Models;
using ET_ShiftManagementSystem.Servises;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Cors;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableCors("CorePolicy")]
    public class EmailController : Controller
    {
        private readonly IEmailServices emailServices;

        public EmailController(IEmailServices emailServices )
        {
            this.emailServices = emailServices;
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
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
