using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.TaskModel;
using ET_ShiftManagementSystem.Services;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    public class TaskController : Controller 
    {
        private readonly ITasksServices _tasksServices;
        private readonly ITaskCommentServices _commentServices;

        public TaskController(ITasksServices tasksServices , ITaskCommentServices commentServices)
        {
            _tasksServices = tasksServices;
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
        /// Get Recent Activity related to task 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RecentActivity()
        {
            return Ok();
        }
        /// <summary>
        /// Get All Task In Organization/ project
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ProjectId}")]
        [Authorize(Roles ="User,Admin,SystemAdmin")]
        public IActionResult GetTasks(Guid ProjectId)
        {
            if(Guid.Empty == ProjectId)
            {
                return BadRequest();
            }
            try
            {
                var responce =  _tasksServices.GetAllTasks(ProjectId);
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
        [HttpGet("Task")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]
        public IActionResult GetTasks()
        {
            try
            {
                var responce = _tasksServices.GetAllTasks();
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
        /// Get All Task In Organization
        /// </summary>
        /// <returns></returns>
        [HttpGet("Task/{TenantId}")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public IActionResult GetTasksByOrg(Guid TenantId)
        {
            if (Guid.Empty == TenantId )
            {
                return BadRequest();
            }

            try
            {
                var responce = _tasksServices.GetAllTasksOfOneOrg(TenantId);
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
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin,User")]

        public async Task<ActionResult> PostTask(Guid UserId, [FromForm] TaskUploadModel TaskDetails)
        {
            if (TaskDetails == null || Guid.Empty == UserId)
            {
                return BadRequest();
            }

            try
            {
                await _tasksServices.PostTaskAsync(UserId, TaskDetails.Text, TaskDetails.FileDetails, TaskDetails.FileType , TaskDetails.DueDate , TaskDetails.Actions , TaskDetails.TaskGivenTo ,TaskDetails.ProjectId);
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
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin,User")]

        public async Task<ActionResult> AddCommentsUpdateTask(Guid TaskID ,[FromForm]UpdateTask updateTask)
        {

            if (Guid.Empty == TaskID || updateTask == null)
            {
                return BadRequest();
            }
            try
            {
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
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin ,User")]

        public IActionResult AddTask(Guid UserId , [FromForm] TaskCommentVM taskVm)
        {
            if (Guid.Empty == UserId || taskVm == null)
            {
                return BadRequest();
            }
            try
            {
                var responce = _commentServices.addComment(UserId, taskVm);
                if (responce == null)
                {
                    return NotFound("User not found");
                }
                return Ok(responce);
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
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin,User")]

        public IActionResult GetTask(Guid TaskID)
        {
            if (Guid.Empty == TaskID)
            {
                return BadRequest();
            }
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
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin,User")]

        public IActionResult GetuserTaskToDo(Guid UserId)
        {
            if(UserId == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                var responce = _tasksServices.GetuserTaskToDo(UserId);

                if (responce == 0)
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
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin,User")]

        public IActionResult GetuserTaskInProgress(Guid UserId)
        {
            if (UserId == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                var responce = _tasksServices.GetuserTaskInProgress(UserId);
                if (responce == 0)
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
    }
}
