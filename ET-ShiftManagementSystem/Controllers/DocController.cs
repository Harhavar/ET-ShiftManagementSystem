using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftManagementServises.Servises;
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

        public DocController(IDocumentServices documentServices , IMapper mapper)
        {
            this.documentServices = documentServices;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocs()
        {
            var Doc = await documentServices.GetallDocument();

            var DocDTO = mapper.Map<List<Models.DocDTO>>(Doc);

            return Ok(DocDTO);
        }

        [HttpPost]

        public IActionResult AddDocs(Doc doc)
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

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoc(int id)
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
    }
}
