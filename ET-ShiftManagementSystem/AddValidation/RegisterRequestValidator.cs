using ET_ShiftManagementSystem.Models.Authmodel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
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
