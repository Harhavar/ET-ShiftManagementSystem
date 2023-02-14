using ET_ShiftManagementSystem.Models.Authmodel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor( x => x.Password ).NotEmpty();
            RuleFor( x => x.ConfirmPassword ).NotEmpty();
           // RuleFor( x => x.Email ).NotEmpty();
            //RuleFor( x => x.UserId ).NotEmpty();

        }
    }
}
