using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.AlertModel;
using ET_ShiftManagementSystem.Servises;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [EnableCors("CorePolicy")]
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

        /// <summary>
        /// based on Triggerd time the data will come
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>alerts</returns>
        [HttpGet]
        [Route("TimeWarp")]
        [Authorize(Roles = "SystemAdmin")]
        public IActionResult GetAlerts(DateTime start, DateTime end)
        {
            try
            {
                var alerts = alertServices.GetAlertByFilter(start, end);
                return Ok(alerts);
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message);
            }
           
        }


        /// <summary>
        /// Based on alert name triggerd time returns alert
        /// </summary>
        /// <param name="alertname"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter")]
        [Authorize(Roles = "SystemAdmin")]
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
        //[HttpGet]
        //[Route("Download")]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Download(int id)
        //{
        //    var file = await alertServices.FindAsync(id);
        //    if (file == null)
        //        return NotFound();

        //    var memory = new MemoryStream();
        //    await memory.WriteAsync(file.Content, 0, file.Content.Length);
        //    memory.Position = 0;
        //    return File(memory, file.ContentType, file.FileName);
        //}


        /// <summary>
        /// Get All Alerts 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("CorePolicy")]
       // [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Alert>>> Get()
        {
            try
            {
                var alert = await alertServices.GetAlertAsync();

                if (alert == null)
                {
                    return BadRequest();
                }

                var AlertDto = new List<AlertsDTO>();
                alert.ToList().ForEach(alert =>
                {
                    var alertDTO = new Models.AlertModel.AlertsDTO()
                    {
                        severity = (int)alert.severity,
                        CreatedDate = alert.CreatedDate,
                        AlertName = alert.AlertName,
                        RCA = alert.RCA,
                        TriggeredTime = alert.TriggeredTime,
                        Description = alert.Description,
                        ReportedBy = alert.ReportedBy,
                        ReportedTo = alert.ReportedTo,
                        Id = alert.Id,
                        lastModifiedDate = alert.lastModifiedDate,
                        Status = alert.Status,
                        TenantId = alert.TenantId,
                        ProjectId = alert.ProjectId,

                    };
                    AlertDto.Add(alertDTO);

                });
                return Ok(AlertDto);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
        }
        //GetAllAlertBySeveority(severityLevel severity)
        [HttpGet]
        [Route("all")]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAllalert()
        {
            try
            {

                var alert = await alertServices.GetAllAlert();

                if (alert == null)
                {
                    return BadRequest();
                }

                var AlertDto = new List<AlertsDTO>();
                alert.ToList().ForEach(alert =>
                {
                    var alertDTO = new Models.AlertModel.AlertsDTO()
                    {
                        severity= (int)alert.severity,
                        CreatedDate = alert.CreatedDate,
                        AlertName = alert.AlertName,
                        RCA = alert.RCA,
                        TriggeredTime = alert.TriggeredTime,
                        Description = alert.Description,
                        ReportedBy = alert.ReportedBy,
                        ReportedTo = alert.ReportedTo,
                        Id = alert.Id,
                        lastModifiedDate = alert.lastModifiedDate,
                        Status = alert.Status,
                        TenantId = alert.TenantId,
                        ProjectId = alert.ProjectId,

                    };
                    AlertDto.Add(alertDTO);

                });
                return Ok(AlertDto);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Get alerts by giving severity
        /// </summary>
        /// <param name="severity"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BySeverity")]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAllAlertBySeveority(severityLevel severity)
        {
            try
            {

                var alert = await alertServices.GetAllAlertBySeveority(severity);

                if (alert == null)
                {
                    return NotFound();
                }

                var AlertDto = new List<AlertsDTO>();
                alert.ToList().ForEach(alert =>
                {
                    var alertDTO = new Models.AlertModel.AlertsDTO()
                    {
                        severity = (int)alert.severity,
                        CreatedDate = alert.CreatedDate,
                        AlertName = alert.AlertName,
                        RCA = alert.RCA,
                        TriggeredTime = alert.TriggeredTime,
                        Description = alert.Description,
                        ReportedBy = alert.ReportedBy,
                        ReportedTo = alert.ReportedTo,
                        Id = alert.Id,
                        lastModifiedDate = alert.lastModifiedDate,
                        Status = alert.Status,
                        TenantId = alert.TenantId,
                        ProjectId = alert.ProjectId,
                    };
                    AlertDto.Add(alertDTO);

                });
                return Ok(AlertDto);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        /// <summary>
        /// Add alert To the perticular organization
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="alert"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> addAlert(Guid ProjectID , [FromBody]AddAlertRequest alert)
        {
            try
            {

                if (alert == null)
                {
                    return BadRequest();
                }
                


                 await alertServices.AddAlert(ProjectID ,alert.alertRequest, alert.severity);

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        /// <summary>
        /// Delete alert by giving alert Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<ActionResult> deleteAlert(Guid id)
        {
            try
            {
                var delete = await alertServices.DeleteAlertAsync(id);

                if (delete == null)
                {
                    return NotFound();
                }

                var alertDTO = new Models.AlertModel.AlertsDTO()
                {
                    severity = (int)delete.severity,
                    AlertName = delete.AlertName,
                    RCA = delete.RCA,
                    Description = delete.Description,
                    ReportedBy = delete.ReportedBy,
                    ReportedTo = delete.ReportedTo,
                    TriggeredTime = delete.TriggeredTime,
                    Id = id,
                    lastModifiedDate = delete.lastModifiedDate,
                    Status = delete.Status,
                    TenantId = delete.TenantId,
                    ProjectId = delete.ProjectId,
                };

                return Ok(alertDTO);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }   
            
        }


    }
}
