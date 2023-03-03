using System.Security.Policy;

namespace ET_ShiftManagementSystem.Entities
{
    public class TaskComment
    {
        public Guid Id { get; set; }

        
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        //navigation property 
        public Guid TaskID { get; set; }


        public Tasks Task { get; set; }

    }
}
