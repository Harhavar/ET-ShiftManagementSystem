using ET_ShiftManagementSystem.Models.Authmodel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class LoginRequestValidate : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidate()
        {
            RuleFor(x => x.username).NotEmpty();
            RuleFor(x => x.password).NotEmpty();

        }
    }
}
