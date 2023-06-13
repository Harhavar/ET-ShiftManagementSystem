using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.Authmodel;
using static ET_ShiftManagementSystem.Servises.TokenHandler;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableCors("CorePolicy")]
    public class AuthController : Controller
    {
        public readonly IUserRepository userRepository;
        public readonly ITokenHandler tokenHandler;
        public readonly IEmailSender emailSender;
        public readonly ITenateServices tenateServices;
        public readonly IorganizationServices iorganizationServices;

        //private readonly IEmailServices emailServices;


        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler, IEmailSender emailSender , ITenateServices tenateServices , IorganizationServices iorganizationServices)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
            this.emailSender = emailSender;
            this.tenateServices = tenateServices;
            this.iorganizationServices = iorganizationServices;
            //this.emailServices = emailServices;
        }


        /// <summary>
        /// For Super admin Registartion 
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        //[Authorize(Roles = "SuperAdmin")] //SUPER ADMIN ADDING SuperAdmin Creating Organization 
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                //request DTO to Domine Model
                var user = new ShiftMgtDbContext.Entities.User()
                {
                    username = registerRequest.username,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    password = registerRequest.password,
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenentID= registerRequest.TenateID,

                };

                // pass details to repository 
                user = await userRepository.RegisterAsync(user);

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
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenateID= registerRequest.TenateID,
                };

                return CreatedAtAction(nameof(LoginAync), new { id = UserDTO.id }, UserDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// adding super admin 
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddingSubsriber")]
        //[Authorize(Roles = "Vender,SuperAdmin,Admin")] //SUPER ADMIN AND ADMIN ADDING USER TO THE PROJECT
        public async Task<IActionResult> AddingSubscriber(RegisterRequest registerRequest )
        {
            try
            {
                //request DTO to Domine Model
                var user = new ShiftMgtDbContext.Entities.User()
                {
                    username = registerRequest.username,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    password = registerRequest.password,
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenentID = registerRequest.TenateID,

                };
                var tenent = new Tenate
                {
                    TenateId = registerRequest.TenateID,
                    TenateName = string.Empty,
                };
                // pass details to repository 
                user = await userRepository.RegisterSubscriber(user, tenent);

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
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenateID = registerRequest.TenateID,
                };

                var resetUrl = Url.Action("LoginAync", "Auth", new { username = user.username, password = user.password }, Request.Scheme);



                await emailSender.SendEmailAsync(registerRequest.Email, $"Invitation To Project",
                    $"{user.FirstName + " " + user.LastName} added to the project . use this credential to login UserId :{user.username} password is :{user.password} TenateID is {registerRequest.TenateID} \n Please login by <h1><a href='{resetUrl}'>clicking here</a></h1>.");


                return CreatedAtAction(nameof(LoginAync), new { id = UserDTO.id }, UserDTO);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            

        }


        /// <summary>
        /// Adding User 
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddingUser")]
        //[Authorize(Roles = "SuperAdmin,Admin")] //SUPER ADMIN AND ADMIN ADDING USER TO THE PROJECT
        public async Task<IActionResult> AddingUser(RegisterRequest registerRequest)
        {
            try
            {
                //request DTO to Domine Model
                var user = new ShiftMgtDbContext.Entities.User()
                {
                    username = registerRequest.username,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    password = registerRequest.password,
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenentID = registerRequest.TenateID,
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
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenateID = registerRequest.TenateID,
                };

                var resetUrl = Url.Action("LoginAync", "Auth", new { username = user.username, password = user.password }, Request.Scheme);


                await emailSender.SendEmailAsync(registerRequest.Email, $"Invitation To Project",
                    $"{user.FirstName + " " + user.LastName} added to the project . use this credential to login UserId :{user.username} password is :{user.password} \n Please login by <h1><a href='{resetUrl}'>clicking here</a></h1>.");

                return CreatedAtAction(nameof(LoginAync), new { id = UserDTO.id }, UserDTO);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
           

        }


        /// <summary>
        /// Adding admin    
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddingAdmin")]
        //[Authorize(Roles = "SuperAdmin")] // SUPER ADMIN ADDING ADMIN TO THE PROJECT
        public async Task<IActionResult> AddingAdmin(RegisterRequest registerRequest)
        {
            try
            {
                //request DTO to Domine Model
                var user = new ShiftMgtDbContext.Entities.User()
                {
                    username = registerRequest.username,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    password = registerRequest.password,
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenentID = registerRequest.TenateID,

                };

                // pass details to repository 

                user = await userRepository.RegisterAdminAsync(user);

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
                    ContactNumber = registerRequest.ContactNumber,
                    AlternateContactNumber = registerRequest.AlternateContactNumber,
                    TenateID = registerRequest.TenateID,
                };

                var resetUrl = Url.Action("LoginAync", "Auth", new { username = user.username, password = user.password }, Request.Scheme);


                await emailSender.SendEmailAsync(registerRequest.Email, $"Invitation To Project",
                    $"{user.FirstName + " " + user.LastName} added to the project . use this credential to login UserId :{user.username} password is :{user.password} \n Please login by <h1><a href='{resetUrl}'>clicking here</a></h1>.");
                var tenate = new Tenate
                {
                    //Status = ,
                };
                return CreatedAtAction(nameof(LoginAync), new { id = UserDTO.id }, UserDTO);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            

        }

        /// <summary>
        /// Login by giving Username and password
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [ActionName("LoginAync")]
        //[Authorize(Roles= "SuperAdmin,Admin,User,SystemAdmin")]
        public IActionResult LoginAync(LoginRequest loginRequest)
         {
            try
            {
                var user = userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

                if (user != null)
                {

                    //generate token 
                    var Token = tokenHandler.CreateToken(user);

                    //var result = $"User token : \"{Token.Result}\" ,  userId :\"{user.id}\" , user role : \"{user.Role}\" , OrganizationID \"{user.TenentID}\"";

                    var reponse = new myResponce()
                    {
                        id = user.id,
                        username = user.username,
                        TenentID = user.TenentID,
                        Role = user.Role,
                        Token = Token,
                    };

                    return Ok(reponse);
                }

                return BadRequest("Email / username or password doesn’t match");
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
            
        }

        
        /// <summary>
        /// Forget password request by giving email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ForgetPassword")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> ForgetPassword([FromBody] forgotPasswordRequest request)
        {
            try
            {
                // Check if the provided email address corresponds to a user in the database
                var user = await userRepository.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound();
                }

                // Generate a password reset token and send an email to the user

                //var token = Guid.NewGuid();
                //var save = new Token
                //{
                //    Useremail = user.Email,
                //    UserId = user.id,
                //    UserToken = token.ToString(),
                //    ExpirationDate = DateTime.UtcNow.AddDays(1),
                //    TokenUsed = false

                //};
                //tokenHandler.SaveToken(save);

                //var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token.ToString(), email = user.Email }, Request.Scheme);
                await emailSender.SendEmailAsync(request.Email, "Reset your password",
                    $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <title>Reset Password</title>\r\n  <link rel=\"stylesheet\" href=\"sms.css\">\r\n</head>\r\n<body>\r\n  <h1>Reset Password</h1>\r\n  <p>\r\n    You are receiving this email because you requested a password reset for your account on the Shift Management System. To reset your password, please click the following link:<a href='{$"http://sms.stratusviews.com/reset-password?userId={user.id}"}'><h2>Reset Password</h2></a>\r\n  </p>\r\n  <p>\r\n    If you did not request a password reset, please ignore this email. Your password will not be changed.\r\n  </p>\r\n  <p>\r\n    Thank you,\r\n    <br>\r\n    Shift Management System Team\r\n  </p>\r\n</body>\r\n</html>");

                return Ok($"user id :{user.id}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }

        //[HttpGet]
        //[Route("ResetPassword")]
        ////[Authorize(Roles = "SuperAdmin,Admin,User")]
        //public IActionResult ResetPassword(Guid token, string email)
        //{
        //    // Verify that the provided token is valid

        //    var result = tokenHandler.VerifyUserToken(token.ToString(), email);
        //    if (result.ExpirationDate <= DateTime.Now)
        //    {
        //        return BadRequest("Token expired.");
        //    }

        //    return Ok(result);
        //}


        /// <summary>
        /// Reset your password by adding new password and confirm password 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]

        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> ResetPassword( Guid userId,[FromBody]ResetPasswordViewModel model)
        {
            try
            {
                if (model.Password != model.ConfirmPassword)
                {
                    return BadRequest("Password must be same");
                }
                // Verify that the provided token is valid
                userRepository.UpdateUser(userId, model.Password);
                return Ok("Password updated successfully");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //[HttpPost]
        //[Route("ResetORGPassword")]

        ////[Authorize(Roles = "SuperAdmin,Admin,User")]
        //public async Task<IActionResult> ResetORGPassword(Guid TenentId, [FromBody] ResetPasswordViewModel model)
        //{
        //    if (model.Password != model.ConfirmPassword)
        //    {
        //        return BadRequest("Password must be same");
        //    }
        //    // Verify that the provided token is valid
        //    iorganizationServices.UpdateOrganization(TenentId, model.Password);
        //    return Ok("Password updated succesfully");

        //}

    }
}

