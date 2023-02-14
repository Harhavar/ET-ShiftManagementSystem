using ET_ShiftManagementSystem.Models.Authmodel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class forgotpasswordRequestValidator : AbstractValidator<forgotPasswordRequest>

    {
        public forgotpasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
