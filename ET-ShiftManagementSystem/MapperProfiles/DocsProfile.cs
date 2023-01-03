using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class DocsProfile : Profile
    {
        public DocsProfile()
        {
            CreateMap<ShiftMgtDbContext.Entities.Doc, Models.DocDTO>().ReverseMap();
        }
    }
}
