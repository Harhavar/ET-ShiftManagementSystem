using ShiftMgtDbContext.Data;
using ShiftMgtDbContext.Entities;
using Microsoft.EntityFrameworkCore;
using ShiftManagementServises.Servises;
using System.Security.Cryptography.X509Certificates;

namespace ShiftManagementServises.Servises
{
    public interface IUserRepository
    {
        User AuthenticateAsync(string username, string password);

        Task<User> RegisterAsync(User user);

        Task<User> ForgotPasswordAsync(string Email);
        Task<User> FindByEmailAsync(string email);
    }
    public class UserRepository : IUserRepository

    {
        private readonly ShiftManagementDbContext _ShiftManagementDbContext;

        public UserRepository(ShiftManagementDbContext ShiftManagementDbContext)
        {
            this._ShiftManagementDbContext = ShiftManagementDbContext;
        }
        public  User AuthenticateAsync(string username, string password)
        {
            var user =  _ShiftManagementDbContext.users.FirstOrDefault(x => x.username.ToLower() == username.ToLower() && x.password == password);

            if (user == null)
            {
                return null;
            }

            var userRoles =  _ShiftManagementDbContext.usersRoles.Where(x => x.Userid == user.id).ToList();

            //user.Roles=  _ShiftManagementDbContext.roles.Where(x => x.Id == user.id).Select(a=>a.Id.ToString()).ToList();
            if (userRoles.Any())
            {
                user.Roles = new List<string>();
                foreach (var item in userRoles)
                {
                    var role = _ShiftManagementDbContext.roles.FirstOrDefault(x => x.Id == item.RoleId);
                    if (role != null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }

            user.password = null;
            return user;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _ShiftManagementDbContext.users.FirstOrDefaultAsync(e => e.Email == email);
            if (user == null)
            {
                return null;

            }

            return user;
        }

        public Task<User> ForgotPasswordAsync(string Email)
        {
            throw new NotImplementedException();
        }

        //public async Task<User> ForgotPasswordAsync(string email)
        //{
        //    var user = await _ShiftManagementDbContext.users.FirstOrDefaultAsync( e => e.Email == email);
        //    if (user == null)
        //    {
        //        return null;

        //    }
        //    var token = await _ShiftManagementDbContext.GeneratePasswordResetAsync(user);


        //}

        public async Task<User> RegisterAsync(User user /*,Role AssignRole*/ )
        {
            user.id = Guid.NewGuid();
            await _ShiftManagementDbContext.users.AddAsync(user);
            await _ShiftManagementDbContext.SaveChangesAsync();

            //need to check roleand cerate a record acordingly
            //if (AssignRole == )
            //{

            //}
            //var role = _ShiftManagementDbContext.roles.Where(x => x.Name == "User");


            ////eg Admin>> take this roleid from role table and add userid

            //_ShiftManagementDbContext.usersRoles = role.Where(x => x.Id = );

            return user;
            
        }
    }
}
