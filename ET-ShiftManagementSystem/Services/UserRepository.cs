using ShiftMgtDbContext.Entities;
using Microsoft.EntityFrameworkCore;
using ET_ShiftManagementSystem.Servises;
using System.Security.Cryptography.X509Certificates;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.Extensions.Logging;
using org.apache.zookeeper.data;


namespace ET_ShiftManagementSystem.Servises
{
    public interface IUserRepository
    {
        User AuthenticateAsync(string username, string password);

        Task<User> RegisterAsync(User user);
        Task<User> RegisterSubscriber(User user ,Tenate tenate);
        Task<User> RegisterUserAsync(User user);

        User Get(Guid userId);

        Task<User> EditUser(Guid userId, User user);

        Task<User> RegisterAdminAsync(User user);
        Task<User> RegisterORGAdminAsync(User user);

        //Task Update(User user);
        public Task<IEnumerable<User>> GetUser();
        public Task<IEnumerable<User>> GetUser(Guid guid);

       
        Task<User> FindByEmailAsync(string email);

       
        void UpdateUser(Guid userId, string pasword);
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
            var user =  _ShiftManagementDbContext.users.FirstOrDefault(x => x.username.ToLower() == username.ToLower() /*|| x.Email == username.ToLower()*/ && x.password == password);

            if (user == null)
            {
                return null;
            }

            var userRoles =  _ShiftManagementDbContext.usersRoles.Where(x => x.Userid == user.id).ToList();

