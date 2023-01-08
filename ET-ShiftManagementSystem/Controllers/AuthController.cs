
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ET_ShiftManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using ShiftMgtDbContext.Entities;
using ET_ShiftManagementSystem.Servises;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;
        private readonly IEmailSender emailSender;

        //private readonly IEmailServices emailServices;


        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
            this.emailSender = emailSender;
            //this.emailServices = emailServices;
        }
        [HttpPost]
        [Route("Register")]
        //[Authorize(Roles = "SuperAdmin")] SUPER ADMIN ADDING USER TO THE PROJECT
        public async Task<IActionResult> Register(Models.RegisterRequest registerRequest)
        {
            //request DTO to Domine Model
            var user = new ShiftMgtDbContext.Entities.User()
            {
                username = registerRequest.username,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                password = registerRequest.password,
                ContactNumber= registerRequest.ContactNumber,
                AlternateContactNumber= registerRequest.AlternateContactNumber,
                
            };

            // pass details to repository 

            user = await userRepository.RegisterAsync(user);

            //convert back to DTO
            var UserDTO = new Models.UserDto
            {
                id = user.id,
                username = user.username,
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                password = user.password,
                IsActive = user.IsActive,
                ContactNumber = registerRequest.ContactNumber,
                AlternateContactNumber= registerRequest.AlternateContactNumber,
            };

            return CreatedAtAction(nameof(LoginAync), new { id = UserDTO.id }, UserDTO);

        }



        [HttpPost]
        [Route("Login")]
        [ActionName("LoginAync")]
        public  IActionResult LoginAync(Models.LoginRequest loginRequest)
        {
            
            var user =  userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

            if (user != null)
            {
                //generate token 
                var Token = tokenHandler.CreateToken(user);
                return Ok(Token);

            }

            return BadRequest("user name or password is incurrect");
        }


        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] forgotPasswordRequest request)
        {
            // Check if the provided email address corresponds to a user in the database
            var user = await userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound();
            }

            // Generate a password reset token and send an email to the user

            var token = Guid.NewGuid();
            var save = new Token
            {
                Useremail = user.Email,
                UserId = user.id,
                UserToken = token.ToString(),
                ExpirationDate = DateTime.UtcNow.AddDays(1),
                TokenUsed=false
                
            };
            tokenHandler.SaveToken(save);
            
            var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token.ToString(), email= user.Email }, Request.Scheme);
            await emailSender.SendEmailAsync(request.Email, "Reset your password",
                $"Please reset your password by <a href='{resetUrl}'>clicking here</a>.");

            return Ok();
        }

        [HttpGet]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(Guid token,string email)
        {
            // Verify that the provided token is valid

            var result = tokenHandler.VerifyUserToken(token.ToString(),email);
            if (result.ExpirationDate<= DateTime.Now)
            {
                return BadRequest("Token expired.");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password must be same");
            }
            // Verify that the provided token is valid
            userRepository.UpdateUser(model.UserId,model.Password);
            return Ok("Password updated succesfully");

        }


        //[HttpPost]
        //[Route("/forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody] forgotPasswordRequest model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //// Check if the email is valid
        //    //if (!IsValidEmail(model.Email))
        //    //{
        //    //    return BadRequest(new { error = "Invalid email address" });
        //    //}

        //    // Check if the email belongs to a registered user
        //    var user = await userRepository.FindByEmailAsync(model.username);
        //    if (user == null)
        //    {
        //        return BadRequest(new { error = "Email not found" });
        //    }

        //    // Generate a reset token and send it to the user's email
        //    var Token = await tokenHandler.CreateToken(user);

        //    if (Token != null)
        //    {
        //        //add new password 

        //        // confirm password 

        //    }
        //    return Ok(Token);

        //    //

        //}

        //
        //public async Task<IActionResult> ResetPassword([FromBody] forgotPasswordRequest model)
        //{
        //    var user = await userRepository.FindByUserNameAsync(model.username);
        //    if(user != null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound);

        //    }
        //    var token = await userRepository.ForgotPasswordAsync(userRepository);
        //}

    }
}

