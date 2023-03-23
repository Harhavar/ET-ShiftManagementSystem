using CloudinaryDotNet.Actions;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.AlertModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IAlertServices
    {
           List<Alert> GetTodaysAlert();
        Task<IEnumerable<Alert>> GetAllAlert();
        List<Alert> GetAllAlertBySeveority(severityLevel severity);

        List<Alert> GetAlertByFilter(string AlertName, DateTime from, DateTime? To);
        List<Alert> GetAlertByFilter( DateTime from, DateTime To);

        Task AddAlert(Guid ProjectId , AlertRequest alertRequest , severityLevel sevearity );

        Task<Alert> DeleteAlertAsync(Guid id);

    }
    public class AlertServices : IAlertServices
    {
        private readonly ShiftManagementDbContext _shiftManagementDb;

        public AlertServices(ShiftManagementDbContext shiftManagementDb)
        {
            _shiftManagementDb = shiftManagementDb;
        }

        public async Task AddAlert(Guid ProjectId, AlertRequest alertRequest, severityLevel sevearity)
        {
            //if (sevearity < 0 && sevearity => 2)
            var alert = new Alert
            {
                Id = Guid.NewGuid(),
                AlertName = alertRequest.AlertName,
                //Id = alert.Id,
                //RCA = alertRequest.RCA,
                Description = alertRequest.Description,
                //ReportedBy = alertRequest.ReportedBy,
                ReportedTo = alertRequest.ReportedTo,
                TriggeredTime = alertRequest.TriggeredTime,
                CreatedDate = DateTime.Now,
                severity = sevearity,
                Status = alertRequest.Status,
                lastModifiedDate = DateTime.Now,
                RCA = "",
                ReportedBy="",
                ProjectId= ProjectId,
                TenantId= _shiftManagementDb.Projects.Where(x => x.ProjectId == ProjectId).Select(x=> x.TenentId).FirstOrDefault(),


            };

             _shiftManagementDb.alerts.Add(alert);
            await _shiftManagementDb.SaveChangesAsync();
           
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
            var alerts =  _shiftManagementDb.alerts.Where(x => x.severity == severity).ToList();

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
    }
}
