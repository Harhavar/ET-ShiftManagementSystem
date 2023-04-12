using ET_ShiftManagementSystem.Models.ShiftModel;
using FluentValidation;

namespace ET_ShiftManagementSystem.AddValidation
{
    public class updateShiftRequestValidator : AbstractValidator<UpdateShiftRequest>
    {
        public updateShiftRequestValidator()
        {
                //RuleFor( s => s.ShiftID ).NotEmpty();
                RuleFor( s => s.StartTime ).NotEmpty();
                RuleFor( s => s.EndTime ).NotEmpty();
                RuleFor( s => s.ShiftName ).NotEmpty();

        }
    }
}
