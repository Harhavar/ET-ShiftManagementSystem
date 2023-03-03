//using ET_ShiftManagementSystem.Data;
//using Microsoft.AspNetCore.Mvc;
//using MimeKit;

//namespace ET_ShiftManagementSystem.Controllers
//{
//    [ApiController]
//    [Route("[Controller]")]
//    public class DocumentController : Controller
//    {
//        private readonly ShiftManagementDbContext _dbContext;

//        public DocumentController(ShiftManagementDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        [HttpPost("upload")]
//        public async Task<IActionResult> UploadFile(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//                return BadRequest("File is empty.");

//            if (file.Length > 10485760) // 10MB limit
//                return BadRequest("File size limit exceeded.");

//            // Read file into memory
//            using (var memoryStream = new MemoryStream())
//            {
//                await file.CopyToAsync(memoryStream);
//                byte[] fileData = memoryStream.ToArray();

//                //// Save file data to SQL Server
//                //var fileEntity = new FileEntity
//                //{
//                //    FileName = file.FileName,
//                //    ContentType = file.ContentType,
//                //    FileData = fileData
//                //};
//                //_dbContext.Files.Add(fileEntity);
//                //await _dbContext.SaveChangesAsync();

//                //return Ok(new { fileId = fileEntity.Id });
//                return null;
//            }
//        }
//    }
//}