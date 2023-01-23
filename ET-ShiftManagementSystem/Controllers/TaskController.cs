using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using ShiftMgtDbContext.Entities;
using System.Data;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskServices taskServices;
        private readonly IMapper mapper;

        public TaskController(ITaskServices taskServices, IMapper mapper)
        {
            this.taskServices = taskServices;
            this.mapper = mapper;
        }

        [HttpGet]
       // [Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> GetTask()
        {
            var task = await taskServices.GetTasks();

            if (task == null)
            {
                return NotFound();
            }

            var TaskDTO = mapper.Map<List<Models.TaskDTO>>(task); 

            return Ok(TaskDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult AddTask(TaskDetail taskDetail)
        {
            try
            {

            
            var taskDtl = new TaskDetail()
            {
                TaskComment = taskDetail.TaskComment,
                CreatedBy = taskDetail.CreatedBy,
                CreatedDate = taskDetail.CreatedDate,
                ModifiedBy = taskDetail.ModifiedBy,
                ModifiedDate = taskDetail.ModifiedDate,
                isActive = taskDetail.isActive,


            };
            taskServices.AddTask(taskDtl);
            return Ok(taskDtl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        public async  Task<IActionResult> DeleteTask(int id)
        {
            try
            {


                var delete = await taskServices.DeleteTask(id);

                if (delete == null)
                {
                    return NotFound();

                }

                var DeleteDto = new Models.TaskDTO()
                {
                    Id = delete.Id,
                    TaskComment = delete.TaskComment,
                    CreatedBy = delete.CreatedBy,
                    ModifiedBy = delete.ModifiedBy,
                    ModifiedDate = delete.ModifiedDate,
                    CreatedDate = delete.CreatedDate,
                    isActive = delete.isActive,

                };
                return Ok(DeleteDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
