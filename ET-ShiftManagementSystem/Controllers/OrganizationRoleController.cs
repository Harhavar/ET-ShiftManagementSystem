using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.CommentModel;
using ET_ShiftManagementSystem.Models.OrganizationRoleModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    public class OrganizationRoleController : Controller
    {
        private readonly IOrganizationRoleServices _roleServices;

        public OrganizationRoleController(IOrganizationRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        /// <summary>
        /// View Organization roles 
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public IActionResult GetOrganizationRole()
        {
            try
            {
                var role = _roleServices.GetRoles();

                if (role == null)
                {
                    return NotFound();
                }
                //retur dto organizationrole
                var OrganizationDTO = new List<OrganizationRoleViewRequest>();
                role.ToList().ForEach(role =>
                {
                    var organizationDTO = new Models.OrganizationRoleModel.OrganizationRoleViewRequest()
                    {
                        RoleName = role.RoleName,
                        Description = role.Description,
                        RoleType = role.RoleType,
                        CreatedDate = role.CreatedDate,
                        LinkedPermission = role.LinkedPermission,


                    };
                    OrganizationDTO.Add(organizationDTO);


                });

                return Ok(OrganizationDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// View Global roles we follow in Our Application
        /// </summary>
        /// <returns></returns>
        [HttpGet("Global")]

        public IActionResult GetGlobalRole()
        {
            try
            {
                var role =  _roleServices.GetGlobalRoles();

                if (role == null)
                {
                    return NotFound();
                }
                //retur dto organizationrole
                var OrganizationDTO = new List<GlobalRoleViewRequest>();
                role.ToList().ForEach(role =>
                {
                    var organizationDTO = new Models.OrganizationRoleModel.GlobalRoleViewRequest()
                    {
                        RoleName = role.RoleName,
                        Description = role.Description,
                        //RoleType = role.RoleType,
                        CreatedDate = role.CreatedDate,
                        LinkedPermission = role.LinkedPermission,


                    };
                    OrganizationDTO.Add(organizationDTO);


                });

                return Ok(OrganizationDTO);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// View Perticular Organization Role By Giving RoleID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("ViewOrgRole")]
        //[Route("{id:guid}")]
        public IActionResult GetOrganizationViewRole(Guid guid)
        {
            try
            {
                var role = _roleServices.GetRoles(guid);

                if (role == null)
                {
                    return NotFound();
                }
                //retur dto organizationrole
                //var OrganizationDTO = new List<OrganizationCustomRoleViewRequest>();
                //role.ToList().ForEach(role =>
                //{
                var organizationDTO = new Models.OrganizationRoleModel.OrganizationCustomRoleViewRequest()
                {
                    RoleName = role.RoleName,
                    Description = role.Description,
                    //RoleType = role.RoleType,
                    CreatedDate = role.CreatedDate,
                    LinkedPermission = role.LinkedPermission,
                    LastModifiedDate = role.LastModifiedDate,

                };
                //OrganizationDTO.Add(organizationDTO);


                //});

                return Ok(organizationDTO);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
            

        }
        /// <summary>
        /// View perticular Global role by giving Role Id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("ViewGlobalRole")]
        //[Route("{id:guid}")]
        public IActionResult GetGlobalViewRole(Guid roleId)
        {
            if (Guid.Empty == roleId)
            {
                return BadRequest();
            }
            try
            {
                var role = _roleServices.GetGlobalRoles(roleId);

                if (role == null)
                {
                    return NotFound();
                }

                var organizationDTO = new Models.OrganizationRoleModel.GlobalCustomRoleViewRequest()
                {
                    RoleName = role.RoleName,
                    Description = role.Description,

                    CreatedDate = role.CreatedDate,
                    LinkedPermission = role.LinkedPermission,
                    LastModifiedDate = role.LastModifiedDate,

                };

                return Ok(organizationDTO);
            }
            catch (Exception ex )
            {

                return BadRequest(ex.Message);
            }
            
        }


        /// <summary>
        /// Update Organization Role
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="editRole"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateRole(Guid guid, EditRoleRequest editRole)
        {
            if (editRole.RoleName == null || editRole.Description == null || editRole.LinkedPermission == null)
            {
                return BadRequest();
            }
            try
            {
                var role = new Entities.OrganizationRole
                {
                    RoleName = editRole.RoleName,
                    Description = editRole.Description,
                    LinkedPermission = editRole.LinkedPermission,
                };
                var responce = _roleServices.EditRoleRequest(guid, role);

                if (responce != null)
                {
                    return Ok(role);
                }

                return NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }         
        }


        /// <summary>
        /// Update Global Role
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="editRole"></param>
        /// <returns></returns>
        [HttpPut("global")]
        public IActionResult UpdateGolbalRole(Guid guid, EditRoleRequest editRole)
        {
            try
            {
                var role = new Entities.GlobalRole
                {
                    Id = guid,
                    RoleName = editRole.RoleName,
                    Description = editRole.Description,
                    LinkedPermission = editRole.LinkedPermission,

                };
                role = _roleServices.EditGlobalRoleRequest(guid, role);

                if (role == null)
                {
                    return NotFound();
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Add new Organization role 
        /// </summary>
        /// <param name="editRole"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostNewRole(EditRoleRequest editRole)
        {

            if(editRole.RoleName == null || editRole.LinkedPermission == null || editRole.Description == null)
            {
                return BadRequest();
            }
            try
            {
                var role = new Entities.OrganizationRole
                {
                    RoleName = editRole.RoleName,
                    Description = editRole.Description,
                    LinkedPermission = editRole.LinkedPermission,

                };
                role = _roleServices.PostRole(role);
                if (role == null)
                {
                    return BadRequest();
                }
                return CreatedAtAction(nameof(GetOrganizationViewRole), new { Id = role.Id }, role);
            }
            catch (Exception ex)  
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Organization role 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]

        public IActionResult DeleteRole(Guid id)
        {
            try
            {
                var delete = _roleServices.DeleteRoleRequest(id);

                if (delete == null)
                {
                    return NotFound();
                }
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        /// <summary>
        /// Delete global Role 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Global")]
        
        public IActionResult DeleteGlobalRole(Guid id)
        {
            try
            {
                var delete =  _roleServices.DeleteGlobalRoleRequest(id);

                if (delete == null)
                {
                    return NotFound();
                }
                return Ok(delete);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

    }
}
