using ET_ShiftManagementSystem.Entities;
using Microsoft.Data.SqlClient;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Models.UserModel
{
    public class UserInfoDataAccesslayer
    {
        public UserCredential GetUserCredential(UserCredential userCredential)
        {
            var userInfo = new UserCredential();

            using (SqlConnection conn = new SqlConnection("ProjectAPIConnectioString"))
            {
                string sql = string.Format(@"");
            }
            return userInfo;
        }
    }
}
