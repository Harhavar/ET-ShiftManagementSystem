using ET_ShiftManagementSystem.Models.ShiftModel;

namespace ET_ShiftManagementSystem.Models.ProjectsModel
{
    public class AddProjectRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserShiftModel> UserShift { get; set; }

    }
}
