using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

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
        public async Task<ActionResult<IEnumerable<FileDetails>>> Get(Guid TenentId)
        {
            var document = await _uploadService.GetallDocs(TenentId);

            if (document == null)
            {
                return NotFound();
            }
            try
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
            catch (Exception ex)
            {
                throw;
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
        public async Task<ActionResult> PostSingleFile(Guid TenentId, [FromForm] FileUploadModel fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                await _uploadService.PostFileAsync(TenentId, fileDetails.FileDetails, fileDetails.FileType);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Multiple File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("PostMultipleFile")]
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
                throw;
            }

        }

        /// <summary>
        /// Delete Unwanted File in a project 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDoc")]

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