            //var Role = _ShiftManagementDbContext.roles.Where(x => x.Name == "SystemAdmin").Select(a => a.Name).FirstOrDefault();

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
        public  User Get(Guid userId)
        {
            
            var abc =  _ShiftManagementDbContext.users.FirstOrDefault(u => u.id == userId);

            return abc;
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

      

        public async Task<User> RegisterAsync(User user)
        {
            user.id = Guid.NewGuid();
            await _ShiftManagementDbContext.users.AddAsync(user);
            await _ShiftManagementDbContext.SaveChangesAsync();
            var roleID = _ShiftManagementDbContext.roles.Where(x => x.Name == "SystemAdmin").Select(a=>a.Id).FirstOrDefault();
            var Role = _ShiftManagementDbContext.roles.Where(x => x.Name == "SystemAdmin").Select(a => a.Name).FirstOrDefault();

            user.Role = Role;
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;
            await _ShiftManagementDbContext.SaveChangesAsync();
            if (!string.IsNullOrEmpty(roleID.ToString()))
            {
                var role = new User_Role()
                {
                    Id = Guid.NewGuid(),
                    Userid = user.id,
                    RoleId = roleID
                };
                await _ShiftManagementDbContext.usersRoles.AddAsync(role);
                await _ShiftManagementDbContext.SaveChangesAsync();
            }
            return user;
            
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            user.id = Guid.NewGuid();
            await _ShiftManagementDbContext.users.AddAsync(user);
            //await _ShiftManagementDbContext.SaveChangesAsync();
            var roleID = _ShiftManagementDbContext.roles.Where(x => x.Name == "User").Select(a => a.Id).FirstOrDefault();
            var Role = _ShiftManagementDbContext.roles.Where(x => x.Name == "User").Select(a => a.Name).FirstOrDefault();

            user.Role = Role;
            user.IsActive= true;
            user.CreatedDate = DateTime.Now;
            user.LastName = "";
            user.password = "lkjhgfdsa";
            await _ShiftManagementDbContext.SaveChangesAsync();
            if (!string.IsNullOrEmpty(roleID.ToString()))
            {
                var role = new User_Role()
                {
                    Id = Guid.NewGuid(),
                    Userid = user.id,
                    RoleId = roleID
                };
                await _ShiftManagementDbContext.usersRoles.AddAsync(role);
                await _ShiftManagementDbContext.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> RegisterAdminAsync(User user)
        {
            user.id = Guid.NewGuid();
            await _ShiftManagementDbContext.users.AddAsync(user);
            await _ShiftManagementDbContext.SaveChangesAsync();
            var roleID = _ShiftManagementDbContext.roles.Where(x => x.Name == "Admin").Select(a => a.Id).FirstOrDefault();
            var Role = _ShiftManagementDbContext.roles.Where(x => x.Name == "Admin").Select(a => a.Name).FirstOrDefault();

            user.Role = Role;
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;
            await _ShiftManagementDbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(roleID.ToString()))
            {
                var role = new User_Role()
                {
                    Id = Guid.NewGuid(),
                    Userid = user.id,
                    RoleId = roleID
                };
                await _ShiftManagementDbContext.usersRoles.AddAsync(role);
                await _ShiftManagementDbContext.SaveChangesAsync();
            }
            return user;
        }
        public async Task<User> RegisterORGAdminAsync(User user)
        {
            user.id = Guid.NewGuid();
            user.LastName = "";
            user.password= "password";
            user.ContactNumber = "";
            user.AlternateContactNumber = " ";
            await _ShiftManagementDbContext.users.AddAsync(user);
            await _ShiftManagementDbContext.SaveChangesAsync();
            var roleID = _ShiftManagementDbContext.roles.Where(x => x.Name == "SuperAdmin").Select(a => a.Id).FirstOrDefault();
            var Role = _ShiftManagementDbContext.roles.Where(x => x.Name == "SuperAdmin").Select(a => a.Name).FirstOrDefault();

            user.Role = Role;
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;
            await _ShiftManagementDbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(roleID.ToString()))
            {
                var role = new User_Role()
                {
                    Id = Guid.NewGuid(),
                    Userid = user.id,
                    RoleId = roleID
                };
                await _ShiftManagementDbContext.usersRoles.AddAsync(role);
                await _ShiftManagementDbContext.SaveChangesAsync();
            }
            return user;
        }

        public void UpdateUser(Guid userId, string pasword)
        {
            var user=_ShiftManagementDbContext.users.FirstOrDefault(a => a.id == userId);
            if (user != null)
            {
                user.password = pasword;
                _ShiftManagementDbContext.SaveChanges();
            }
            
        }

        public async Task<User> RegisterSubscriber(User user , Tenate tenate)
        {
            await _ShiftManagementDbContext.Tenates.AddAsync(tenate);
            user.id = Guid.NewGuid();

            await _ShiftManagementDbContext.users.AddAsync(user);
            await _ShiftManagementDbContext.SaveChangesAsync();
            var roleID = _ShiftManagementDbContext.roles.Where(x => x.Name == "SuperAdmin").Select(a => a.Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(roleID.ToString()))
            {
                var role = new User_Role()
                {
                    Id = Guid.NewGuid(),
                    Userid = user.id,
                    RoleId = roleID
                };
                await _ShiftManagementDbContext.usersRoles.AddAsync(role);
                await _ShiftManagementDbContext.SaveChangesAsync();
            }
            return user;

        }

        public async Task<IEnumerable<User>> GetUser()
        {
           return await _ShiftManagementDbContext.users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUser(Guid guid)
        {
           var user = await _ShiftManagementDbContext.users.Where( x => x.TenentID == guid ).ToListAsync();

            if (user == null)
            {
                return null;
            }

            return (IEnumerable<User>)user;
        }

        public async Task<User> EditUser(Guid userId, User user)
        {
            var ExistingUser = _ShiftManagementDbContext.users.FirstOrDefault(x => x.id == userId);

            if (ExistingUser == null)
            {
                return null;
            }

            ExistingUser.username = user.FirstName;
            ExistingUser.Email = user.Email;
            ExistingUser.ContactNumber= user.ContactNumber;
            ExistingUser.AlternateContactNumber= user.AlternateContactNumber;
            ExistingUser.IsActive= user.IsActive;
            ExistingUser.Role= user.Role;

            //ExistingUser.OrganizationLogo = organization.OrganizationLogo;
            //ExistingOrganization.OrganizationName = organization.OrganizationName;
            //ExistingOrganization.StreetLandmark = organization.StreetLandmark;
            //ExistingOrganization.PhoneNumber = organization.PhoneNumber;
            //ExistingOrganization.HouseBuildingNumber = organization.HouseBuildingNumber;
            //ExistingOrganization.Country = organization.Country;
            //ExistingOrganization.PhoneNumber = organization.PhoneNumber;
            //ExistingOrganization.Adminemailaddress = organization.Adminemailaddress;
            //ExistingOrganization.Adminfullname = organization.Adminfullname;
            //ExistingOrganization.EmailAddress = organization.EmailAddress;
            //ExistingOrganization.CityTown = organization.CityTown;
            //ExistingOrganization.StateProvince = organization.StateProvince;
            //ExistingOrganization.LastModifiedDate = DateTime.Now;

            await _ShiftManagementDbContext.SaveChangesAsync();

            return ExistingUser;
        }
    }
}
