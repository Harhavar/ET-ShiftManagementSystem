using AutoMapper;
using ET_ShiftManagementSystem.Models.TaskModel;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.TaskDetail, TaskDTO>().ReverseMap();
        }
    }
}
