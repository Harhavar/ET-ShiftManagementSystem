using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IDocumentServices
    {
        Task<IEnumerable<Doc>> GetallDocument();

        long AddDocs(Doc doc);

        Task<Doc> DeleteDoc(int id);

    }
    public class DocumentServices : IDocumentServices
    {
        private readonly ShiftManagementDbContext docServises;


        public DocumentServices(ShiftManagementDbContext DocServises)
        {
            docServises = DocServises;
        }

        public long AddDocs(Doc doc)
        {
            docServises.Docs.Add(doc);
            docServises.SaveChanges();
            return doc.Id;
        }

        public async Task<Doc> DeleteDoc(int id)
        {
            var Doc = await docServises.Docs.FirstOrDefaultAsync(x => x.Id == id);

            if (Doc == null)
            {
                return null;
            }

            docServises.Docs.Remove(Doc);
            await docServises.SaveChangesAsync();

            return Doc;
        }

        public async Task<IEnumerable<Doc>> GetallDocument()
        {
            return await docServises.Docs.ToListAsync();
        }
    }
}
