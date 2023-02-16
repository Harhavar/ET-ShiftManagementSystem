using AutoMapper;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<Entities.Permission , Models.PermissionModel.PermissionDTO>().ReverseMap();
        }
    }
}
