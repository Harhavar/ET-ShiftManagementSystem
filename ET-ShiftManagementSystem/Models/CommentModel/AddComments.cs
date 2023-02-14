namespace ET_ShiftManagementSystem.Models.CommentModel
{
    public class AddComments
    {
        //public int CommentID { get; set; }

        public string CommentText { get; set; }

        public int ShiftID { get; set; }

        // public int UserID { get; set; }

        public bool Shared { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
