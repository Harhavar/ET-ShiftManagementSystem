using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class ResetPasswordViewModelValidator : AbstractValidator<Models.ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor( x => x.Password ).NotEmpty();
            RuleFor( x => x.ConfirmPassword ).NotEmpty();
            RuleFor( x => x.Email ).NotEmpty();
            //RuleFor( x => x.UserId ).NotEmpty();

        }
    }
}
