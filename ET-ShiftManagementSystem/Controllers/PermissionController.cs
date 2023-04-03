using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.PermissionModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<Entities.Permission>>> Get()
        {
            try
            {
                var perm = await permissionServises.GetPermissions();

                if (perm == null)
                {
                    return NotFound();
                }
                var PermissionRequest = new List<GetGlobalPermission>();

                perm.ToList().ForEach(permission =>
                {
                    var permissionRequest = new GetGlobalPermission()
                    {
                        PermissionName = permission.PermissionName,
                        Description = permission.Description,
                        CreatedDate = permission.CreatedDate,
                    };
                    PermissionRequest.Add(permissionRequest);

                });

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
        public async Task<IActionResult> Addpermission(AddPermissionRequest addPermission)
        {
            try
            {
                if (addPermission == null)
                {
                    return BadRequest("Should not be null");
                }
                var perm = new Permission
                {
                    PermissionName = addPermission.PermissionName,
                    Description = addPermission.Description,
                };

                perm = await permissionServises.AddPermission(perm);

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

        public async Task<IActionResult> UpdatePermission([FromRoute] Guid id, [FromBody] UpdatePermissionRequest updatePermission)
        {
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var parm = permissionServises.GetPermissionById(id);
                if (parm == null)
                {
                    return NotFound();
                }

                var delete = permissionServises.DeletePermission(id);

                return Ok(delete);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        /// <summary>
        /// Get organization permission 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("org/[Controller]")]
        public async Task<ActionResult<IEnumerable<Entities.Permission>>> GetOrg()
        {
            try
            {
                var perm = await permissionServises.GetOrgPermissions();

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
        public async Task<IActionResult> GetOrgPermissions(Guid id)
        {
            try
            {
                var perm = await permissionServises.GetOrgPermissionById(id);
                if (perm == null)
                {
                    return NotFound();
                }

                var PermissionRequest = new List<GetOrgPermissionview>();

                // perm.ToList().ForEach(permission =>
                //{
                var permissionRequest = new GetOrgPermissionview()
                {
                    PermissionName = perm.PermissionName,
                    Description = perm.Description,
                    CreatedDate = perm.CreatedDate,
                    //PermissionType = perm.PermissionType,
                    LastModifiedDate = perm.LastModifiedDate,

                };
                PermissionRequest.Add(permissionRequest);

                // });

                return Ok(PermissionRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// add new organization permission 
        /// </summary>
        /// <param name="addPermission"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("org/[Controller]")]

        public async Task<IActionResult> AddOrgpermission(AddOrgPermission addPermission)
        {
            try
            {
                if (addPermission == null)
                {
                    return BadRequest("Should not be null");
                }
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
        public async Task<IActionResult> UpdateOrgPermission([FromRoute] Guid id, [FromBody] UpdateOrgPermission updatePermission)
        {
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
        public async Task<IActionResult> DeleteOrgPermission(Guid id)
        {
            try
            {
                var delete = await permissionServises.DeleteOrgPermission(id);
                if (delete == null)
                {
                    return NotFound();
                }
                return Ok(delete);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

    }
}
