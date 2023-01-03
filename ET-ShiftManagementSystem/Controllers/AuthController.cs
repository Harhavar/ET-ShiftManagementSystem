
using ShiftManagementServises.Servises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ET_ShiftManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> LoginAync(Models.LoginRequest loginRequest)
        {
            //verify the incoming request 
            //if (loginRequest == null)
            //{
            //    return BadRequest();

            //}
            //check user is authenticated 
            // check user name and password 
            var user = await userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

            if (user != null)
            {
                //generate token 
                var Token = await tokenHandler.CreateToken(user);
                return Ok(Token);

            }

            return BadRequest("user name or password is incurrect");
        }

        [HttpPost]
        [Route("api/forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] forgotPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //// Check if the email is valid
            //if (!IsValidEmail(model.Email))
            //{
            //    return BadRequest(new { error = "Invalid email address" });
            //}

            // Check if the email belongs to a registered user
            var user = await userRepository.FindByEmailAsync(model.username);
            if (user == null)
            {
                return BadRequest(new { error = "Email not found" });
            }

            // Generate a reset token and send it to the user's email
            var Token = await tokenHandler.CreateToken(user);

            if (Token != null)
            {
                //add new password 

                // confirm password 

            }
            return Ok(Token);

            //

        }

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

