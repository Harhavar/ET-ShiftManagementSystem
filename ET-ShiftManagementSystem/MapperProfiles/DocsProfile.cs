using AutoMapper;
using ET_ShiftManagementSystem.Models.DocModel;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class DocsProfile : Profile
    {
        public DocsProfile()
        {
            CreateMap<ET_ShiftManagementSystem.Entities.Doc, DocDTO>().ReverseMap();
        }
    }
}
