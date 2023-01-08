using AutoMapper;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class ProjectDetails : Profile
    {
        public ProjectDetails() 
        {
            CreateMap<ProjectDetail, Models.ProjectDetailsDTO>().ReverseMap();
        }
    }
}
