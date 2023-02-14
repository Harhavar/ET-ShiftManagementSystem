using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.ProjectModel;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class ProjectDetails : Profile
    {
        public ProjectDetails() 
        {
            CreateMap<ProjectDetail, ProjectDetailsDTO>().ReverseMap();
        }
    }
}
