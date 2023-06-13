using CloudinaryDotNet.Actions;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.AlertModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IAlertServices
    {
        List<Alert> GetTodaysAlert();
        Task<IEnumerable<Alert>> GetAllAlert();
        List<Alert> GetAllAlert(Guid ProjectId);
        List<Alert> GetAllAlertBySeveority(severityLevel severity);

        List<Alert> GetAlertByFilter(string AlertName, DateTime from, DateTime? To);
        List<Alert> GetAlertByFilter(DateTime from, DateTime To);

        Task<Alert> AddAlert(Guid ProjectId, AlertRequest alertRequest, severityLevel sevearity);

        Task UpdateAlertStatus(Guid AlertId, UpdateAlertStatus UpdateAlertStatus);
        Task UpdateAlertStatus(Guid AlertId, AlertRequest UpdateAlertStatus);
        Task<Alert> DeleteAlertAsync(Guid id);

        List<TimelineUpdate> GetAlertUpdates(Guid AlertId);
        TimelineUpdate PostAlertUpdates(Guid AlertId, TimelineUpdateModel updateModel);
        Alert GetAlertById(Guid alertId);
    }
    public class AlertServices : IAlertServices
    {
        private readonly ShiftManagementDbContext _shiftManagementDb;

        public AlertServices(ShiftManagementDbContext shiftManagementDb)
        {
            _shiftManagementDb = shiftManagementDb;
        }

        public async Task<Alert> AddAlert(Guid ProjectId, AlertRequest alertRequest, severityLevel sevearity)
        {
            var projectName = _shiftManagementDb.Projects.Where(x => x.ProjectId == ProjectId).Select(x => x.Name).FirstOrDefault();
            var TenantId = _shiftManagementDb.Projects.Where(x => x.ProjectId == ProjectId).Select(x => x.TenentId).FirstOrDefault();
            var createdbyname = _shiftManagementDb.users.Where(x => x.id == alertRequest.ReportedBy).Select(x => x.username).FirstOrDefault();
            var alert = new Alert
            {
                Id = Guid.NewGuid(),
                AlertName = alertRequest.AlertName,

                Description = alertRequest.Description,
                ReportedBy = alertRequest.ReportedBy,
                ReportedTo = alertRequest.ReportedTo,
                TriggeredTime = alertRequest.TriggeredTime,
                CreatedDate = DateTime.Now,
                severity = sevearity,
                Status = alertRequest.Status,
                lastModifiedDate = DateTime.Now,
                RCA = "",
                ReportedByName = _shiftManagementDb.users.Where(x => x.id == alertRequest.ReportedBy).Select(x => x.username).FirstOrDefault(),
                ReportedToName= _shiftManagementDb.users.Where(x => x.id == alertRequest.ReportedTo).Select(x => x.username).FirstOrDefault(),
                ProjectId = ProjectId,
                TenantId = TenantId,


            };
            var activity = new Activity
            {
                ActivityId = Guid.NewGuid(),
                ProjectName = projectName,
                Action = "Alert Added",
                Message = $"{alertRequest.AlertName} ",
                UserName = "",
                TenetId = TenantId,
                Timestamp = DateTime.Now,
            };
            var timeline = new TimelineUpdate
            {
                Id = Guid.NewGuid(),
                Alertid = alert.Id,
                createdby = alert.ReportedBy,
                Description = alert.Description,
                datetime = alert.CreatedDate,
                status = alert.Status,
                keyvalue = "Description",
                Action = "Alert Created",
                createdbyname = createdbyname,

            };
            _shiftManagementDb.TimelineUpdates.Add(timeline);
            _shiftManagementDb.SaveChanges();
            _shiftManagementDb.Activities.Add(activity);
            _shiftManagementDb.SaveChanges();
            _shiftManagementDb.alerts.Add(alert);
            await _shiftManagementDb.SaveChangesAsync();
            return alert;
        }

        public async Task<Alert> DeleteAlertAsync(Guid id)
        {
            var delete = await _shiftManagementDb.alerts.FirstOrDefaultAsync(x => x.Id == id);
            if (delete == null)
            {
                return null;
            }

            _shiftManagementDb.alerts.Remove(delete);
            _shiftManagementDb.SaveChanges();
            return delete;

        }

        public async Task<IEnumerable<Alert>> GetAllAlert()
        {
            return await _shiftManagementDb.alerts.ToListAsync();
        }
        public List<Alert> GetAllAlertBySeveority(severityLevel severity)
        {
            var alerts = _shiftManagementDb.alerts.Where(x => x.severity == severity).ToList();

            if (alerts == null)
            {
                return null;
            }
            return alerts;
        }

        public List<Alert> GetTodaysAlert()
        {
            var TodaysAlert = _shiftManagementDb.alerts.Where(x => x.CreatedDate.Date == DateTime.Today);
            return TodaysAlert.ToList();
            //await _shiftManagementDb.alerts.ToListAsync();
        }

        public List<Alert> GetAlertByFilter(string AlertName, DateTime from, DateTime? To)
        {
            if (To == null)
            {
                To = DateTime.Now;
            }

            var filterAlerts = _shiftManagementDb.alerts.Where(x => x.AlertName == AlertName
            || x.CreatedDate >= from
            && x.CreatedDate <= To);
            //.Include('x => ((Derived)x).CreatedDate == from')
            //.Include(x => x.CreatedDate == To)
            //.ToList() ;

            return filterAlerts.ToList();
        }

        public List<Alert> GetAlertByFilter(DateTime from, DateTime To)
        {
            var alert = _shiftManagementDb.alerts
                .Where(a => a.TriggeredTime >= from && a.TriggeredTime < To);

            return alert.ToList();
        }

        public List<Alert> GetAllAlert(Guid ProjectId)
        {
            var alerts = _shiftManagementDb.alerts.Where(x => x.ProjectId == ProjectId).ToList();

            if (alerts == null)
            {
                return null;
            }
            return alerts;
        }

        public Task UpdateAlertStatus(Guid AlertId, UpdateAlertStatus UpdateAlertStatus)
        {
            var Existingalert = _shiftManagementDb.alerts.FirstOrDefault(x => x.Id == AlertId);
            var createdby = _shiftManagementDb.users.Where(x => x.id == Existingalert.ReportedBy).Select(x => x.username).FirstOrDefault();
            if (Existingalert == null)
            {
                return null;
            }
            Existingalert.AlertName = UpdateAlertStatus.AlertName;
            Existingalert.Description = UpdateAlertStatus.Description;
            Existingalert.Status = UpdateAlertStatus.Status;
            Existingalert.RCA = UpdateAlertStatus.RCA;
            Existingalert.TriggeredTime = UpdateAlertStatus.TriggeredTime;
            Existingalert.ReportedTo = UpdateAlertStatus.ReportedTo;
            Existingalert.ReportedToName = _shiftManagementDb.users.Where(x => x.id == UpdateAlertStatus.ReportedTo).Select(x => x.username).FirstOrDefault();
            Existingalert.lastModifiedDate = DateTime.Now;
            if (UpdateAlertStatus.Status == "Investigating")
            {
                var timeline = new TimelineUpdate
                {
                    Id = Guid.NewGuid(),
                    Alertid = AlertId,
                    createdby = Existingalert.ReportedBy,
                    Description = UpdateAlertStatus.Description,
                    datetime = DateTime.Now,
                    status = UpdateAlertStatus.Status,
                    keyvalue = "Description",
                    Action = "Status Updated",
                    createdbyname= createdby


                };
                _shiftManagementDb.TimelineUpdates.Add(timeline);
                _shiftManagementDb.SaveChanges();
            }
            else if (UpdateAlertStatus.Status == "Resolved")
            {
                var timeline = new TimelineUpdate
                {
                    Id = Guid.NewGuid(),
                    Alertid = AlertId,
                    createdby = Existingalert.ReportedBy,
                    Description = UpdateAlertStatus.Description,
                    datetime = DateTime.Now,
                    status = UpdateAlertStatus.Status,
                    keyvalue = "RCA",
                    Action = "Status Updated",
                    createdbyname = createdby

                };
                _shiftManagementDb.TimelineUpdates.Add(timeline);
                _shiftManagementDb.SaveChanges();
            }
            
            _shiftManagementDb.SaveChanges();
            return Task.CompletedTask;

        }

        public Task UpdateAlertStatus(Guid AlertId, AlertRequest UpdateAlertStatus)
        {
            var Existingalert = _shiftManagementDb.alerts.FirstOrDefault(x => x.Id == AlertId);
            if (Existingalert == null)
            {
                return null;
            }
            Existingalert.AlertName = UpdateAlertStatus.AlertName;
            Existingalert.TriggeredTime = UpdateAlertStatus.TriggeredTime;
            Existingalert.Description = UpdateAlertStatus.Description;
            Existingalert.Status = UpdateAlertStatus.Status;
            Existingalert.ReportedTo = UpdateAlertStatus.ReportedTo;
            _shiftManagementDb.SaveChanges();
            return Task.CompletedTask;
        }

        public List<TimelineUpdate> GetAlertUpdates(Guid AlertId)
        {
            var responce = _shiftManagementDb.TimelineUpdates.Where(x => x.Alertid == AlertId).ToList();
            if (responce == null)
            {
                return null;
            }
            return responce;
        }

        public TimelineUpdate PostAlertUpdates(Guid AlertId, TimelineUpdateModel updateModel)
        {
            var createdbyname = _shiftManagementDb.users.Where(x => x.id == updateModel.createdby).Select(x => x.username).FirstOrDefault();
            var timelineupdate = new TimelineUpdate
            {
                status = "Commented",
                Action = "Comment Added",
                datetime = DateTime.Now,
                Alertid = AlertId,
                Id = Guid.NewGuid(),
                Description = updateModel.Description,
                keyvalue = "Comment",
                createdby = updateModel.createdby,
                createdbyname = createdbyname,
            };
            _shiftManagementDb.TimelineUpdates.Add(timelineupdate);
            _shiftManagementDb.SaveChanges();
            return timelineupdate;

        }

        public Alert GetAlertById(Guid alertId)
        {
           var alert = _shiftManagementDb.alerts.Where(x => x.Id== alertId).FirstOrDefault();
            return alert== null ? null : alert;
        }
    }

}

