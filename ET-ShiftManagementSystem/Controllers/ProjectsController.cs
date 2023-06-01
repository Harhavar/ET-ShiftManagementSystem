using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.ProjectsModel;
using ET_ShiftManagementSystem.Models.ShiftModel;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Pipelines.Sockets.Unofficial.Arenas;
using static ET_ShiftManagementSystem.Servises.ShiftServices;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    public class ProjectsController : Controller
    {
        private readonly IProjectServices _projectServices;
        private readonly IUserRepository _userRepository;
        private readonly IShiftServices _shiftServices;
        private readonly ShiftManagementDbContext _context;

        public ProjectsController(IProjectServices projectServices, IUserRepository userRepository, IShiftServices shiftServices )
        {
            _projectServices = projectServices;
            _userRepository = userRepository;
            _shiftServices = shiftServices;
            //_context = context;
        }
        [HttpGet]
        [Route("RecentActivity")]
        [EnableCors("CorePolicy")]
        [Authorize(Roles = "SystemAdmin,Admin,User,SuperAdmin")]
        public IActionResult GetRecentActivity()
        {
            var guid = Guid.NewGuid();
            var project = new Projects();
            var respone = _projectServices.AddProject(guid, project);
            return Ok(respone);
        }
        /// <summary>
        /// Get All Peojects in Application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllProjects")]
        [EnableCors("CorePolicy")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]
        public IActionResult GetProjectsData()
        {
            try
            {
                var Organization =  _projectServices.GetProjectsData();

                if (Organization == null)
                {
                    return NotFound();
                }

                var OrganizationRequest = new List<ProjectsViewRequest>();

                Organization.ToList().ForEach(Organization =>
                {
                    var organizationRequest = new ProjectsViewRequest()
                    {
                        Id=Organization.ProjectId,
                        TenantId=Organization.TenentId,
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

                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Get All Project Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectDetails")]
        [EnableCors("CorePolicy")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public IActionResult GetDetails()
        {
            try
            {
                var Organization =  _projectServices.GetProjectsData();

                if (Organization == null)
                {
                    return NotFound();
                }

                var OrganizationRequest = new List<ProjectDetailsRequest>();

                Organization.ToList().ForEach(Organization =>
                {
                    var organizationRequest = new ProjectDetailsRequest()
                    {
                        ProjectId=Organization.ProjectId,
                        TenantId =Organization.TenentId,
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
        /// Get All Project Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectDetailsWithTenentID")]
        [EnableCors("CorePolicy")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public IActionResult GetTenentProject(Guid tenentId )
        {
            if (Guid.Empty == tenentId)
            {
                return BadRequest();
            }
            try
            {
                var Organization = _projectServices.GetProjectsData(tenentId);

                if (Organization == null)
                {
                    return NotFound();
                }

                var OrganizationRequest = new List<ProjectDetailsRequest>();

                Organization.ToList().ForEach(Organization =>
                {
                    var organizationRequest = new ProjectDetailsRequest()
                    {
                        ProjectId = Organization.ProjectId,
                        TenantId = Organization.TenentId,
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
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public IActionResult GetCount()
        {
            try
            {
                var Organization = _projectServices.GetProjectsData();
               
                return Ok(Organization.Count());

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

        }
        /// <summary>
        /// Count of projects Present in Application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProjectCountOfOrganization")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "SystemAdmin,SuperAdmin")]

        public IActionResult GetCountOrganization(Guid TenentId)
        {
            if (Guid.Empty == TenentId)
            {
                return BadRequest();
            }
            try
            {
                var Organization = _projectServices.GetProjectsCount(TenentId);

                return Ok(Organization);

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
        [HttpGet("SingleProject/{ProjectId}")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public IActionResult GetProjectById(Guid ProjectId)
        {
            if (ProjectId == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                var responce =  _projectServices.GetProjectById(ProjectId);

                if (responce == null)
                {
                    return NotFound();
                }
                //var ProjectDetails = new List<ProjectDetailsRequest>();

                var projectDetails = new ProjectDetailsRequest()
                {
                    ProjectId = responce.ProjectId,
                    TenantId = responce.TenentId,
                    Name = responce.Name,
                    Description = responce.Description,
                    Status = responce.Status,
                    CreatedDate = responce.CreatedDate,
                    LastModifiedDate = responce.LastModifiedDate,

                };
                //ProjectDetails.Add(projectDetails);


                return Ok(projectDetails);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Add Project in Perticular Organization
        /// </summary>
        /// <param name="TenetId"></param>
        /// <param name="addProject"></param>
        /// <returns></returns>
        [HttpPost("AddProject/{TenetId}" , Name = "Post")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]

        public async Task<IActionResult> AddProject(Guid TenetId, [FromBody] AddProjectRequest addProject)
        {
            if (Guid.Empty == TenetId || addProject == null)
            {
                return BadRequest();
            }
            try
            {
                var Project = new Entities.Projects()
                {

                    Name = addProject.Name,
                    Description = addProject.Description,

                };

                Project = await _projectServices.AddProject(TenetId, Project);

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
                


                return CreatedAtAction(nameof(AddProject), new { Id = Project.ProjectId }, Project);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
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
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> UpdateProject(Guid ProjectId, [FromBody] AddProjectRequest addProject)
        {
            if (Guid.Empty == ProjectId || addProject == null)
            {
                return BadRequest();
            }
            try
            {
                var Project = new Entities.Projects()
                {

                    Name = addProject.Name,
                    Description = addProject.Description,

                };


                Project = await _projectServices.EditProject(ProjectId, Project);

                var userShifts = new List<UserShift>();

                addProject.UserShift.ToList().ForEach(Organization =>
                {
                    var userShift = new UserShift()
                    {
                        ShiftId = Organization.ShiftId,
                        UserId = Organization.UserId,
                        ProjectId = Project.ProjectId,
                        //ID = Guid.NewGuid(),
                    };
                    userShifts.Add(userShift);

                });

                _shiftServices.UpdateExisting(ProjectId,userShifts);
                return Ok(Project);
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
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> DeleteProject(Guid ProjectId)
        {
            if (Guid.Empty == ProjectId)
            {
                return BadRequest();
            }
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
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin, User")]

        public async Task<IActionResult> GetShiftUser(Guid ProjectId)
        {
            if(ProjectId == Guid.Empty)
            {
                return BadRequest();
            }
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
        [HttpGet("Users_In_Shift/{ShiftId}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<IActionResult> GetUserDetails(Guid ShiftId)
        {
            if(ShiftId == Guid.Empty)
            {
                return BadRequest();
            }
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
        //[HttpPost("SREAssignment/Add/{ProjectId}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

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
        [HttpPost("SREAssignment/Add/{ProjectId}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult add(Guid ProjectId, [FromBody] List<addShift> UpdateExisting)
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
