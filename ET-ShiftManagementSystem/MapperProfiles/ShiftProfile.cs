using AutoMapper;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class ShiftProfile : Profile
    {
        public ShiftProfile()
        {
            CreateMap<Shift, Models.ShiftDTO>().ReverseMap();
        }
    }
}
