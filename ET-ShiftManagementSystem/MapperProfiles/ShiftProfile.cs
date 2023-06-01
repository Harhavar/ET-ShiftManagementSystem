using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.ShiftModel;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class ShiftProfile : Profile
    {
        public ShiftProfile()
        {
            CreateMap<Entities.AddShiftRequest, ShiftDTO>().ReverseMap();
        }
    }
}
