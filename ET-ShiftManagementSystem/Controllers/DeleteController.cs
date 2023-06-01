using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class DeleteController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public DeleteController(IUserRepository repository)
        {
            _repository = repository;
        }
        [HttpDelete]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult DeleteUser(Guid Id)
        {
            if (Guid.Empty == Id)
            {
                return BadRequest();
            }
            try
            {
                var responce = _repository.DeleteUser(Id);
                return Ok(responce);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
