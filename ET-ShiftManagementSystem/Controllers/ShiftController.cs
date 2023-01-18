using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using System.Data;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ShiftController : Controller
    {
        private readonly IShiftServices shiftServices;
        private readonly IMapper mapper;

        public ShiftController(IShiftServices shiftServices , IMapper mapper)
        {
            this.shiftServices = shiftServices;
            this.mapper = mapper;
        }

        [HttpGet]
        //[Route("All/")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShifts()
        {
            var shift =  await shiftServices.GetAllShiftAsync();

            if(shift == null)
            {
                return BadRequest();
            }

            var ShiftDTO = mapper.Map<List<Models.ShiftDTO>>(shift);

            return Ok(ShiftDTO);
        }

        [HttpGet]
        [Route("{id}")]
        // [Authorize(Roles = "Admin")]
        [Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> GetShiftByID(int id)
        {
            var shift = await shiftServices.GetShiftById(id);

            if(shift == null)
            {
                return BadRequest();
            }

            var ShiftDTO = mapper.Map<Models.ShiftDTO>(shift);

            return Ok(ShiftDTO);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddShift(Shift shiftDTO)
        {
            if (shiftDTO == null)
            {
                return BadRequest();
            }
            try
            {

            

            var shift = new Shift()
            {
                ShiftName = shiftDTO.ShiftName,
                StartTime    = DateTime.Now,
                EndTime = DateTime.Now,

            };
            shiftServices.AddSift(shift);

            return Ok(shift);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateShift(int id,Models.UpdateShiftRequest shiftDTO)
        {
            try
            {

            
            var shift = new ET_ShiftManagementSystem.Entities.Shift()
            {
                ShiftId = shiftDTO.ShiftId,
                ShiftName = shiftDTO.ShiftName,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
                
            };

            await shiftServices.UpdateShiftAsync(id, shift);

            return Ok(shift);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteShiftAsync(int Id)
        {
            try
            {

            
            var delete = await shiftServices.DeleteShiftAsync(Id);

            if (delete == null)
            {
                return NotFound();
            }

            var DeleteDTO = new Models.ShiftDTO()
            {

                ShiftId  = delete.ShiftId,
                ShiftName = delete.ShiftName,
                StartTime = delete.StartTime,
                EndTime = delete.EndTime,
                
            };

            return Ok(DeleteDTO);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
