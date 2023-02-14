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
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Net.WebSockets;
using System.Formats.Asn1;
using System.Runtime.Intrinsics.X86;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.ProjectModel;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("CorePolicy")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServises _projectServices;
        private readonly IProjectDatailServises _projectDatailServises;
        //private readonly IUserServices _userServices;
        private readonly IShiftServices _shiftServices;
        private readonly ICommentServices _commentServices;
        private readonly IMapper mapper;
        private readonly ISREDetiles _sreDetails;
        private readonly IUserRepository userRepsository;
        private readonly IProjectUserRepository _projectUser;
        private readonly ITenateServices tenateServices;

        public ProjectController(IProjectServises projectServices, IProjectDatailServises projectDatailServises, /*IUserServices userServices*/ IShiftServices shiftServices, ICommentServices commentServices, IMapper mapper, ISREDetiles SreDetails, IUserRepository userRepository, IProjectUserRepository projectUser , ITenateServices tenateServices)
        {
            _projectServices = projectServices;
            _projectDatailServises = projectDatailServises;
            //_userServices = userServices;
            _shiftServices = shiftServices;
            _commentServices = commentServices;
            this.mapper = mapper;
            _sreDetails = SreDetails;
            this.userRepsository = userRepository;
            this._projectUser = projectUser;
            this.tenateServices = tenateServices;
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
        //[Route("allProject/")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> GetAllProjects()
        {
            var project = await _projectServices.GetAllAsync();

            if (project == null)
            {
                return NotFound();
            }

            var ProjectDTO = mapper.Map<List<ProjectDto>>(project);

            return Ok(ProjectDTO);

        }

        [HttpGet]
        [Route("{id}")]
        [ActionName("GetProjectAsync")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> GetProjectAsync(int id)
        {
            var project = await _projectServices.GetProjectAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var projectDTO = mapper.Map<ProjectDto>(project);

            return Ok(project);
        }

        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult AddProjectASync(Project projectDto)
        {
            try
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

                return Ok(Proj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateProject(int id, Project ProjectDto)
        {
            try
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

                await _projectServices.UpdateAsync(id, Project);

                return Ok(Project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            var DeleteDTO = new Models.ProjectModel.ProjectDto()
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

        [HttpPost]
        [Route("AddUserToProject")]
        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult AddUserToProject(Guid userId, int projectId)
        {
            try
            {
                var user = userRepsository.Get(userId);
                var project = _projectUser.GetProject(projectId);
                if (user == null || project == null)
                {
                    //if((project). >= 1)
                    //{
                    //    return BadRequest("already in project");
                    //}
                    return null;
                }
                var projectUser = new Entities.ProjectUser()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ProjectId = projectId,
                };
                //tenateServices.increase()

                var abc = _projectUser.Update(projectUser);

                return Ok(abc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RemoveUserFromProject")]
        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult RemoveUserFromProject(Guid userId, int projectId)
        {
            try
            {
                var user = _projectUser.GetUserId(userId);
                var project = _projectUser.GetProjectId(projectId);
                if (user == null || project == null)
                {
                    return null;
                }

                var abc = _projectUser.Remove(user);
                //var 

                return Ok(abc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSRE")]
        public async Task<IActionResult> GetSRE()
        {
            var sre = await _sreDetails.GetAllSRE();

            if (sre == null)
            {
                return NotFound();
            }
            var SREDto = new List<Models.SREdetailsDTO>();

            sre.ToList().ForEach(sre =>
            {
                var SreDTO = new Models.SREdetailsDTO()
                {
                    MobileNumber = sre.MobileNumber,
                    SRE = sre.SRE,
                    Email = sre.Email,

                };
                SREDto.Add(SreDTO);


            });

            return Ok(SREDto);
        }

        [HttpPost]
        [Route("AddSRE")]
        public IActionResult AddSRE(SREDetaile sRE)
        {
            try
            {

                var SRE = new SREDetaile()
                {
                    SRE = sRE.SRE,
                    MobileNumber = sRE.MobileNumber,
                    Email = sRE.Email,
                    IsActive = sRE.IsActive,
                };

                _sreDetails.AddSRE(SRE);

                return Ok(SRE);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("ModifySRE")]
        public async Task<IActionResult> ModifySRE(int Id, SREdetailsDTO Sredetail)
        {
            try
            {


                var SRE = new SREDetaile()
                {
                    SRE = Sredetail.SRE,
                    MobileNumber = Sredetail.MobileNumber,
                    Email = Sredetail.Email,
                    //IsActive = Sredetail.IsActive,
                };

                await _sreDetails.UpdateSRE(Id, SRE);

                return Ok(SRE);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSRE")]
        public async Task<IActionResult> DeleteSRE(int Id)
        {
            var Delete = await _sreDetails.DeleteSRE(Id);

            if (Delete == null)
            {
                return NotFound();
            }

            var DeleteDTO = new SREDetaile()
            {
                Id = Delete.Id,
                SRE = Delete.SRE,
                MobileNumber = Delete.MobileNumber,
                Email = Delete.Email,
                IsActive = Delete.IsActive,
            };

            return Ok(DeleteDTO);
        }

    }
}
