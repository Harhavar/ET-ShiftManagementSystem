using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class DocsProfile : Profile
    {
        public DocsProfile()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.Doc, Models.DocDTO>().ReverseMap();
        }
    }
}
