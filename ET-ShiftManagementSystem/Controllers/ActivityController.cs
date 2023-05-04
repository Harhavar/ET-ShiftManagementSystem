using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ShiftManagementDbContext _context;

        public ActivityController(ShiftManagementDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Activity>> GetActivities()
        {
            return  _context.Activities.ToList();
        }
        [HttpGet("TenetId")]
        public ActionResult<IEnumerable<Activity>> GetActivities(Guid TenetId)
        {
            return _context.Activities.Where(x => x.TenetId == TenetId).ToList();
        }
    }
}
