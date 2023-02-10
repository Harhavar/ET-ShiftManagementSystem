using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.TaskDetail, Models.TaskDTO>().ReverseMap();
        }
    }
}
