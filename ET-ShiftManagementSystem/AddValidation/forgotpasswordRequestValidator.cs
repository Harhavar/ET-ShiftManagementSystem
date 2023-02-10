using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class forgotpasswordRequestValidator : AbstractValidator<Models.forgotPasswordRequest>

    {
        public forgotpasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
