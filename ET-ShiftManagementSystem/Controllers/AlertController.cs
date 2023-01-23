using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AlertController : Controller
    {
        private readonly IAlertServices alertServices;
        private readonly IMapper mapper;

        public AlertController(IAlertServices alertServices, IMapper mapper)
        {
            this.alertServices = alertServices;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("filter")]
        public IActionResult GetAlert(string alertname, DateTime from, DateTime? to)
        {
            try
            {


                var alert = alertServices.GetAlertByFilter(alertname, from, to);

                if (alert == null)
                {
                    return NotFound();
                }

                return Ok(alert);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Alert>>> Get()
        {
            var alert = await alertServices.GetAlertAsync();

            if (alert == null)
            {
                return BadRequest();
            }

            var AlertDto = new List<Models.AlertsDTO>();
            alert.ToList().ForEach(alert =>
            {
                var alertDTO = new Models.AlertsDTO()
                {
                    CreatedDate = alert.CreatedDate,
                    AlertName = alert.AlertName,
                    RCA = alert.RCA,
                    TriggeredTime = alert.TriggeredTime,
                    Description = alert.Description,
                    ReportedBy = alert.ReportedBy,
                    ReportedTo = alert.ReportedTo,
                    Id = alert.Id,

                };
                AlertDto.Add(alertDTO);

            });
            return Ok(AlertDto);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAllalert()
        {
            try
            {

                var alert = await alertServices.GetAllAlert();

                if (alert == null)
                {
                    return BadRequest();
                }

                var AlertDto = new List<Models.AlertsDTO>();
                alert.ToList().ForEach(alert =>
                {
                    var alertDTO = new Models.AlertsDTO()
                    {
                        CreatedDate = alert.CreatedDate,
                        AlertName = alert.AlertName,
                        RCA = alert.RCA,
                        TriggeredTime = alert.TriggeredTime,
                        Description = alert.Description,
                        ReportedBy = alert.ReportedBy,
                        ReportedTo = alert.ReportedTo,
                        Id = alert.Id,

                    };
                    AlertDto.Add(alertDTO);

                });
                return Ok(AlertDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]

        public IActionResult addAlert(Alert alert)
        {
            try
            {


                if (alert == null)
                {
                    return BadRequest();
                }

                var aler = new Alert
                {
                    AlertName = alert.AlertName,
                    //Id = alert.Id,
                    RCA = alert.RCA,
                    Description = alert.Description,
                    ReportedBy = alert.ReportedBy,
                    ReportedTo = alert.ReportedTo,
                    TriggeredTime = alert.TriggeredTime,
                    CreatedDate = DateTime.Now
                };

                var alertDTO = alertServices.AddAlert(aler);

                return Ok(alertDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> deleteAlert(int id)
        {
            var delete = await alertServices.DeleteAlertAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            var alertDTO = new Models.AlertsDTO()
            {
                AlertName = delete.AlertName,
                RCA = delete.RCA,
                Description = delete.Description,
                ReportedBy = delete.ReportedBy,
                ReportedTo = delete.ReportedTo,
                TriggeredTime = delete.TriggeredTime,
                Id = id
            };

            return Ok(alertDTO);
        }


    }
}
