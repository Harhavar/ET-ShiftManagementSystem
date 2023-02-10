using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class TenateProfile : Profile
    {
        public TenateProfile() 
        { 
            CreateMap<Entities.Tenate, Models.TenateDTO>().ReverseMap();
        }

    }
}
