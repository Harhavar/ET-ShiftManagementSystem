using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.PermissionModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    
    public class PermissionController : Controller
    {
        private readonly IPermissionServises permissionServises;
        private readonly IMapper mapper;

        public PermissionController(IPermissionServises permissionServises, IMapper mapper )
        {
            this.permissionServises = permissionServises;
            this.mapper = mapper;
        }

        //global permission 
        [HttpGet]
        [Route("[Controller]")]
        public async Task<ActionResult<IEnumerable<Entities.Permission>>> Get()
        {
           var perm =  await permissionServises.GetPermissions();

            if(perm == null)
            {
                return NotFound();
            }
            var PermissionRequest = new List<GetGlobalPermission>();

            perm.ToList().ForEach(permission =>
            {
                var permissionRequest = new GetGlobalPermission()
                {
                    PermissionName = permission.PermissionName,
                    Description= permission.Description,
                    CreatedDate = permission.CreatedDate,
                };
                PermissionRequest.Add(permissionRequest);

            });

            return Ok(PermissionRequest);

        }
        [HttpGet]
        [Route("[Controller]/{id:guid}")]
       
        public async Task<IActionResult> GetPermissions(Guid id)
        {
            var perm = await  permissionServises.GetPermissionById(id);
            if(perm == null)
            {
                return NotFound();
            }

            var PermissionRequest = mapper.Map<PermissionDTO>(perm);

            return Ok(PermissionRequest);

            
        }

        [HttpPost]
        [Route("[Controller]")]
        public async Task<IActionResult> Addpermission(AddPermissionRequest addPermission)
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

        [HttpPut]
        [Route("[Controller]/{id:guid}")]
        
        public async Task<IActionResult> UpdatePermission([FromRoute]Guid id ,[FromBody]UpdatePermissionRequest updatePermission)
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
                PermissionName= parm.PermissionName,
                Description= parm.Description,
                CreatedDate= parm.CreatedDate,
                Id= parm.Id,
                LastModifiedDate= parm.LastModifiedDate,
                
            };
            return Ok(permissionDTO);
        }

        [HttpDelete]
        [Route("[Controller]")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var parm = permissionServises.GetPermissionById(id);
            if(parm == null)
            {
                return NotFound();
            }
            
            var delete = permissionServises.DeletePermission(id);

            return Ok(delete);
        }
        //organization permission 
        [HttpGet]
        [Route("org/[Controller]")]
        public async Task<ActionResult<IEnumerable<Entities.Permission>>> GetOrg()
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
                    PermissionType= permission.PermissionType,
                    
                };
                PermissionRequest.Add(permissionRequest);

            });

            return Ok(PermissionRequest);

        }

        //organization permission view only one row 
        [HttpGet]
       
        [Route("org/[Controller]/{id:guid}")]
        public async Task<IActionResult> GetOrgPermissions(Guid id)
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
                    LastModifiedDate= perm.LastModifiedDate,

                };
                PermissionRequest.Add(permissionRequest);

           // });

            return Ok(PermissionRequest);

        }
        //add new organization permission 
        [HttpPost]
        [Route("org/[Controller]")]
       
        public async Task<IActionResult> AddOrgpermission(AddOrgPermission addPermission)
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

        [HttpPut]
       
        [Route("org/[Controller]/{id:guid}")]
        public async Task<IActionResult> UpdateOrgPermission([FromRoute] Guid id, [FromBody] UpdateOrgPermission updatePermission)
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
                PermissionType= parm.PermissionType,

            };
            return Ok(permissionDTO);
        }

        [HttpDelete]
        [Route("org/[Controller]")]
        
        public async Task<IActionResult> DeleteOrgPermission(Guid id)
        {
            //var perm = await permissionServises.GetOrgPermissionById(id);
            var delete = await permissionServises.DeleteOrgPermission(id);
            if (delete == null)
            {
                return NotFound();
            }

            

            return Ok(delete);
        }

    }
}
