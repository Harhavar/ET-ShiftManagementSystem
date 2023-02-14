using ET_ShiftManagementSystem.Models.Authmodel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class loginrequestValidator : AbstractValidator<LoginRequest>
    {
        public loginrequestValidator()
        {
            RuleFor(x => x.username ).NotEmpty();
            RuleFor(x => x.password ).NotEmpty();

        }
    }
}
