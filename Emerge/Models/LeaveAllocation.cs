namespace Emerge.Models
{
    public class LeaveAllocation
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public int EmployeeId { get; set; }
        public int NumberOfDays { get; set; }
    }
}
