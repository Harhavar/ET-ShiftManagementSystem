using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.CommentModel;
using ET_ShiftManagementSystem.Models.OrganizationRoleModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class OrganizationRoleController : Controller
    {
        private readonly IOrganizationRoleServices _roleServices;

        public OrganizationRoleController(IOrganizationRoleServices roleServices)
        {
            _roleServices = roleServices;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<OrganizationRole>>> GetOrganizationRole()
        {
            var role = await _roleServices.GetRoles();

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
        [HttpGet("Global")]

        public async Task<ActionResult<IEnumerable<OrganizationRole>>> GetGlobalRole()
        {
            var role = await _roleServices.GetGlobalRoles();

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
        [HttpGet("ViewOrgRole")]
        //[Route("{id:guid}")]
        public async Task<ActionResult<IEnumerable<OrganizationRole>>> GetOrganizationViewRole(Guid guid)
        {
            var role = await _roleServices.GetRoles(guid);

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
        [HttpGet("ViewGlobalRole")]
        //[Route("{id:guid}")]
        public async Task<ActionResult<IEnumerable<OrganizationRole>>> GetGlobalViewRole(Guid guid)
        {
            var role = await _roleServices.GetGlobalRoles(guid);

            if (role == null)
            {
                return NotFound();
            }
            //retur dto organizationrole
            //var OrganizationDTO = new List<OrganizationCustomRoleViewRequest>();
            //role.ToList().ForEach(role =>
            //{
            var organizationDTO = new Models.OrganizationRoleModel.GlobalCustomRoleViewRequest()
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

        [HttpPut]

        public async Task<IActionResult> UpdateRole(Guid guid, EditRoleRequest editRole)
        {
            var role = new Entities.OrganizationRole
            {
                RoleName = editRole.RoleName,
                Description = editRole.Description,
                LinkedPermission = editRole.LinkedPermission,

            };
             await _roleServices.EditRoleRequest(guid, role);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);

        }
        [HttpPut("global")]

        public async Task<IActionResult> UpdateGolbalRole(Guid guid, EditRoleRequest editRole)
        {
            var role = new Entities.GlobalRole
            {
                Id= guid,
                RoleName = editRole.RoleName,
                Description = editRole.Description,
                LinkedPermission = editRole.LinkedPermission,
                
            };
            await _roleServices.EditGlobalRoleRequest(guid, role);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);

        }

        [HttpPost]

        public async Task<IActionResult> PostNewRole(EditRoleRequest editRole)
        {
            if(editRole== null)
            {
                return BadRequest();
            }

            var role = new Entities.OrganizationRole
            {
                RoleName = editRole.RoleName,
                Description = editRole.Description,
                LinkedPermission = editRole.LinkedPermission,

            };
            await _roleServices.PostRole(role);

            return CreatedAtAction(nameof(GetOrganizationViewRole), new { Id = role.Id }, role);
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var delete = await _roleServices.DeleteRoleRequest(id);

            if(delete == null)
            {
                return NotFound();
            }
            return Ok(delete);
        }
        [HttpDelete("Global")]
        
        public async Task<IActionResult> DeleteGlobalRole(Guid id)
        {
            var delete = await _roleServices.DeleteGlobalRoleRequest(id);

            if (delete == null)
            {
                return NotFound();
            }
            return Ok(delete);
        }
    }
}
