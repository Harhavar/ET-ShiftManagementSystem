using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.PermissionModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    public class PermissionController : Controller
    {
        private readonly IPermissionServises permissionServises;
        private readonly IMapper mapper;

        public PermissionController(IPermissionServises permissionServises, IMapper mapper)
        {
            this.permissionServises = permissionServises;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get Global permission 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[Controller]")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult GetGlobalPermission()
        {
            try
            {
                var perm =  permissionServises.GetPermissions();

                if (perm.Count == 0)
                {
                    return NotFound();
                }
                var PermissionRequest = new List<Permission>();

                perm.ToList().ForEach(permission =>
                {
                    var permissionRequest = new Permission()
                    {
                        PermissionName = permission.PermissionName,
                        Description = permission.Description,
                        CreatedDate = permission.CreatedDate,
                    };
                    PermissionRequest.Add(permissionRequest);

                });
                if(PermissionRequest == null)
                {
                    return NotFound();
                }
                return Ok(PermissionRequest);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Get perticular global permission by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[Controller]/{id:guid}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> GetPermissions(Guid id)
        {
            try
            {
                var perm = await permissionServises.GetPermissionById(id);
                if (perm == null)
                {
                    return NotFound();
                }

                var PermissionRequest = mapper.Map<PermissionDTO>(perm);

                return Ok(PermissionRequest);

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
           
        }


        /// <summary>
        /// Add Global permission  
        /// </summary>
        /// <param name="addPermission"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[Controller]")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> Addpermission(AddPermissionRequest addPermission)
        {
            if (addPermission == null)
            {
                return BadRequest("Should not be null");
            }
            try
            {
                var perm = new Permission
                {
                    PermissionName = addPermission.PermissionName,
                    Description = addPermission.Description,
                };

                perm = await permissionServises.AddPermission(perm);

                if (perm == null)
                {
                    return BadRequest();
                }
                return Ok(perm);
            }   
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
            
        }

        /// <summary>
        /// Update Global Permission 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatePermission"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[Controller]/{id:guid}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> UpdatePermission([FromRoute] Guid id, [FromBody] UpdatePermissionRequest updatePermission)
        {
            if (updatePermission == null)
            {
                return BadRequest("Should not be null");
            }
            try
            {
                var parm = new Permission
                {
                    PermissionName = updatePermission.PermissionName,
                    Description = updatePermission.Description,

                };

                parm = await permissionServises.EditPermission(id, parm);

                if (parm == null)
                {
                    return NotFound();
                }

                var permissionDTO = new Models.PermissionModel.PermissionDTO()
                {
                    PermissionName = parm.PermissionName,
                    Description = parm.Description,
                    CreatedDate = parm.CreatedDate,
                    Id = parm.Id,
                    LastModifiedDate = parm.LastModifiedDate,

                };
                return Ok(permissionDTO);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Delete Global permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[Controller]")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult Delete(Guid id)
        {
            try
            {
                var delete =  permissionServises.DeletePermission(id);
                if (delete == null)
                {
                    return NotFound();
                }
                return Ok(delete);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
           
        }
        /// <summary>
        /// Get organization permission 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("org/[Controller]")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult GetOrganizationPermission()
        {
            try
            {
                var perm =  permissionServises.GetOrgPermissions();

                if (perm == null)
                {
                    return NotFound();
                }
                var PermissionRequest = new List<GetOrgPermission>();

                perm.ToList().ForEach(permission =>
                {
                    var permissionRequest = new GetOrgPermission()
                    {
                        PermissionName = permission.PermissionName,
                        Description = permission.Description,
                        CreatedDate = permission.CreatedDate,
                        PermissionType = permission.PermissionType,

                    };
                    PermissionRequest.Add(permissionRequest);

                });

                return Ok(PermissionRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
           

        }

        /// <summary>
        /// Get organization permission By giving Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        [Route("org/[Controller]/{id:guid}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult GetOrgPermissions(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest();
            }
            try
            {
                var perm = permissionServises.GetOrgPermissionById(id);
                if (perm == null)
                {
                    return NotFound();
                }

                var PermissionRequest = new List<GetOrgPermissionview>();
                
                var permissionRequest = new GetOrgPermissionview()
                {
                    PermissionName = perm.PermissionName,
                    Description = perm.Description,
                    CreatedDate = perm.CreatedDate,
                    LastModifiedDate = perm.LastModifiedDate,

                };
                PermissionRequest.Add(permissionRequest);

                return Ok(PermissionRequest);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// add new organization permission 
        /// </summary>
        /// <param name="addPermission"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("org/[Controller]")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> AddOrgpermission(AddOrgPermission addPermission)
        {

            if (addPermission == null)
            {
                return BadRequest("Should not be null");
            }
            try
            {
                var perm = new OrgPermission
                {
                    PermissionName = addPermission.PermissionName,
                    Description = addPermission.Description,
                };

                perm = await permissionServises.AddOrgPermission(perm);

                return Ok(perm);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        /// <summary>
        /// Update Organization permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatePermission"></param>
        /// <returns></returns>
        [HttpPut]

        [Route("org/[Controller]/{id:guid}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public async Task<IActionResult> UpdateOrgPermission([FromRoute] Guid id, [FromBody] UpdateOrgPermission updatePermission)
        {
            if (updatePermission == null || Guid.Empty == id)
            {
                return BadRequest();
            }
            try
            {
                var parm = new OrgPermission
                {
                    PermissionName = updatePermission.PermissionName,
                    Description = updatePermission.Description,
                };

                parm = await permissionServises.EditOrgPermission(id, parm);

                if (parm == null)
                {
                    return NotFound();
                }

                var permissionDTO = new Models.PermissionModel.OrgPermissionDTO()
                {
                    PermissionName = parm.PermissionName,
                    Description = parm.Description,
                    CreatedDate = parm.CreatedDate,
                    Id = parm.Id,
                    LastModifiedDate = parm.LastModifiedDate,
                    PermissionType = parm.PermissionType,

                };
                return Ok(permissionDTO);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Delete Organization 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("org/[Controller]")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin")]

        public IActionResult DeleteOrgPermission(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest();
            }
            try
            {
                var delete = permissionServises.DeleteOrgPermission(id);
                if (delete == false)
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

    }
}
