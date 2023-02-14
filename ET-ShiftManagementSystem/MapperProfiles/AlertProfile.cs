using AutoMapper;
using ET_ShiftManagementSystem.Models.AlertModel;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class AlertProfile : Profile
    {
        public AlertProfile()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.Alert,AlertsDTO>().ReverseMap();
        }
    }
}
