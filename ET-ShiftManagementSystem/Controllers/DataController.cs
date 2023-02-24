using ET_ShiftManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Controllers
{
    public class DataController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShiftManagementDbContext dbContext;

        public DataController(IHttpContextAccessor httpContextAccessor, ShiftManagementDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        //[HttpGet]
        //public IActionResult GetData()
        //{
        //    var tenantId = _httpContextAccessor.HttpContext.Items["TenantId"] as string;
        //    // Use the tenantId to filter the data
        //    var data = GetDataForTenant(tenantId);
        //    return Ok(data);
        //}

        //private IEnumerable<> GetDataForTenant(string tenantId)
        //{
        //    // Retrieve the data for the tenant from the database
        //    var data = dbContext.users.Where(x => x.TenateID.ToString() == tenantId);
        //    return data.ToString();
        //}
       // using (var memorystream = new MemoryStream() )
       //{
       // }

    }

}
