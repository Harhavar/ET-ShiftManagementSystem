//using IDWalks.Models.Domines;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.Authmodel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShiftMgtDbContext.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ET_ShiftManagementSystem.Servises
{
    public interface ITokenHandler
    {
        Task<string> CreateToken(User user);

        Token SaveToken(Token token);

        Token VerifyUserToken(string tokenId,string email);
    }
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration configuration;
        private readonly ShiftManagementDbContext shiftManagementDb;

        public TokenHandler(IConfiguration configuration , ShiftManagementDbContext shiftManagementDb)
        {
            this.configuration = configuration;
            this.shiftManagementDb = shiftManagementDb;
        }
        public Task<string> CreateToken(User user)
        {

            //create claims
            var Claims = new List<Claim>();
            Claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            Claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            Claims.Add(new Claim(ClaimTypes.Email, user.Email));

            //Loop into Roles of user 
            user.Roles.ForEach((roles) =>
            {
                Claims.Add(new Claim(ClaimTypes.Role, roles));
            });

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var Credential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                Claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: Credential);

            var t =  Task.FromResult(new JwtSecurityTokenHandler().WriteToken(Token));

            //var user = userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

            //var t1 = t.Result;

            return  t;
        }

        public Token SaveToken(Token token)
        {

            shiftManagementDb.Tokens.Add(token);
            shiftManagementDb.SaveChanges();
            return token;
        }
        
        public Token VerifyUserToken(string tokenId,string email)
        {
            var token = shiftManagementDb.Tokens.First(x => x.UserToken == tokenId && x.Useremail == email);
            if (token == null)
            {
                return null;
            }

            return token;
        }

        public class myResponce
        {
            public Guid id { get; set; }

            public Guid TenentID { get; set; }
            public string Role { get; set; }

            public Task Token { get; set; }
        }
    }
}
