using CloudinaryDotNet.Actions;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;

namespace ET_ShiftManagementSystem.Servises
{
    public interface IAlertServices
    {
        Task<IEnumerable<Alert>> GetAlertAsync();
        Task<IEnumerable<Alert>> GetAllAlert();

        Task<IEnumerable<Alert>> GetAlertByFilter(string AlertName, DateTime from, DateTime? To);

        long AddAlert(Alert alert);

        Task<Alert> DeleteAlertAsync(int id);


    }
    public class AlertServices : IAlertServices
    {
        private readonly ShiftManagementDbContext _shiftManagementDb;

        public AlertServices(ShiftManagementDbContext shiftManagementDb)
        {
            _shiftManagementDb = shiftManagementDb;
        }

        public long AddAlert(Alert alert)
        {

            _shiftManagementDb.alerts.Add(alert);
            _shiftManagementDb.SaveChanges();
            return alert.Id;
        }

        public async Task<Alert> DeleteAlertAsync(int id)
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

        public async Task<IEnumerable<Alert>> GetAlertAsync()
        {
            var TodaysAlert = _shiftManagementDb.alerts.Where(x => x.CreatedDate.Date == DateTime.Today);
            return TodaysAlert.ToList();
            //await _shiftManagementDb.alerts.ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetAlertByFilter(string AlertName, DateTime from, DateTime? To)
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
    }
}
