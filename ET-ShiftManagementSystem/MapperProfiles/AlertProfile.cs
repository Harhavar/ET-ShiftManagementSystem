using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class AlertProfile : Profile
    {
        public AlertProfile()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.Alert,Models.AlertsDTO>().ReverseMap();
        }
    }
}
