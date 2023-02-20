using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Models.Authmodel;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.UserModel;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using org.apache.zookeeper.data;
using ShiftMgtDbContext.Entities;
using static System.Net.WebRequestMethods;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ShiftManagementDbContext shiftManagementDb;
        private readonly IEmailSender emailSender;

        public UserController(IUserRepository userRepository, ShiftManagementDbContext shiftManagementDb , IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.shiftManagementDb = shiftManagementDb;
            this.emailSender = emailSender;
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

        [HttpGet]
        [Route("Tenent/users")]
        public async Task<IActionResult> GetUsers(Guid TenentID)
        {
            var user = await userRepository.GetUser(TenentID);

            if(user == null)
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
                    Email = user.Email,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,
                    
                    //CreatedDate = user.CreatedDate,
                };
                OrganizationRequest.Add(organizationRequest);

            });

            return Ok(OrganizationRequest);
        }

        [HttpGet]
        [Route("Tenent/users/Id")]
        public async  Task<IActionResult> Getuser(Guid UserId)
        {
            var user = userRepository.Get(UserId);

            if(user == null)
            {
                return NotFound();
            }
            var OrganizationRequest = new List<GetUserRequest>();

           // user.ToList().ForEach(user =>
            //{
                var organizationRequest = new GetUserRequest()
                {
                    username = user.username,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    Email = user.Email,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,

                    //CreatedDate = user.CreatedDate,
                };
                OrganizationRequest.Add(organizationRequest);

            //});

            return Ok(OrganizationRequest);
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser( Guid Tenentid ,AddUserRequest userRequest)
        {
            //request DTO to Domine Model
            var user = new ShiftMgtDbContext.Entities.User()
            {
                FirstName = userRequest.FirstName,
                // LastName = registerRequest.LastName,
                Email = userRequest.Email,
                //password = registerRequest.password,
                ContactNumber = userRequest.ContactNumber,
                AlternateContactNumber = userRequest.AlternateContactNumber,
                username = userRequest.FirstName,
                TenentID = Tenentid
            };

            // pass details to repository 

            user = await userRepository.RegisterUserAsync(user);

            //convert back to DTO
            var UserDTO = new Models.UserModel.UserDto
            {
                id = user.id,
                username = user.username,
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                password = user.password,
                IsActive = user.IsActive,
                ContactNumber = user.ContactNumber,
                AlternateContactNumber = user.AlternateContactNumber,
                TenateID = user.TenentID,
            };

            //var resetUrl = Url.Action("LoginAync", "Auth", new { username = user.username, password = user.password }, Request.Scheme);


            await emailSender.SendEmailAsync(user.Email, $"Invitation To Project",
                $" use this credential to login UserId :{user.username} \n Please reset ur password  <h1><a href='{"http://localhost:3000/resetpassword"}'>click here</a></h1>.");

            return CreatedAtAction(nameof(Getuser), new { id = UserDTO.id }, UserDTO);
        }

        [HttpPut]
        [Route("{UserId:guid}")]
        public async Task<IActionResult> EditUser(Guid UserId ,[FromBody] UpdateUserRequest updateUserRequest)
        {
            var user = new User()
            {
                FirstName = updateUserRequest.FirstName,
                Email = updateUserRequest.Email,
                ContactNumber = updateUserRequest.ContactNumber,
                AlternateContactNumber = updateUserRequest.AlternateContactNumber,
                IsActive = updateUserRequest.IsActive,
            };

            user =  await userRepository.EditUser(UserId, user);

            if(user == null)
            {
                return NotFound();
            }

            var Euser = new Models.UserModel.UserDto()
            {
                id = user.id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                username = user.username,
                ContactNumber=user.ContactNumber,
                AlternateContactNumber= user.AlternateContactNumber,
                IsActive=user.IsActive,
                password=user.password,
                TenateID= user.TenentID,
                Role=user.Role,

            };

            return Ok(Euser);

        }
    }
}