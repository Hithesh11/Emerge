namespace Emerge.Models
{
    public class LeaveRequest
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int leaveTypeId { get; set; }
        public DateTime dateRequested { get; set; }
        public string requestComments { get; set; }
        public bool approved { get; set; }
        public bool cancelled { get; set; }
        public string requestingEmployeeId { get; set; }
    }
}
