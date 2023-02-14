using ET_ShiftManagementSystem.Models.UserModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class AuthValidator : AbstractValidator<UserDto>
    {
        public AuthValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.username).NotEmpty();
            RuleFor(x => x.password).NotEmpty();
            RuleFor(x => x.ContactNumber).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
