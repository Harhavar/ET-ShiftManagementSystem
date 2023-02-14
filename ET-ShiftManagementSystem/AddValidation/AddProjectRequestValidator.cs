using ET_ShiftManagementSystem.Models.ProjectModel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class AddProjectRequestValidator : AbstractValidator<AddProjectRequest>
    {
        public AddProjectRequestValidator()
        {
            RuleFor(s => s.ProjectName).NotEmpty();
            RuleFor(s => s.Description).NotEmpty();
            RuleFor(s => s.IsActive).NotEmpty();
            RuleFor(s => s.ClientName).NotEmpty();
            RuleFor(s => s.CreatedBy).NotEmpty();
            RuleFor(s => s.ModifieBy).NotEmpty();

        }
    }
}
