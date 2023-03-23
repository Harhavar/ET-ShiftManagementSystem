using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Models.TaskModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Services
{
    public interface ITasksServices
    {
        public Task<IEnumerable<Tasks>> GetAllTasks();
        public int GetuserTaskToDo(Guid userId);
        public int GetuserTaskInProgress(Guid userId);
        public Task PostTaskAsync(Guid UserId, string text, IFormFile? fileDetails, FileType? fileType, DateTime dueDate, Actions Actions, Guid? TaskGivento);
        Task UpdateTask(Guid TaskID , UpdateTask updateTask);
    }

    public class TasksServices : ITasksServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public TasksServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Tasks>> GetAllTasks()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public int GetuserTaskToDo(Guid userId)
        {
            var responce = _dbContext.Tasks.Where(x => x.TaskGivenToID== userId).Where( x => x.Actions == Actions.ToDo).ToList();
           
            return responce.Count;
        }
        public int GetuserTaskInProgress(Guid userId)
        {
            var responce = _dbContext.Tasks.Where(x => x.TaskGivenToID == userId).Where(x => x.Actions == Actions.inProgress).ToList();

            return responce.Count;
        }

        public async Task PostTaskAsync(Guid UserId, string text, IFormFile? fileDetails, FileType? fileType, DateTime dueDate, Actions Actions, Guid? TaskGivento)
        {

            try
            {
                var username = _dbContext.users.Where(x => x.id == UserId).Select(x => x.username).FirstOrDefault();
                var TenantId = _dbContext.users.Where(x => x.id == UserId).Select(x => x.TenentID).FirstOrDefault();
                var assingnedUser = _dbContext.users.Where(x => x.id == TaskGivento).Select(x => x.username).FirstOrDefault();
                var Task = new Tasks()
                {
                    Id = Guid.NewGuid(),
                    TenantId = TenantId,
                    Text = $"Task given by {username} to {TaskGivento} Description :{text}",
                    Actions = Actions,
                    DueDate = dueDate,
                    // if(dueDate => DateTime.Now) 
                    //{ Task.DueComment = "overDue" }
                    FileName = fileDetails.FileName,
                    FileType = fileType,
                    CreatedDate = DateTime.Now,
                    TaskGivenTo = assingnedUser,
                    TaskGivenToID = TaskGivento,
                    CreatedBy = username

                };

                using (var stream = new MemoryStream())
                {
                    fileDetails.CopyTo(stream);
                    Task.FileData = stream.ToArray();
                }

                var result = _dbContext.Tasks.Add(Task);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task UpdateTask(Guid TaskID,UpdateTask updateTask)
        {
           var existingTask = _dbContext.Tasks.FirstOrDefault( x => x.Id == TaskID );

            if(existingTask != null )
            {
                //existingTask.Id = TaskID;
                existingTask.Actions = updateTask.Actions;
                existingTask.ModifiedDate = DateTime.Now;
                //_dbContext.Tasks.Add(existingTask);
                await _dbContext.SaveChangesAsync();
            }
            
        }
    }
}
