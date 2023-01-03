using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<ShiftMgtDbContext.Entities.TaskDetail, Models.TaskDTO>().ReverseMap();
        }
    }
}
