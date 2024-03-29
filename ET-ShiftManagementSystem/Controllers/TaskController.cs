﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftManagementServises.Servises;
using ShiftMgtDbContext.Entities;
using System.Data;

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

        public async Task<IActionResult> GetTask()
        {
            var task = await taskServices.GetTasks();

            var TaskDTO = mapper.Map<List<Models.TaskDTO>>(task); 

            return Ok(TaskDTO);
        }

        [HttpPost]
        public IActionResult AddTask(TaskDetail taskDetail)
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

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> DeleteTask(int id)
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
    }
}
