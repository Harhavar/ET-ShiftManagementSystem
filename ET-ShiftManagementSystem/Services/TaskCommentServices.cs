using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.TaskModel;

namespace ET_ShiftManagementSystem.Services
{
    public class TaskCommentServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public TaskCommentServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void addComment( Guid UserId, TaskCommentVM taskComment)
        {
            var user = _dbContext.users.Where(x => x.id == UserId).Select(x => x.username).FirstOrDefault();
            var comment = new TaskComment
            {
                Id = Guid.NewGuid(),
                TaskID = taskComment.TaskID,
                Comments = taskComment.Comments,
                CreatedBy = user,
                CreatedDate = DateTime.Now,

            };
            _dbContext.TaskComments.Add(comment);
            _dbContext.SaveChanges();

        }
        public CommentVM GetAllComment(Guid TaskID)
        {
            var responce = _dbContext.TaskComments.Where(x => x.TaskID == TaskID).Select(x => new CommentVM()
            {
                CreatedBy = x.CreatedBy, CreatedDate = DateTime.Now, Comments = x.Comments
            }).FirstOrDefault();
           

            if( responce == null)
            {
                return null;
            }
            return responce;
           
        }
    }
}
