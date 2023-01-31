using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftMgtDbContext.Entities;
using System.Data;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class DocController : Controller
    {
        private readonly IDocumentServices documentServices;
        private readonly IMapper mapper;

        public DocController(IDocumentServices documentServices, IMapper mapper)
        {
            this.documentServices = documentServices;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> GetAllDocs()
        {
            var Doc = await documentServices.GetallDocument();

            if (Doc == null)
            {
                return NotFound();
            }

            var DocDTO = mapper.Map<List<Models.DocDTO>>(Doc);

            return Ok(DocDTO);
        }

        //[HttpGet]
        
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public IActionResult AddDocs(Doc doc)
        {
            try
            {
                var Document = new Doc
                {
                    // Id= doc.Id,
                    Docs = doc.Docs,
                    CreatedBy = doc.CreatedBy,
                    CreatedDate = doc.CreatedDate,
                    modifiedBy = doc.modifiedBy,
                    ModifiedDate = doc.ModifiedDate,
                    isActive = doc.isActive,

                };

                documentServices.AddDocs(Document);
                return Ok(Document);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> DeleteDoc(int id)
        {
            try
            {

                var delete = await documentServices.DeleteDoc(id);

                if (delete == null)
                {
                    return NotFound();
                }

                var DeleteDTO = new Models.DocDTO()
                {
                    Id = id,
                    Docs = delete.Docs,
                    CreatedBy = delete.CreatedBy,
                    modifiedBy = delete.modifiedBy,
                    ModifiedDate = delete.ModifiedDate,
                    isActive = delete.isActive,
                    CreatedDate = delete.CreatedDate,

                };
                return Ok(DeleteDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
