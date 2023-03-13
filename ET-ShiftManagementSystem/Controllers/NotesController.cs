using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.NotesModel;
using ET_ShiftManagementSystem.Models.TaskModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class NotesController : ControllerBase
    {
        private readonly INotesServices _notesServices;

        public NotesController(INotesServices notesServices)
        {
            _notesServices = notesServices;
        }

        /// <summary>
        /// only admin can see all the notes from different projects 
        /// </summary>
        /// <returns></returns>
        [HttpGet("NotesAdminView")]
        public async Task<ActionResult<IEnumerable<Note>>> Get()
        {
            try
            {
                var responce = await _notesServices.GetAllNotes();

                if (responce == null)
                {
                    return NotFound();
                }
                var Notes = new List<NotesViewRequest>();
                responce.ToList().ForEach(x =>
                {
                    var note = new NotesViewRequest
                    {
                        FileName = x.FileName,
                        Text = x.Text,
                        FileData = x.FileData,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                    };
                    Notes.Add(note);

                });

                return Ok(Notes);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
        }

        /// <summary>
        /// view notes by user and admin , only showing notes on perticular project
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpGet("NotesView")]
        public async Task<ActionResult<IEnumerable<Note>>> Get(Guid ProjectId)
        {
            try
            {
                var responce = await _notesServices.GetAllNotes(ProjectId);

                if (responce == null)
                {
                    return NotFound();
                }
                var Notes = new List<NotesViewRequest>();
                //responce.ToList().ForEach(x =>
                //{
                var note = new NotesViewRequest()
                {
                    FileName = responce.Where(x => x.ProjectId == ProjectId).ToList().FirstOrDefault().FileName,
                    Text = responce.Where(x => x.ProjectId == ProjectId).ToList().FirstOrDefault().Text,
                    FileData = responce.Where(x => x.ProjectId == ProjectId).ToList().FirstOrDefault().FileData,
                    CreatedBy = responce.Where(x => x.ProjectId == ProjectId).ToList().FirstOrDefault().CreatedBy,
                    CreatedDate = responce.Where(x => x.ProjectId == ProjectId).ToList().FirstOrDefault().CreatedDate,
                };
                Notes.Add(note);

                //});

                return Ok(Notes);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
           
        }

        /// <summary>
        /// add notes by user or admin 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="addNotes"></param>
        /// <returns></returns>
        [HttpPost("AddNotes")]
        public async Task<ActionResult> PostNotes(Guid UserId, [FromForm] AddNotesVM addNotes)
        {
            if (addNotes == null)
            {
                return BadRequest();
            }

            try
            {
                await _notesServices.PostNotesAsync(UserId, addNotes.Text, addNotes.FileDetails);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
