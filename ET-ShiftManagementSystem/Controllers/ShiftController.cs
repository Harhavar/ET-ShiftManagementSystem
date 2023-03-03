using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using System.Data;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.ShiftModel;
//using Microsoft.Graph;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [EnableCors("CorePolicy")]
    public class ShiftController : Controller
    {
        private readonly IShiftServices shiftServices;
        private readonly IMapper mapper;

        public ShiftController(IShiftServices shiftServices, IMapper mapper)
        {
            this.shiftServices = shiftServices;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Route("All/")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShifts()
        {
            var shift = await shiftServices.GetAllShiftAsync();

            if (shift == null)
            {
                return NotFound();
            }

            var ShiftDTO = mapper.Map<List<ShiftDTO>>(shift);

            return Ok(ShiftDTO);
        }
        [HttpGet("GetTenantShift")]
        //[Route("All/")]
        // [Authorize(Roles = "Admin")]
        public IActionResult GetAllShift(Guid TenatID)
        {
            var shift =  shiftServices.GetAllShift(TenatID);

            //if (shift == null)
            //{
            //    return NotFound();
            //}

            //var ShiftDTO = mapper.Map<List<ShiftDTO>>(shift);

            return Ok(shift);
        }

        [HttpGet]
        [Route("{id}")]
        // [Authorize(Roles = "Admin")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> GetShiftByID(Guid id)
        {
            var shift = await shiftServices.GetShiftById(id);

            if (shift == null)
            {
                return NotFound();
            }

            var ShiftDTO = mapper.Map<ShiftDTO>(shift);

            return Ok(ShiftDTO);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddShift([FromForm]Shift shiftDTO)
        {
            if (shiftDTO == null)
            {
                return NotFound();
            }
            try
            {

                var shift = new Shift()
                {
                    ShiftName = shiftDTO.ShiftName,
                    StartTime = shiftDTO.StartTime,
                    EndTime = shiftDTO.EndTime,

                };
                shiftServices.AddSift(shift);

                return Ok(shift);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateShift(UpdateShiftRequest shiftDTO)
        {
            if (shiftDTO.ShiftID == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {
                var shift = new ET_ShiftManagementSystem.Entities.Shift()
                {
                    ShiftID = shiftDTO.ShiftID,
                    ShiftName = shiftDTO.ShiftName,
                    StartTime = shiftDTO.StartTime,
                    EndTime = shiftDTO.EndTime

                };
                shiftServices.UpdateShiftAsync(shift);
                return Ok(shift);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteShiftAsync(Guid Id)
        {
            try
            {


                var delete = await shiftServices.DeleteShiftAsync(Id);

                if (delete == null)
                {
                    return NotFound();
                }

                //var DeleteDTO = new Models.ShiftModel.ShiftDTO()
                //{

                //    ShiftID = delete.ShiftID,
                //    ShiftName = delete.ShiftName,
                //    StartTime = delete.StartTime,
                //    EndTime = delete.EndTime,

                //};

                return Ok(Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
