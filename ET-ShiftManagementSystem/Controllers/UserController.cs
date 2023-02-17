using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.UserModel;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ShiftManagementDbContext shiftManagementDb;

        public UserController(IUserRepository userRepository, ShiftManagementDbContext shiftManagementDb)
        {
            this.userRepository = userRepository;
            this.shiftManagementDb = shiftManagementDb;
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
                    Role = user.Role,
                    IsActive = user.IsActive,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,
                    Email = user.Email,
                    CreatedDate = user.CreatedDate,
                };
                OrganizationRequest.Add(organizationRequest);

            });

            return Ok(OrganizationRequest);
        }
        [HttpGet]
        [Route("Tenent")]
        public async Task<IActionResult> GetUser(Guid tenentId)
        {
            var user = await shiftManagementDb.users.Where(u => u.TenentID == tenentId).ToListAsync();
            if (user == null)
            {
                return NotFound();

            }
            var result = $"{user.Count} ";
            return Ok(user);
        }
    }
}