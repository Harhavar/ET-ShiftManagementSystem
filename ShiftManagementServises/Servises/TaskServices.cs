using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using ShiftMgtDbContext.Data;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftManagementServises.Servises
{
    public interface ITaskServices
    {
        Task<IEnumerable<TaskDetail>> GetTasks();

        long AddTask(TaskDetail task);

        public Task<TaskDetail> DeleteTask(int id);

    }
    public class TaskServices : ITaskServices
    {
        private readonly ShiftManagementDbContext _shiftManagement;

        public TaskServices(ShiftManagementDbContext shiftManagement)
        {
            _shiftManagement = shiftManagement;
        }

        public long AddTask(TaskDetail task)
        {
            _shiftManagement.taskDetails.Add(task);
            _shiftManagement.SaveChanges();
            return task.Id;
        }

        public async Task<TaskDetail> DeleteTask(int id)
        {
            var DLT = await _shiftManagement.taskDetails.FirstOrDefaultAsync(x => x.Id == id);

            if (DLT == null)
            {
                return null;
            }
            _shiftManagement.taskDetails.Remove(DLT);
            await _shiftManagement.SaveChangesAsync();

            return DLT;

        }

        public async  Task<IEnumerable<TaskDetail>> GetTasks()
        {
            return await _shiftManagement.taskDetails.ToListAsync();

        }
    }
}
