namespace ET_ShiftManagementSystem.Models
{
    public class SuscriptionDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int SubscriptionLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentId { get; set; }
        public int Status { get; set; }
    }
}
