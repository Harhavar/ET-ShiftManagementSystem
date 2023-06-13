using ET_ShiftManagementSystem.Models.UserModel;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        [EnableCors("CorePolicy")]
        //[Authorize(Roles ="SystemAdmin,Admin,User")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]
        public IActionResult GetUser()
        {
            try
            {
                var user = userRepository.GetUser();
                if (user == null)
                {
                    return NotFound();

                }
                var OrganizationRequest = new List<GetUserRequest>();

                user.ToList().ForEach(user =>
                {
                    var organizationRequest = new GetUserRequest()
                    {
                        UserId = user.id,
                        TenantId = user.TenentID,

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
        [EnableCors("CorePolicy")]
        [Route("CountUsers/{tenantId}")]
        //[Authorize(Roles ="SystemAdmin, Admin,User")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public IActionResult GetUser(Guid tenantId)
        {
            if (Guid.Empty == tenantId)
            {
                return BadRequest();

            }
            try
            {
                var user = userRepository.GetUserCount(tenantId);


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
        [EnableCors("CorePolicy")]
        [Route("Tenent/users/{TenentID}")]
        [Authorize(Roles ="SystemAdmin, Admin,User")]

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
                        UserId = user.id,
                        TenantId = user.TenentID,
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
        [Route("{UserId}")]
        [EnableCors("CorePolicy")]
        [Authorize(Roles ="SystemAdmin, Admin ,User")]

        public async Task<IActionResult> Getuser(Guid UserId)
        {
            try
            {
                var user = userRepository.Get(UserId);

                if (user == null)
                {
                    return NotFound();
                }
                //var OrganizationRequest = new List<GetUserRequest>();

                var organizationRequest = new GetUserRequest()
                {
                    UserId = user.id,
                    TenantId = user.TenentID,
                    username = user.username,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    Email = user.Email,
                    ContactNumber = user.ContactNumber,
                    AlternateContactNumber = user.AlternateContactNumber,
                };
                // OrganizationRequest.Add(organizationRequest);

                return Ok(organizationRequest);
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
        [EnableCors("CorePolicy")]
        [Authorize(Roles ="SystemAdmin, Admin,SuperAdmin")]
        public async Task<IActionResult> AddUser(Guid Tenentid, AddUserRequest userRequest)
        {
            if (Guid.Empty == Tenentid || userRequest == null)
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

                if (user == null)
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
                    Role= user.Role
                };

                await emailSender.SendEmailAsync(user.Email, "Reset your password",
                    $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <title>Reset Password</title>\r\n  <link rel=\"stylesheet\" href=\"sms.css\">\r\n</head>\r\n<body>\r\n  <h1>Reset Password</h1>\r\n  <p>\r\n    You are receiving this email because you requested a password reset for your account on the Shift Management System. To reset your password, please click the following link:<a href='{$"http://sms.stratusviews.com/reset-password?userId={user.id}"}'><h2>Reset Password</h2></a>\r\n  </p>\r\n  <p>\r\n    If you did not request a password reset, please ignore this email. Your password will not be changed.\r\n  </p>\r\n  <p>\r\n    Thank you,\r\n    <br>\r\n    Shift Management System Team\r\n  </p>\r\n</body>\r\n</html>");


                return CreatedAtAction(nameof(Getuser), new { id = UserDTO.id }, UserDTO);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// update User Profile 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [EnableCors("CorePolicy")]
        [Route("UpdateProfile/{UserId}")]
        [Authorize(Roles = "SystemAdmin , User, Admin")]
        public async Task<IActionResult> UpdateUser(Guid UserId, [FromBody] UpdateUserProfileRequest updateUserRequest)
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
                    //IsActive = updateUserRequest.IsActive,
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
        [HttpPut]
        [Route("UpdateUser/{UserId}")]
        [EnableCors("CorePolicy")]
        [Authorize(Roles = "SystemAdmin, Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateUser(Guid UserId, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (Guid.Empty == UserId || updateUserRequest == null)
            {
                return BadRequest();
            }
            try
            {
                var user = new User()
                {
                    FirstName = updateUserRequest.UserName,
                    Email = updateUserRequest.Email,
                    ContactNumber = updateUserRequest.ContactNumber,
                    AlternateContactNumber = updateUserRequest.AlternateContactNumber,
                    IsActive = updateUserRequest.IsActive,
                    Role = updateUserRequest.Role,
                };

                user = await userRepository.UpdateUser(UserId, user);

                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);

            }
        }

        /// <summary>
        /// Assigned project to the perticular user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("Assigned-Project")]
        [EnableCors("CorePolicy")]
        [Authorize(Roles = "SystemAdmin,Admin, User")]
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
        /// <summary>
        /// Delete user 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>

        [HttpDelete("Delete/{UserId}")]
        [EnableCors("CorePolicy")]
        [Authorize(Roles = "SystemAdmin , Admin,SuperAdmin")]
        public IActionResult DeleteUser(Guid UserId)
        {
            if (Guid.Empty == UserId)
            {
                return BadRequest();
            }
            try
            {
                var delete = userRepository.DeleteUser(UserId);
                if (delete == false)
                {
                    return NotFound();
                }

                return Ok(delete);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}