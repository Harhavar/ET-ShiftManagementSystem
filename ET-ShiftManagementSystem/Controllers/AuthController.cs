
using ShiftManagementServises.Servises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ET_ShiftManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
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
            //verify the incoming request 
            //if (loginRequest == null)
            //{
            //    return BadRequest();

            //}
            //check user is authenticated 
            // check user name and password 
            var user =  userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

            if (user != null)
            {
                //generate token 
                var Token = tokenHandler.CreateToken(user);
                return Ok(Token);

            }

            return BadRequest("user name or password is incurrect");
        }
        //[HttpPost]
        //[Route("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        //{
        //    // Check if the provided email address corresponds to a user in the database
        //    var user = await    .FindByEmailAsync(request.Email);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // Generate a password reset token and send an email to the user
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var resetUrl = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);
        //    await _emailSender.SendEmailAsync(request.Email, "Reset your password",
        //        $"Please reset your password by <a href='{resetUrl}'>clicking here</a>.");

        //    return Ok();
        //}
        //[HttpGet]
        //[Route("reset-password")]
        //public IActionResult ResetPassword(string token)
        //{
        //    // Verify that the provided token is valid
        //    var result = _userManager.VerifyUserTokenAsync(user, "Default", "ResetPassword", token);
        //    if (!result.Result.Succeeded)
        //    {
        //        return BadRequest();
        //    }

        //    return View();
        //}

        //[HttpPost]
        //[Route("reset-password")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    // Verify that the provided token is valid
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok();
        //}


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

