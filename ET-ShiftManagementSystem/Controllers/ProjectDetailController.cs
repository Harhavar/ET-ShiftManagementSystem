using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using System.Security.Cryptography.X509Certificates;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProjectDetailController : Controller
    {
        private readonly IProjectDatailServises projectDatailServises;
        private readonly IShiftServices _shiftServices;
        private readonly IMapper mapper;

        public ProjectDetailController(IProjectDatailServises projectDatailServises ,IShiftServices shiftServices, IMapper mapper)
        {
            this.projectDatailServises = projectDatailServises;
            this.mapper = mapper;
            this._shiftServices= shiftServices;
        }

        [HttpGet]
        public async  Task<IActionResult> GetProjectDetails()
        {
            try
            {

           
            var project = await  projectDatailServises.GetProjecDetails();

                if(project == null)
                {
                    return NotFound();
                }

            var ProjectDetailDTO = new List<Models.ProjectDetailsDTO>();
            project.ToList().ForEach(project =>
            {
                var ProjectDTO = new Models.ProjectDetailsDTO()
                {
                    ProjectDetailsID = project.ProjectDetailsID,
                    ShiftID = project.ShiftID,
                    UserID = project.UserID,
                    CreatedBy = project.CreatedBy,
                    CreatedDate = project.CreatedDate,
                    ModifiedBy = project.ModifiedBy,
                    ModifiedDate = project.ModifiedDate,
                    IsActive = project.IsActive
                };
                if(ProjectDTO.ShiftID.HasValue)
                {
                    var shiftDetails = _shiftServices.GetAllShiftAsync();
                    //if (shiftDetails != null) ;
                    //{
                    //ProjectDTO.ShiftID = shiftDetails.Id;
                    //ProjectDTO.ShiftName = shiftDetails.ShiftName;

                   // ProjectDTO = shiftDetails;
                }
                ProjectDetailDTO.Add(ProjectDTO);

            });


            //var ProjectDetailDTO = mapper.Map<List<Models.ProjectDetailsDTO>>(project);

            return Ok(ProjectDetailDTO);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
