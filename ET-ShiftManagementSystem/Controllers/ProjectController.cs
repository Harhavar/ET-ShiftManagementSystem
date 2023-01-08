using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servises.ProjectServises;
using ShiftMgtDbContext.Entities;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Models;
using ET_ShiftManagementSystem.Servises;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServises _projectServices;
        private readonly IProjectDatailServises _projectDatailServises;
        //private readonly IUserServices _userServices;
        private readonly IShiftServices _shiftServices;
        private readonly ICommentServices _commentServices;
        private readonly IMapper mapper;

        public ProjectController(IProjectServises projectServices, IProjectDatailServises projectDatailServises, /*IUserServices userServices*/ IShiftServices shiftServices, ICommentServices commentServices , IMapper mapper)
        {
            _projectServices = projectServices;
            _projectDatailServises = projectDatailServises;
            //_userServices = userServices;
            _shiftServices = shiftServices;
            _commentServices = commentServices;
            this.mapper = mapper;
        }
        //[Route("Details/{projectId}")]
        //[HttpGet]
        //[Authorize(Roles = "Admin")]
       // public async Task<IActionResult> GetProjectDetails(int projectId)
        //{
            //var projectDetailsData = new ProjectDetailsDTO();
            //var projectdetail = await _projectServices.GetProject(projectId);
            //projectDetailsData.ProjectName= projectdetail.ProjectName;
            //projectDetailsData.ClientName= projectdetail.ClientName;
            //projectDetailsData.Description = projectdetail.Description;
            //projectDetailsData.ProjectID = projectId;
            ////var projectData = _projectDatailServises.GetProjecDetails(projectId);


            ////var CommentDetail = await _commentServices.GetComment(projectId);

            //var ShiftDetail = _shiftServices.GetShiftDetails(projectId);
            
            //foreach ( var item in projectData)
            //{
                //var userDetail = _userServices.GetUserDetails(item.UserID);
                //var projectUser = new ProjectUser()
                //{
                //    UserId = item.UserID,
                //    UserName = userDetail.UserName,
                //    FullName = userDetail.FullName,
                //    Email = userDetail.Email,

                //};
                //if (item.ShiftID.HasValue)
                //{
                //    var shiftDetails = _shiftServices.GetShiftDetails(item.ShiftID.Value);
                //    if (shiftDetails != null) 
                //    {
                //        projectUser.ShiftID = item.ShiftID.Value;
                //        projectUser.ShiftName = shiftDetails.ShiftName;

                //    }
                //};

                //projectDetailsData.ProjectUsers.Add(projectUser);

                
            //}
            ////foreach (var value in CommentDetail)
            //{
            //    var Comment = _commentServices.GetComment(value.CommentID);
            //    var userComment = new CommentDetailes()
            //    {
            //        CommentText = value.CommentText,
            //        CreatedDate= value.CreatedDate,
            //        EmployeeName = projectDetailsData.ProjectUsers.FirstOrDefault(a => a.UserId==value.UserID).UserName,
            //        ShiftID= value.ShiftID,
            //        UserID  = value.UserID,
            //        Shift = projectDetailsData.ProjectUsers.FirstOrDefault(a => a.ShiftID == value.ShiftID).ShiftName
            //    };
            //    projectDetailsData.CommentDetiles.Add(userComment);
            //}
                

            //return Ok(projectDetailsData);
        //}

        [HttpGet]
        [Route("/allProject")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects()
        {
            var project = await _projectServices.GetAllAsync();

           var ProjectDTO = mapper.Map<List<Models.ProjectDto>>(project);

            return Ok(ProjectDTO);

        }

        [HttpGet]
        [Route("/singleProject")]
        [ActionName("GetProjectAsync")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectAsync(int id)
        {
            var project = await _projectServices.GetProjectAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var projectDTO = mapper.Map<Models.ProjectDto>(project);

            return Ok(project);
        }

        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        public  IActionResult AddProjectASync(Project projectDto)
        {
            var Proj = new Project()
            {
                ProjectName = projectDto.ProjectName,
                Description = projectDto.Description,
                ClientName = projectDto.ClientName,
                CreatedBy = projectDto.CreatedBy,
                CreatedDate = DateTime.Now,
                ModifieBy = projectDto.ModifieBy,
                ModifieDate = DateTime.Now,
                IsActive = projectDto.IsActive
            };

             _projectServices.AddProjectAsync(Proj); 

            ////Conevrt back to dto
            //var projectDTO = new Models.ProjectDto()
            //{
            //    ProjectId = Proj.ProjectId,
            //    ProjectName = Proj.ProjectName,
            //    Description = Proj.Description,
            //    ClientName = Proj.ClientName,
            //    CreatedBy = Proj.CreatedBy,
            //    ModifieBy= Proj.ModifieBy,
            //    IsActive    = Proj.IsActive,


            //}

            //return CreatedAtAction(nameof())
            return Ok(Proj);
        }
      

        [HttpPut]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateProject(int id , Project ProjectDto) 
        {
            var Project = new Project()
            {
                ProjectName = ProjectDto.ProjectName,
                Description = ProjectDto.Description,
                ClientName = ProjectDto.ClientName,
                CreatedBy = ProjectDto.CreatedBy,
                CreatedDate = DateTime.Now,
                ModifieBy = ProjectDto.ModifieBy,
                ModifieDate = DateTime.Now,
                IsActive = ProjectDto.IsActive
            };

            await _projectServices.UpdateAsync(id ,Project);

            return Ok(Project);
        }

        [HttpDelete]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var delete = await _projectServices.DeleteProjectAsync(id);

            if (delete == null)
            {
                NotFound();
            }

            //Convert response back to DTO

            var DeleteDTO = new Models.ProjectDto()
            {
                ProjectId = delete.ProjectId,
                CreatedBy = delete.CreatedBy,
                ClientName = delete.ClientName,
                ModifieBy = delete.ModifieBy,
                Description = delete.Description,
                IsActive = delete.IsActive,
                ProjectName = delete.ProjectName

            };

            return Ok(DeleteDTO);
        }

        
    }
}
