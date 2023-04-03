using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ET_ShiftManagementSystem.Services
{
    public interface IFileService
    {
        public List<FileDetails> GetFileDetails();
        public Task<IEnumerable<FileDetails>> GetallDocs(Guid TenentId);
        public Task<FileDetails> GetSingleDocs(Guid id);
        public Task PostFileAsync( Guid TenentId, IFormFile fileData, FileType fileType);

        public Task PostMultiFileAsync(Guid TenentId, List<FileUploadModel> fileData);

        public Task DownloadFileById(Guid fileName);
        public Task<FileDetails> DeleteDocuent(Guid id);
    }
    public class FileService : IFileService
    {
        private readonly ShiftManagementDbContext dbContextClass;

        public FileService(ShiftManagementDbContext dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task PostFileAsync(Guid TenentId , IFormFile fileData, FileType fileType)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                    ID = Guid.NewGuid(),
                    FileName = fileData.FileName,
                    FileType = fileType,
                    UpdateDate = DateTime.Now,
                    TenentID= TenentId
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = dbContextClass.FileDetails.Add(fileDetails);
                await dbContextClass.SaveChangesAsync();
               // return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(Guid TenentId, List<FileUploadModel> fileData)
        {
            try
            {
                foreach (FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        ID = Guid.NewGuid(),
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                        UpdateDate=DateTime.Now,
                        TenentID=TenentId,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = dbContextClass.FileDetails.Add(fileDetails);
                }
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DownloadFileById(Guid Id)
        {
            try
            {
                var file = dbContextClass.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

                var content = new System.IO.MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.Result.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public List<FileDetails> GetFileDetails()
        {
            var doc =  dbContextClass.FileDetails.ToList();

            return doc;
        }

        public async Task<IEnumerable<FileDetails>> GetallDocs(Guid TenentId)
        {
            return await dbContextClass.FileDetails.Where(x => x.TenentID == TenentId).ToListAsync();
        }
        public async Task<FileDetails> DeleteDocuent(Guid id)
        {
            var delete = await dbContextClass.FileDetails.FirstOrDefaultAsync(x => x.ID == id);

            if (delete == null)
            {
                return null;
            }

            dbContextClass.FileDetails.Remove(delete);
            dbContextClass.SaveChanges();
            return delete;
        }

        public async Task<FileDetails> GetSingleDocs(Guid id)
        {
            var doc = await dbContextClass.FileDetails.FirstOrDefaultAsync(x => x.ID == id);

            if (doc == null)
            {
                return null;
            }
            return doc;
        }
    }
}
