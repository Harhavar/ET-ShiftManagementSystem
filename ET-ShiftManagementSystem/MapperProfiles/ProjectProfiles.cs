using AutoMapper;
using ET_ShiftManagementSystem.Models.ProjectModel;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class ProjectProfiles : Profile
    {
        public ProjectProfiles()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.Project , ProjectDto>().ReverseMap();
        }
    }
}
