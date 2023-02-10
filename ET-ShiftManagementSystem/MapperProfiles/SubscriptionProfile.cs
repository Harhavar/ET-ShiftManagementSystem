using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Entities.Subscription , Models.SuscriptionDTO>().ReverseMap();
        }
    }
}
