namespace ET_ShiftManagementSystem.Models.ShiftModel
{
    public class UpdateShiftRequest
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
