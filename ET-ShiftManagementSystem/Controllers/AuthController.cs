﻿using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ShiftMgtDbContext.Entities;
using ET_ShiftManagementSystem.Servises;
using ET_ShiftManagementSystem.Entities;
using System.Data;
using ET_ShiftManagementSystem.Services;
using Org.BouncyCastle.Asn1.Cmp;
using ET_ShiftManagementSystem.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static System.Net.WebRequestMethods;
using ET_ShiftManagementSystem.Models.Authmodel;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableCors("CorePolicy")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;
        private readonly IEmailSender emailSender;
        private readonly ITenateServices tenateServices;
        private readonly IorganizationServices iorganizationServices;

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

        [HttpPost]
        [Route("AddingSubsriber")]
        //[Authorize(Roles = "Vender,SuperAdmin,Admin")] //SUPER ADMIN AND ADMIN ADDING USER TO THE PROJECT
        public async Task<IActionResult> AddingSubscriber(RegisterRequest registerRequest )
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
                TenateName=string.Empty,
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

        [HttpPost]
        [Route("AddingUser")]
        //[Authorize(Roles = "SuperAdmin,Admin")] //SUPER ADMIN AND ADMIN ADDING USER TO THE PROJECT
        public async Task<IActionResult> AddingUser(RegisterRequest registerRequest)
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


        [HttpPost]
        [Route("AddingAdmin")]
        //[Authorize(Roles = "SuperAdmin")] // SUPER ADMIN ADDING ADMIN TO THE PROJECT
        public async Task<IActionResult> AddingAdmin(RegisterRequest registerRequest)
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
                TenentID =registerRequest.TenateID,

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
                TenateID= registerRequest.TenateID,
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


        [HttpPost]
        [Route("Login")]
        [ActionName("LoginAync")]
        //[Authorize(Roles= "SuperAdmin,Admin,User,SystemAdmin")]
        public IActionResult LoginAync(LoginRequest loginRequest)
        {
            var user = userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

            if (user != null)
            {
                
                //generate token 
                var Token = tokenHandler.CreateToken(user);

                var result = $"User token : \"{Token.Result}\" ,  userId :\"{user.id}\" , user role : \"{user.Role}\"";

                return Ok(result);
            }

            return BadRequest("Email / username or password doesn’t match");
        }


        [HttpPost]
        [Route("ForgetPassword")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
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
                TokenUsed = false

            };
            tokenHandler.SaveToken(save);

            //var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token.ToString(), email = user.Email }, Request.Scheme);
            await emailSender.SendEmailAsync(request.Email, "Reset your password",
                $"Please reset your password by <a href='{"http://localhost:3000/ResetPaswword"}'>clicking here</a>.");

            return Ok($"user id : {user.id}");
        }

        [HttpGet]
        [Route("ResetPassword")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public IActionResult ResetPassword(Guid token, string email)
        {
            // Verify that the provided token is valid

            var result = tokenHandler.VerifyUserToken(token.ToString(), email);
            if (result.ExpirationDate <= DateTime.Now)
            {
                return BadRequest("Token expired.");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("ResetPassword")]

        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> ResetPassword( Guid userId,[FromBody]ResetPasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password must be same");
            }
            // Verify that the provided token is valid
            userRepository.UpdateUser(userId, model.Password);
            return Ok("Password updated succesfully");

        }
        [HttpPost]
        [Route("ResetORGPassword")]

        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> ResetORGPassword(Guid TenentId, [FromBody] ResetPasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password must be same");
            }
            // Verify that the provided token is valid
            iorganizationServices.UpdateOrganization(TenentId, model.Password);
            return Ok("Password updated succesfully");

        }

    }
}

