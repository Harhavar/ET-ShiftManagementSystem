using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class SREprofile : Profile
    {
        public SREprofile()
        {
                CreateMap<ET_ShiftManagementSystem.Entities.SREDetile ,ET_ShiftManagementSystem.Models.SREdetailsDTO>().ReverseMap();
        }
    }
}
