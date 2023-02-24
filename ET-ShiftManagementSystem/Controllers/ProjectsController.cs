using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ET_ShiftManagementSystem.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectServices projectServices;

        public ProjectsController(IProjectServices projectServices)
        {
            this.projectServices = projectServices;
        }

        [HttpGet]
        public IActionResult GetProject()
        {
            var proj = projectServices.GetProject();

            if(proj== null)
            {
                return NotFound();
            }

            return Ok(proj);
        }

        //[HttpPost]

        //public IActionResult PostProject(Project project)
        //{

        //}
    }
}
