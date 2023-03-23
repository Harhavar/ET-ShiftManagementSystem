using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using ShiftMgtDbContext.Entities;
using System.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.TaskModel;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Data;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableCors("CorePolicy")]
    public class TaskController : Controller
    {
        private readonly ITaskServices taskServices;
        private readonly IMapper mapper;
        private readonly ITasksServices _tasksServices;
        private readonly ShiftManagementDbContext _dbContext;
        private readonly TaskCommentServices _commentServices;

        public TaskController(ITaskServices taskServices, IMapper mapper , ITasksServices tasksServices , ShiftManagementDbContext dbContext , TaskCommentServices commentServices)
        {
            this.taskServices = taskServices;
            this.mapper = mapper;
            _tasksServices = tasksServices;
             _dbContext = dbContext;
            _commentServices = commentServices;
        }

        //[HttpGet]
        //// [Authorize(Roles = "SuperAdmin,Admin,User")]
        //public async Task<IActionResult> GetTask()
        //{
        //    var task = await taskServices.GetTasks();

        //    if (task == null)
        //    {
        //        return NotFound();
        //    }

        //    var TaskDTO = mapper.Map<List<TaskDTO>>(task);

        //    return Ok(TaskDTO);
        //}

        //[HttpPost]
        ////[Authorize(Roles = "SuperAdmin,Admin")]
        //public IActionResult AddTask(TaskDetail taskDetail)
        //{
        //    try
        //    {

        //        var taskDtl = new TaskDetail()
        //        {
        //            TaskComment = taskDetail.TaskComment,
        //            CreatedBy = taskDetail.CreatedBy,
        //            CreatedDate = DateTime.Now,
        //            ModifiedBy = taskDetail.ModifiedBy,
        //            ModifiedDate = taskDetail.ModifiedDate,
        //            isActive = taskDetail.isActive,
        //        };
        //        taskServices.AddTask(taskDtl);
        //        return Ok(taskDtl);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete]
        ////[Authorize(Roles = "Admin")]
        ////[Authorize(Roles = "SuperAdmin,Admin")]
        //public async Task<IActionResult> DeleteTask(int id)
        //{
        //    try
        //    {


        //        var delete = await taskServices.DeleteTask(id);

        //        if (delete == null)
        //        {
        //            return NotFound();

        //        }

        //        var DeleteDto = new Models.TaskModel.TaskDTO()
        //        {
        //            Id = delete.Id,
        //            TaskComment = delete.TaskComment,
        //            CreatedBy = delete.CreatedBy,
        //            ModifiedBy = delete.ModifiedBy,
        //            ModifiedDate = delete.ModifiedDate,
        //            CreatedDate = delete.CreatedDate,
        //            isActive = delete.isActive,

        //        };
        //        return Ok(DeleteDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// Get All Task In Organization
        /// </summary>
        /// <returns></returns>
        [HttpGet("Task")]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            try
            {
                var responce = await _tasksServices.GetAllTasks();
                if (responce == null)
                {
                    return NotFound();
                }
                return Ok(responce);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Add task by user or admin 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TaskDetails"></param>
        /// <returns></returns>
        [HttpPost("AddTask")]
        public async Task<ActionResult> PostTask(Guid UserId, [FromForm] TaskUploadModel TaskDetails)
        {
            if (TaskDetails == null)
            {
                return BadRequest();
            }

            try
            {
                await _tasksServices.PostTaskAsync(UserId, TaskDetails.Text, TaskDetails.FileDetails, TaskDetails.FileType , TaskDetails.DueDate , TaskDetails.Actions , TaskDetails.TaskGivenTo);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add Comment by User Or admin on a perticular Task
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="updateTask"></param>
        /// <returns></returns>
        [HttpPut("Update-Task")]
        public async Task<ActionResult> AddCommentsUpdateTask(Guid TaskID ,[FromForm]UpdateTask updateTask)
        {
            try
            {
                var Task = _dbContext.Tasks.FirstOrDefault(x => x.Id == TaskID);
                if (Task == null)
                {
                    return BadRequest();
                }
                await _tasksServices.UpdateTask(TaskID, updateTask);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Add task by admin and User
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="taskVm"></param>
        /// <returns></returns>
        [HttpPost("TaskComment")]
        public IActionResult AddTask(Guid UserId , [FromForm] TaskCommentVM taskVm)
        {
            try
            {
                _commentServices.addComment(UserId, taskVm);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


        /// <summary>
        /// Get Comments in perticualr Task By Task Id 
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        [HttpGet("TaskComments")]

        public IActionResult GetTask(Guid TaskID)
        {
            try
            {
                var Task = _commentServices.GetAllComment(TaskID);
                if (Task == null)
                {
                    return NotFound();
                }


                return Ok(Task);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        [HttpGet]
        [Route("[Action]")]
        public IActionResult GetuserTaskToDo(Guid UserId)
        {
            var responce = _tasksServices.GetuserTaskToDo(UserId);

            if(responce == null)
            {
                return NotFound();
            }
            return Ok(responce);
        }
        [HttpGet]
        [Route("[Action]")]
        public IActionResult GetuserTaskInProgress(Guid UserId)
        {
            var responce = _tasksServices.GetuserTaskInProgress(UserId);
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(responce);
        }
    }
}
