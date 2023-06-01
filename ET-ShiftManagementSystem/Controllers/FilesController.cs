using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _uploadService;

        public FilesController(IFileService uploadService)
        {
            _uploadService = uploadService;
        }


        /// <summary>
        /// View Document Upoladed Related to the project 
        /// </summary>
        /// <param name="TenentId"></param>
        /// <returns></returns>
        [HttpGet("View")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult<IEnumerable<FileDetails>>> Get(Guid TenentId)
        {
            try
            {
                var document = await _uploadService.GetallDocs(TenentId);

                if (document != null)
                {

                    var Doc = new List<ViewDocument>();

                    document.ToList().ForEach(document =>
                    {
                        var doc = new ViewDocument()
                        {
                            FileName = document.FileName,
                            UpdateDate = document.UpdateDate,
                        };
                        Doc.Add(doc);

                    });

                    return Ok(Doc);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        /// <summary>
        /// View Pericular document Uploaded by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("View-Single")]
        //[Route("{id:Guid}")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<IActionResult> GetOrganizationById(Guid id)
        {
            try
            {
                var Organization = await _uploadService.GetSingleDocs(id);

                if (Organization == null)
                {
                    return NotFound();
                }

                //var doc = new ViewDocument()
                //{
                //    FileName = Organization.FileName,
                //    UpdateDate = Organization.UpdateDate,
                //};

                return Ok(Organization);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Single File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("PostSingleFile")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult> PostSingleFile(Guid TenentId, [FromForm] FileUploadModel fileDetails)
        {
            try
            {
                if (fileDetails == null)
                {
                    return BadRequest();
                }
                if (fileDetails == null || TenentId == Guid.Empty)
                {
                    return BadRequest();
                }


                 await _uploadService.PostFileAsync(TenentId, fileDetails.FileDetails, fileDetails.FileType , fileDetails.ProjectId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Multiple File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("PostMultipleFile")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult> PostMultipleFile([FromRoute] Guid id, [FromForm] List<FileUploadModel> fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                await _uploadService.PostMultiFileAsync(id, fileDetails);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpGet("DownloadFile")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult> DownloadFile(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                await _uploadService.DownloadFileById(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Delete Unwanted File in a project 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDoc")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult> DeleteDocs(Guid id)
        {
            try
            {
                var Delete = await _uploadService.DeleteDocuent(id);
                if (Delete == null)
                {
                    return NotFound();
                }

                return Ok(Delete.FileName);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
    }
}
