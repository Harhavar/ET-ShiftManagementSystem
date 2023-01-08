using AutoMapper;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftManagementSystem.MapperProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, Models.CommentDTO>().ReverseMap();
        }
    }
}
