using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Models.Authmodel;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.UserModel;
using ET_ShiftManagementSystem.Services;
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
       // private readonly ShiftManagementDbContext shiftManagementDb;
        private readonly IEmailSender emailSender;

        public UserController(IUserRepository userRepository, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            //this.shiftManagementDb = shiftManagementDb;
            this.emailSender = emailSender;
        }

        /// <summary>
        /// Get All User in Application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                var user =  userRepository.GetUser();
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
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Get Users Count in Perticular application 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CountUsers/{tenantId}")]
        public IActionResult GetUser(Guid tenentId)
        {
            if (Guid.Empty  == tenentId)
            {
                return BadRequest();

            }
            try
            {
                var user =  userRepository.GetUserCount(tenentId);


                return Ok(user);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }


        /// <summary>
        /// getting users in the single organization 
        /// </summary>
        /// <param name="TenentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Tenent/users/{TenentID}")]
        public async Task<IActionResult> GetUsers(Guid TenentID)
        {
            try
            {
                var user = await userRepository.GetUser(TenentID);

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
                        Email = user.Email,
                        ContactNumber = user.ContactNumber,
                        AlternateContactNumber = user.AlternateContactNumber,

                        //CreatedDate = user.CreatedDate,
                    };
                    OrganizationRequest.Add(organizationRequest);

                });

                return Ok(OrganizationRequest);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        /// <summary>
        /// getting single user Details
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Tenent/users/{UserId}")]
        public async  Task<IActionResult> Getuser(Guid UserId)
        {
            try
            {
                var user = userRepository.Get(UserId);

                if (user == null)
                {
                    return NotFound();
                }
                var OrganizationRequest = new List<GetUserRequest>();

                var organizationRequest = new GetUserRequest()
                {
                    username = user.username,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    Email = user.Email,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,
                };
                OrganizationRequest.Add(organizationRequest);

                return Ok(OrganizationRequest);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// adding user inside the organization by Admin
        /// </summary>
        /// <param name="Tenentid"></param>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUser/{Tenentid}")]
        public async Task<IActionResult> AddUser( Guid Tenentid ,AddUserRequest userRequest)
        {
            if(Guid.Empty == Tenentid || userRequest == null)
            {
                return BadRequest();
            }
            try
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

                if(user == null)
                {
                    return BadRequest();
                }
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
                    $"use this credential to login UserId :{user.username} \n Please reset ur password  <h1><a href='{"http://localhost:3000/resetpassword"}'>click here</a></h1>.");

                return CreatedAtAction(nameof(Getuser), new { id = UserDTO.id }, UserDTO);
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        /// <summary>
        /// update User Details 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{UserId:guid}")]
        public async Task<IActionResult> UpdateUser(Guid UserId ,[FromBody] UpdateUserRequest updateUserRequest)
        {
            if (Guid.Empty == UserId || updateUserRequest == null)
            {
                return BadRequest();
            }
            try
            {
                var user = new User()
                {
                    FirstName = updateUserRequest.FirstName,
                    Email = updateUserRequest.Email,
                    ContactNumber = updateUserRequest.ContactNumber,
                    AlternateContactNumber = updateUserRequest.AlternateContactNumber,
                    IsActive = updateUserRequest.IsActive,
                };

                user = await userRepository.EditUser(UserId, user);

                if (user == null)
                {
                    return NotFound();
                }

                var UpdateUser = new Models.UserModel.UserDto()
                {
                    id = user.id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    username = user.username,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,
                    IsActive = user.IsActive,
                    password = user.password,
                    TenateID = user.TenentID,
                    Role = user.Role,

                };

                return Ok(UpdateUser);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet("Assigned-Project")]
        public IActionResult GetAssignedProject(Guid userid)
        {
            if (Guid.Empty == userid)
            {
                return BadRequest();
            }
            try
            {
                var Responce = userRepository.AssignedProject(userid);
                if (Responce == null)
                {
                    return null;
                }

                return Ok(Responce.Count);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        //[HttpDelete]
        //public async Task<IActionResult> DeleteUser(Guid UserId)
        //{
        //    var delete = await userRepository.DeleteUser(UserId);
        //    if (delete == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(delete);
        //}
    }
}