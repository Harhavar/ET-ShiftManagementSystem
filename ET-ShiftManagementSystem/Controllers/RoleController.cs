using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleServices;

        public RoleController(IRoleServices roleServices )
        {
            _roleServices = roleServices;
        }

        [HttpGet]

        public IActionResult GetAction()
        {
            var role = _roleServices.GetRole();

            if ( role == null )
            {
                return NotFound();
            }
            return Ok( role );
        }
    }
}
