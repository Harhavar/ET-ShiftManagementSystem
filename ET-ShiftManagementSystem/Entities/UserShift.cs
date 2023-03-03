namespace ET_ShiftManagementSystem.Entities
{
    public class UserShift
    {
        public Guid ID { get; set; }

        public Guid ShiftId { get; set; }

        public Guid UserId { get; set; }

        public Guid ProjectId { get; set; }
    }

    public class addShift
    {
        public Guid ShiftId { get; set; }

        public Guid UserId { get; set; }


    }
}
