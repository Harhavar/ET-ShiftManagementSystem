using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Models.NotesModel;
using ET_ShiftManagementSystem.Models.TaskModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ET_ShiftManagementSystem.Services
{
    public interface INotesServices
    {
        public Task<IEnumerable<Note>> GetAllNotes();
        public Task<IEnumerable<Note>> GetAllNotes(Guid ProjectId);
        Task PostNotesAsync(Guid userId, string text, IFormFile? fileDetails);
    }
    public class NotesServices : INotesServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public NotesServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            return await _dbContext.Notes.ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetAllNotes(Guid ProjectId)
        {
            var responce = await _dbContext.Notes.Where(x => x.ProjectId == ProjectId).ToListAsync();

            if (responce == null)
            {
                return null;
            }
            //var Notes = new List<NotesViewRequest>();
            //responce.ToList().ForEach(x =>
            //{
            //    var note = new NotesViewRequest()
            //    {
            //        FileName = x.FileName,
            //        Text = x.Text,
            //        FileData = x.FileData,
            //        CreatedBy = x.CreatedBy,
            //        CreatedDate = x.CreatedDate,
            //    };
            //    Notes.Add(note);

            //});

            return responce;
        }
        public async Task PostNotesAsync(Guid userId, string text, IFormFile? fileDetails)
        {
            try
            {
                var username = _dbContext.users.Where(x => x.id == userId).Select(x => x.username).FirstOrDefault();
                var TenantId = _dbContext.users.Where(x => x.id == userId).Select(x => x.TenentID).FirstOrDefault();

                var Note = new Note()
                {
                    Id = Guid.NewGuid(),
                    TenantId = TenantId,
                    Text = text,
                    ProjectId = new Guid("3CB4D802-2D3F-43B7-AF53-BE05E8700DC1"),

                    FileName = fileDetails.FileName,

                    CreatedDate = DateTime.Now,

                    CreatedBy = username

                };

                using (var stream = new MemoryStream())
                {
                    fileDetails.CopyTo(stream);
                    Note.FileData = stream.ToArray();
                }

                var result = _dbContext.Notes.Add(Note);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
