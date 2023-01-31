//using ShiftMgtDbContext.Data;
//using ShiftMgtDbContext.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShiftManagementServises.Servises
//{
    
    
//        public interface IUserServices
//        {
//            User GetUserDetails(int UserId);
//        }

//        public class UserServices : IUserServices
//        {
//            private readonly ShiftManagementDbContext _shiftDbContext;
//            public UserServices(ShiftManagementDbContext shiftDbContext)
//            {
//                _shiftDbContext = shiftDbContext;
//            }
           
//            public User GetUserDetails(int UserId)
//            {
//            return _shiftDbContext.users.FirstOrDefault(a => a.UserId == UserId);
//            }
//        }
    
//}
