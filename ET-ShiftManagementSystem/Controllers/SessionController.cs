using ET_ShiftManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetSessions()
        {
            List<string> SessionInfo = new List<string>();

            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariable.SessionKeyUserName)))
            {
                HttpContext.Session.SetString(SessionKeyEnum.SessionKeyUserName.ToString(), "current user");
                HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionId.ToString(), Guid.NewGuid().ToString()); 
            }
            var username = HttpContext.Session.GetString(SessionVariable.SessionKeyUserName);
            var SesssionId = HttpContext.Session.GetString(SessionVariable.SessionKeySessionId);

            SessionInfo.Add(username);
            SessionInfo.Add(SesssionId);

            return SessionInfo;
        }
    }
}
