using AutoMapper.Configuration.Annotations;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.NotesModel;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.ProjectModel;
using ET_ShiftManagementSystem.Models.ProjectsModel;
using ET_ShiftManagementSystem.Models.ShiftModel;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Pipelines.Sockets.Unofficial.Arenas;
using System.Drawing.Drawing2D;
using static ET_ShiftManagementSystem.Servises.ShiftServices;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableCors("CorePolicy")]
    public class ProjectsController : Controller
    {
        private readonly IProjectServices _projectServices;
        private readonly IUserRepository _userRepository;
        private readonly IShiftServices _shiftServices;

        public ProjectsController(IProjectServices projectServices, IUserRepository userRepository, IShiftServices shiftServices)
        {
            _projectServices = projectServices;
            _userRepository = userRepository;
            _shiftServices = shiftServices;
        }

        /// <summary>
        /// Get All Peojects in Application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GETAllProjects")]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Entities.Projects>>> GetProjectsData()
        {
            try
            {
                var Organization = await _projectServices.GetProjectsData();

                if (Organization == null)
                {
                    return NotFound();
                }

                var OrganizationRequest = new List<ProjectsViewRequest>();

                Organization.ToList().ForEach(Organization =>
                {
                    var organizationRequest = new ProjectsViewRequest()
                    {
                        Name = Organization.Name,
                        Description = Organization.Description,
                        Status = Organization.Status,
                        CreatedDate = Organization.CreatedDate,
                    };
                    OrganizationRequest.Add(organizationRequest);

                });

                return Ok(OrganizationRequest);

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Get All Project Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectDetails")]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Entities.Projects>>> GetDetails()
        {
            try
            {
                var Organization = await _projectServices.GetProjectsData();

                if (Organization == null)
                {
                    return NotFound();
                }

                var OrganizationRequest = new List<ProjectDetailsRequest>();

                Organization.ToList().ForEach(Organization =>
                {
                    var organizationRequest = new ProjectDetailsRequest()
                    {
                        Name = Organization.Name,
                        Description = Organization.Description,
                        Status = Organization.Status,
                        CreatedDate = Organization.CreatedDate,
                        LastModifiedDate = Organization.LastModifiedDate,
                    };
                    OrganizationRequest.Add(organizationRequest);

                });

                return Ok(OrganizationRequest);

            }
            catch (Exception Ex)
            {

                return Ok(Ex.Message);
            }

        }


        /// <summary>
        /// Count of projects Present in Application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectCount")]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Entities.Projects>>> GetCount()
        {
            try
            {
                var Organization = await _projectServices.GetProjectsData();

                return Ok(Organization.Count());

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }
       
        /// <summary>
        /// Get Perticular project By giving Id 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpGet("GETSingleProject/{ProjectId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectById(Guid ProjectId)
        {
            try
            {
                var responce = await _projectServices.GetProjectById(ProjectId);

                if (responce == null)
                {
                    return NotFound();
                }
                var ProjectDetails = new List<ProjectDetailsRequest>();

                var projectDetails = new ProjectDetailsRequest()
                {
                    Name = responce.Name,
                    Description = responce.Description,
                    Status = responce.Status,
                    CreatedDate = responce.CreatedDate,
                    LastModifiedDate = responce.LastModifiedDate,

                };
                ProjectDetails.Add(projectDetails);


                return Ok(ProjectDetails);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Add Project in Perticular Organization
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="addProject"></param>
        /// <returns></returns>
        [HttpPost("AddProject/{tenantId}")]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> AddProject(Guid tenantId, [FromBody] AddProjectRequest addProject)
        {
            try
            {
                var Project = new Entities.Projects()
                {

                    Name = addProject.Name,
                    Description = addProject.Description,

                };

                Project = await _projectServices.AddProject(tenantId, Project);

                var userShifts = new List<UserShift>();

                addProject.UserShift.ToList().ForEach(Organization =>
                {
                    var userShift = new UserShift()
                    {
                        ShiftId = Organization.ShiftId,
                        UserId = Organization.UserId,
                        ProjectId = Project.ProjectId,
                        ID = Guid.NewGuid(),
                    };
                    userShifts.Add(userShift);

                });

                _shiftServices.Update(userShifts);


                return CreatedAtAction(nameof(GetProjectById), new { Id = Project.ProjectId }, Project);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Update Project 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="addProject"></param>
        /// <returns></returns>
        [HttpPut("UpdateProject/{ProjectId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject(Guid ProjectId, [FromBody] AddProjectRequest addProject)
        {
            try
            {
                var Project = new Entities.Projects()
                {

                    Name = addProject.Name,
                    Description = addProject.Description,

                };


                Project = await _projectServices.EditProject(ProjectId, Project);

                var ExistingUserShift = _projectServices.UserShift(ProjectId);

                //foreach (var item in ExistingUserShift)
                //{
                //   var A = ExistingUserShift.Where(x => x.ProjectId == ProjectId).FirstOrDefault(); 
                //    var  B = addProject.UserShift.Select(x => x.ShiftId).FirstOrDefault();
                //    var c = addProject.UserShift.Select(x => x.UserId).FirstOrDefault();
                //    A.ShiftId = B;
                //    A.UserId = c;

                //    _
                //}



                return Ok(ExistingUserShift);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Delete Project 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProject/{ProjectId}")]

        public async Task<IActionResult> DeleteProject(Guid ProjectId)
        {
            try
            {
                var delete = await _projectServices.DeleteProject(ProjectId);

                if (delete == null)
                {
                    return NotFound();
                }

                return Ok(delete);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }
        //[HttpPost]
        //[Route("Add-User-To-Shift")]
        ////[Authorize(Roles = "SuperAdmin")]
        //public IActionResult AddUserToProject(Guid ProjectID, UserShiftModel shiftUser)
        //{
        //    try
        //    {
        //        var user = _userRepository.Get(shiftUser.UserId);
        //        var Shift = _shiftServices.GetShiftById(shiftUser.ShiftId);
        //        if (user == null || Shift == null)
        //        {
        //            return BadRequest();
        //        }
        //        var projectUser = new Entities.UserShift()
        //        {
        //            ID = Guid.NewGuid(),
        //            UserId = shiftUser.UserId,
        //            ShiftId = shiftUser.ShiftId,
        //            ProjectId = ProjectID,
        //        };

        //        //_shiftServices.Update(projectUser);

        //        return Ok(projectUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// Getting Assigned user to the shift along with his name and shift name 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpGet("SREAssignment/{ProjectId}")]
        public async Task<IActionResult> GetShiftUser(Guid ProjectId)
        {
            try
            {
                var users = await _shiftServices.userShifts(ProjectId);

                var shiftUsers = new List<ShiftUserDetails>();
                users.ToList().ForEach(Organization =>
                {
                    var note = new ShiftUserDetails()
                    {
                        username = Organization.username,
                    };
                    shiftUsers.Add(note);

                });

                var Shift = await _shiftServices.UserShiftName(ProjectId);

                var ShiftNames = new List<ShiftNames>();
                Shift.ToList().ForEach(Organization =>
                {
                    var note = new ShiftNames()
                    {
                        ShiftName = Organization.ShiftName
                    };
                    ShiftNames.Add(note);

                });

                var responce = new UserShiftResponce
                {
                    Shifts = ShiftNames,
                    Users = shiftUsers,
                };
                return Ok(responce);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// user details in shift1 , shift2 , shift3 
        /// by giving shift Id we are able to fetch the details of the user in a shift 
        /// </summary>
        /// <param name="ShiftId"></param>
        /// <returns></returns>
        [HttpGet("User_in_Shift/{ShiftId}")]

        public async Task<IActionResult> GetUserDetails(Guid ShiftId)
        {
            try
            {
                var users = await _shiftServices.userShiftsDetails(ShiftId);

                var shiftUsers = new List<UserDetailsInShift>();
                users.ToList().ForEach(Organization =>
                {
                    var note = new UserDetailsInShift()
                    {
                        username = Organization.username,
                        Email = Organization.Email,
                        AlternateContactNumber = Organization.AlternateContactNumber,
                        ContactNumber = Organization.ContactNumber,
                    };
                    shiftUsers.Add(note);

                });
                return Ok(shiftUsers);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }
        /// <summary>
        /// Add SRE to the project 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UpdateExisting"></param>
        /// <returns></returns>
        [HttpPut("SREAssignment/Alter/{ProjectId}")]
        [HttpPost("SREAssignment/Add/{ProjectId}")]
        public IActionResult Update(Guid ProjectId, [FromBody] List<addShift> UpdateExisting)
        {
            try
            {
                var existingProject = _projectServices.GetProjectById(ProjectId);

                if (existingProject != null)
                {
                    _shiftServices.UpdateExisting(ProjectId, UpdateExisting);
                    return Ok();
                }
                else
                {
                    var userShifts = new List<UserShift>();
                    //UpdateExisting.UserShift.ToList().ForEach(Organization =>
                    //{
                    while (UpdateExisting.Count > 0)
                    {
                        var userShift = new UserShift()
                        {
                            ShiftId = UpdateExisting.Select(x => x.ShiftId).FirstOrDefault(),
                            UserId = UpdateExisting.Select(x => x.UserId).FirstOrDefault(),
                            ProjectId = ProjectId,
                            ID = Guid.NewGuid(),
                        };
                        userShifts.Add(userShift);
                       
                    }
                    //});
                    _shiftServices.Update(userShifts);
                    return Ok();
                }
                return BadRequest("Project not found ");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
