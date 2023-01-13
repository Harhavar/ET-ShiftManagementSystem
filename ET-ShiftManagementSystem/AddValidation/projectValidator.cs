using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class projectValidator : AbstractValidator<Entities.Project>
    {
        public projectValidator()
        {
            RuleFor(x => x.ProjectName).NotEmpty();
            RuleFor(x => x.ClientName).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
            RuleFor(x => x.CreatedDate).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
}
