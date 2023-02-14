using ET_ShiftManagementSystem.Models.CommentModel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class AddCommmentsValidator : AbstractValidator<AddComments>
    {
        public AddCommmentsValidator()
         {
            RuleFor(s => s.CommentText).NotEmpty();
            RuleFor(s => s.CreatedDate).NotEmpty();
            RuleFor(s => s.Shared).NotEmpty();
            RuleFor(s => s.ShiftID).NotEmpty();
            //RuleFor(s => s.UserID).NotEmpty();
           // RuleFor(s => s.CommentID).NotEmpty();

        }
    }
}
