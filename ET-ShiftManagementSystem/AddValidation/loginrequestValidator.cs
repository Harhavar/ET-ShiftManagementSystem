using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class loginrequestValidator : AbstractValidator<Models.LoginRequest>
    {
        public loginrequestValidator()
        {
            RuleFor(x => x.username ).NotEmpty();
            RuleFor(x => x.password ).NotEmpty();

        }
    }
}
