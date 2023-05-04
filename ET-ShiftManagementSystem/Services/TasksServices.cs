using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Models.TaskModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;
using ShiftMgtDbContext.Entities;

namespace ET_ShiftManagementSystem.Services
{
    public interface ITasksServices
    {
        public List<Tasks> GetAllTasks(Guid ProjectId);
        public List<Tasks> GetAllTasks();
        public List<Tasks> GetAllTasksOfOneOrg(Guid TenantId);
        public int GetuserTaskToDo(Guid userId);
        public int GetuserTaskInProgress(Guid userId);
        public Task PostTaskAsync(Guid UserId, string text, IFormFile? fileDetails, FileType? fileType, DateTime dueDate, Actions Actions, Guid? TaskGivento , Guid ProjectId);
        public Task UpdateTask(Guid TaskID, UpdateTask updateTask);
    }

    public class TasksServices : ITasksServices
    {
        private readonly ShiftManagementDbContext _dbContext;

        public TasksServices(ShiftManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<Tasks> GetAllTasks(Guid ProjectId)
        {
            return _dbContext.Tasks.Where(x => x.ProjectId == ProjectId).ToList();
        }
        public List<Tasks> GetAllTasks()
        {
            return _dbContext.Tasks.ToList();
        }
        public int GetuserTaskToDo(Guid userId)
        {
            var responce = _dbContext.Tasks.Where(x => x.TaskGivenToID == userId).Where(x => x.Actions == Actions.ToDo).ToList();
            if (responce.Count == 0)
            {
                return 0;
            }
            return responce.Count;
        }
        public int GetuserTaskInProgress(Guid userId)
        {
            var responce = _dbContext.Tasks.Where(x => x.TaskGivenToID == userId).Where(x => x.Actions == Actions.inProgress).ToList();
            if (responce.Count == 0)
            {
                return 0;
            }
            return responce.Count;
        }

        public async Task PostTaskAsync(Guid UserId, string text, IFormFile? fileDetails, FileType? fileType, DateTime dueDate, Actions Actions, Guid? TaskGivento , Guid ProjectId)
        {

            try
            {
                var username = _dbContext.users.Where(x => x.id == UserId).Select(x => x.username).FirstOrDefault();
                var TenantId = _dbContext.users.Where(x => x.id == UserId).Select(x => x.TenentID).FirstOrDefault();
                var assingnedUser = _dbContext.users.Where(x => x.id == TaskGivento).Select(x => x.username).FirstOrDefault();
                var projectName = _dbContext.Projects.Where(x => x.ProjectId == ProjectId).Select(x => x.Name).FirstOrDefault();
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
                var activity = new Activity
                {
                    ActivityId = Guid.NewGuid(),
                    ProjectName = projectName,
                    Action = "Task Added",
                    Message = $"Task given By {" "} Task Given To {TaskGivento} ",
                    UserName = "",
                    TenetId = TenantId,
                    Timestamp = DateTime.Now,
                };
                _dbContext.Activities.Add(activity);
                _dbContext.SaveChanges();
                var result = _dbContext.Tasks.Add(Task);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task UpdateTask(Guid TaskID, UpdateTask updateTask)
        {
            var existingTask = _dbContext.Tasks.FirstOrDefault(x => x.Id == TaskID);

            if (existingTask != null)
            {
                existingTask.Actions = updateTask.Actions;
                existingTask.ModifiedDate = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }

        }

        public List<Tasks> GetAllTasksOfOneOrg(Guid TenantId)
        {
            return _dbContext.Tasks.Where(x => x.TenantId == TenantId).ToList();
        }
    }
}
