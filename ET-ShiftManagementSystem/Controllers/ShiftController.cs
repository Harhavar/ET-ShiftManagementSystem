using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.ShiftModel;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
    public class ShiftController : Controller
    {
        private readonly IShiftServices shiftServices;
        private readonly IMapper mapper;
        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="shiftServices"></param>
        /// <param name="mapper"></param>
        public ShiftController(IShiftServices shiftServices, IMapper mapper)
        {
            this.shiftServices = shiftServices;
            this.mapper = mapper;
        }

        /// <summary>
        /// get All Shifts 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("All/")]
         [Authorize(Roles = "Admin,SystemAdmin,SuperAdmin")]
        public  IActionResult GetAllShifts()
        {
            try
            {
                var shift = shiftServices.GetAllShiftAsync();

                if (shift == null)
                {
                    return NotFound();
                }

                var ShiftDTO = mapper.Map<List<ShiftDTO>>(shift);

                return Ok(ShiftDTO);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Tenant Specific Shifts Defined in Organization
        /// </summary>
        /// <param name="TenatID"></param>
        /// <returns></returns>
        [HttpGet("TenantShift/{TenatID}")]
        //[Route("All/")]
        [Authorize(Roles = "Admin,SystemAdmin")]
        public IActionResult GetAllShift(Guid TenatID)
        {
            if(Guid.Empty == TenatID)
            {
                return BadRequest();
            }
            try
            {
                var shift = shiftServices.GetAllShift(TenatID);

                if(shift == null)
                {
                    return NotFound();
                }

                return Ok(shift);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        /// <summary>
        /// Get Perticular Shift By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{Shiftid}")]
        // [Authorize(Roles = "Admin")]
        [Authorize(Roles = "SuperAdmin,Admin,User,SystemAdmin")]
        public async Task<IActionResult> GetShiftByID(Guid Shiftid)
        {
            if (Guid.Empty == Shiftid)
            {
                return BadRequest();
            }
            try
            {
                var shift = await shiftServices.GetShiftById(Shiftid);

                if (shift == null)
                {
                    return NotFound();
                }

                var ShiftDTO = mapper.Map<ShiftDTO>(shift);

                return Ok(ShiftDTO);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Addshift 
        /// </summary>
        /// <param name="shiftDTO"></param>
        /// <returns></returns>
        [HttpPost("{TenantId}")]
        [Authorize(Roles = "SuperAdmin , Admin ,SuperAdmin")]
        public IActionResult AddShift(Guid TenantId , [FromBody] Models.ShiftModel.AddShiftRequest shiftDTO)
        {

            //if (shiftDTO.ShiftName == null || shiftDTO.StartTime==null || shiftDTO.EndTime==null|| Guid.Empty == TenantId || shiftDTO == null)
            //{
            //    return BadRequest();
            //}
            try
            {
                var shift = new Entities.AddShiftRequest()
                {
                    ShiftName = shiftDTO.ShiftName,
                    StartTime = shiftDTO.StartTime,
                    EndTime = shiftDTO.EndTime,

                };
                shiftServices.AddSift(TenantId , shift);

                return Ok(shift);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Update Shift 
        /// </summary>
        /// <param name="shiftDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "SuperAdmin,Admin,SystemAdmin")]
        public async Task<IActionResult> UpdateShift(Guid ShiftId , UpdateShiftRequest shiftDTO)
        {
            if (ShiftId == Guid.Empty || shiftDTO == null)
            {
                return BadRequest();
            }
            try
            {
                
                var responce = await shiftServices.UpdateShiftAsync(ShiftId, shiftDTO);
                if (responce == null)
                {
                    return NotFound();
                }
                return Ok(responce);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        /// <summary>
        /// Delete Shift 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "SuperAdmin,Admin,SystemAdmin")]
        public  IActionResult DeleteShiftAsync(Guid Id)
        {
            if (Guid.Empty == Id)
            {
                return BadRequest();
            }
            try
            {
                var delete =  shiftServices.DeleteShiftAsync(Id);

                if (delete == null)
                {
                    return NotFound();
                }

                return Ok(Id);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
