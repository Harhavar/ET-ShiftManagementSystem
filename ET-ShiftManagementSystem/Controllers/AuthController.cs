﻿
using ShiftManagementServises.Servises;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register(Models.RegisterRequest registerRequest)
        {
            //request DTO to Domine Model
            var user = new ShiftMgtDbContext.Entities.User()
            {
                username = registerRequest.username,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                password = registerRequest.password
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
                IsActive = user.IsActive
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
    }
}

