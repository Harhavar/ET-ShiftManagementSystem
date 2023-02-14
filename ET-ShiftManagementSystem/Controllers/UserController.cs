using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.UserModel;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = await userRepository.GetUser();
            if (user == null)
            {
                return NotFound();

            }
            var OrganizationRequest = new List<GetUserRequest>();

            user.ToList().ForEach(user =>
            {
                var organizationRequest = new GetUserRequest()
                {
                    username = user.username,
                    Role= user.Role,
                    IsActive= user.IsActive,
                    ContactNumber= user.ContactNumber,
                    AlternateContactNumber= user.AlternateContactNumber,
                    Email= user.Email,
                    CreatedDate= user.CreatedDate,
                };
                OrganizationRequest.Add(organizationRequest);

            });

            return Ok(OrganizationRequest);
        }
    }
}
