//using AutoMapper;
//using ET_ShiftManagementSystem.Data;
//using ET_ShiftManagementSystem.Entities;
//using ET_ShiftManagementSystem.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace ET_ShiftManagementSystem.Controllers
//{
//    [ApiController]
//    [Route("[Controller]")]
//    //[Authorize("Vender")]
//    public class TenateController : Controller
//    {
//        private readonly ITenateServices _tenateServices;
//        private readonly IMapper mapper;
//        private readonly ShiftManagementDbContext dbContext;

//        public TenateController(ITenateServices tenateServices, IMapper mapper , ShiftManagementDbContext dbContext)
//        {
//            _tenateServices = tenateServices;
//            this.mapper = mapper;
//            this.dbContext = dbContext;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetTenate()
//        {
//            var tenate = await _tenateServices.GetTenates();

//            if(tenate == null)
//            {
//                return NotFound();
//            }
//            var tenateDto = mapper.Map<List<Models.TenateDTO>>(tenate);  

//            return Ok(tenateDto);
//        }

//        [HttpPost]
//        public IActionResult AddSubscriber([FromBody] Tenate subscriber)
//        {
//            subscriber.Status = 0; // Assign a default status value
//            dbContext.Tenates.Add(subscriber);
//            dbContext.SaveChanges();

//            return Ok(subscriber.TenateId +" " + "use this tenent id for the future reference");
//        }

//        [HttpPut("{id}")]
//        public IActionResult UpdateStatus(int id)
//        {
//            var subscriber = dbContext.Tenates.Find(id);
//            if (subscriber == null)
//            {
//                return NotFound();
//            }

//            subscriber.Status += 1; // Increment the status value when user is added into the application 
//            dbContext.SaveChanges();

//            return Ok(subscriber.Status);
//        }

//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            var delete = dbContext.Tenates.Find(id);

//            if(delete == null)
//            {
//                return NotFound();

//            }
//            dbContext.Tenates.Remove(delete);
//            dbContext.SaveChanges();
//            return Ok(delete);
//        }
//    }
//}
