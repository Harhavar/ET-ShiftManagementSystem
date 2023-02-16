using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.organizationModels;
using ET_ShiftManagementSystem.Models.PermissionModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PermissionController : Controller
    {
        private readonly IPermissionServises permissionServises;
        private readonly IMapper mapper;

        public PermissionController(IPermissionServises permissionServises, IMapper mapper)
        {
            this.permissionServises = permissionServises;
            this.mapper = mapper;
        }

        [HttpGet]
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
        [Route("{id:guid}")]
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
        [Route("{id:guid}")]
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

    }
}
