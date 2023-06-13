using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.AlertModel;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    //[EnableCors("CorePolicy")]
    [Route("api/[Controller]")]
    [EnableCors("CorePolicy")]
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
        //[Authorize(Roles = "SystemAdmin,SuperAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

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
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

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
        /// Get Todays Alerts 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("CorePolicy")]
        // [Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult<IEnumerable<Alert>>> GetTodaysAlert()
        {
            try
            {
                Log.Information("LoggerInfo");
                var alert = alertServices.GetTodaysAlert();

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
                        ReportedToName = alert.ReportedToName,
                        ReportedByName = alert.ReportedByName,

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
        /// get all alert
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

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
                        ReportedByName=alert.ReportedByName,
                        ReportedToName=alert.ReportedToName,
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

        [HttpGet]
        [Route("Alert/{ProjectId}")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public IActionResult GetAllalert(Guid ProjectId)
        {
            if (ProjectId == null)
            {
                return BadRequest("ProjectId should not be null");
            }
            try
            {

                var alert = alertServices.GetAllAlert(ProjectId);

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
                        ReportedToName= alert.ReportedToName,
                        ReportedByName= alert.ReportedByName,
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
        [HttpGet]
        [Route("{AlertId}")]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public IActionResult GetAlertById(Guid AlertId)
        {
            if (AlertId == null)
            {
                return BadRequest("ProjectId should not be null");
            }
            try
            {

                var alert = alertServices.GetAlertById(AlertId);

                if (alert == null)
                {
                    return BadRequest();
                }


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
                    ReportedByName = alert.ReportedByName,
                    ReportedToName = alert.ReportedToName,
                };

                return Ok(alertDTO);
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
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult<IEnumerable<Alert>>> GetAllAlertBySeveority(severityLevel severity)
        {
            try
            {
                var alert = alertServices.GetAllAlertBySeveority(severity);

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
                        ReportedToName= alert.ReportedToName,
                        ReportedByName= alert.ReportedByName,
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


        /// <summary>
        /// Add alert To the perticular organization
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="alert"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public async Task<ActionResult> addAlert(Guid ProjectID, [FromBody] AddAlertRequest alert)
        {
            if (ProjectID == Guid.Empty || alert == null)
            {
                return BadRequest();
            }
            try
            {

                if (alert == null)
                {
                    return BadRequest();
                }

                var responce = await alertServices.AddAlert(ProjectID, alert.alertRequest, alert.severity);
                if (responce == null)
                {
                    return BadRequest();
                }

                return Ok(responce);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        /// <summary>
        /// Update alert Using alert id
        /// </summary>
        /// <param name="AlertId"></param>
        /// <param name="UpdateAlertStatus"></param>
        /// <returns></returns>
        [HttpPut("UpdateAlert/{AlertId}")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        public IActionResult UpdateAlertStatus(Guid AlertId, UpdateAlertStatus UpdateAlertStatus)
        {

            if (AlertId == null || UpdateAlertStatus == null)
            {
                return BadRequest("AlertId should not be null");
            }
            try
            {
                var alert = alertServices.UpdateAlertStatus(AlertId, UpdateAlertStatus);
                if (alert == null)
                {
                    return NotFound("Alert not found");
                }
                return Ok("Updated");
            }
            catch (Exception)
            {

                throw;
            }
        }
        //[HttpPut("EditAlert /{AlertId}")]
        //[Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

        //public IActionResult EditAlert(Guid AlertId, AlertRequest UpdateAlertStatus)
        //{

        //    if (AlertId == null || UpdateAlertStatus == null)
        //    {
        //        return BadRequest("AlertId should not be null");
        //    }
        //    try
        //    {
        //        var alert = alertServices.UpdateAlertStatus(AlertId, UpdateAlertStatus);
        //        if (alert == null)
        //        {
        //            return NotFound("Alert not found");
        //        }
        //        return Ok("Edited");
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        /// <summary>
        /// Delete alert by giving alert Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Authorize(Roles = "SystemAdmin")]
        [Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]

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
                    ReportedByName= delete.ReportedByName,
                    ReportedToName  = delete.ReportedToName,
                    CreatedDate= delete.CreatedDate,

                };

                return Ok(alertDTO);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Get timeline and updates of alert 
        /// </summary>
        /// <param name="AlertId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAlertUpdates/{AlertId}")]
        //[Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]
        public IActionResult GetAlertUpdates(Guid AlertId)
        {
            if (AlertId == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {

                var alert = alertServices.GetAlertUpdates(AlertId);

                if (alert == null)
                {
                    return BadRequest();
                }

                return Ok(alert);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        /// <summary>
        /// Post timeline and updates
        /// </summary>
        /// <param name="AlertId"></param>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostAlertUpdates/{AlertId}")]
        //[Authorize(Roles = "Admin ,SuperAdmin,SystemAdmin,User")]
        public IActionResult PostAlertUpdates(Guid AlertId, TimelineUpdateModel updateModel)
        {
            if (AlertId == Guid.Empty || updateModel == null)
            {
                return BadRequest();
            }
            try
            {

                var alert = alertServices.PostAlertUpdates(AlertId, updateModel);

                if (alert == null)
                {
                    return BadRequest();
                }

                return Ok(alert);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }
}
