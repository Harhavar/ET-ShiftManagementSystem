using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    public class EmailController : Controller
    {
        private readonly IEmailServices emailServices;

        public EmailController(IEmailServices emailServices )
        {
            this.emailServices = emailServices;
        }

        /// <summary>
        /// Send Email By Giving to mail , subject and body
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public IActionResult SendMail(Email request)
        {
            try
            {
                emailServices.sendEmail(request);

            return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
