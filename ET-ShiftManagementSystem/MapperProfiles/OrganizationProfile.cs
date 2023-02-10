using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Entities.Organization,Models.organizationModels.Organization>().ReverseMap();
        }
    }
}
