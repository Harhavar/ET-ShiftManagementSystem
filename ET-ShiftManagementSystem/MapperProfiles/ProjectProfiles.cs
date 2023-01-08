using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class ProjectProfiles : Profile
    {
        public ProjectProfiles()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.Project , Models.ProjectDto>().ReverseMap();
        }
    }
}
