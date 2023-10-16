using System.ComponentModel.DataAnnotations;

namespace Emerge.Models
{
    public class LeaveInfo
    {
        public int Id { get; set; }

        [MaxLength(2)]
        public string Type { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public int? NoOfDays { get; set; }

        public int? MaxSlot { get; set; }

        [MaxLength(1)]
        public string Gender { get; set; }

        public int? StartValidityDays { get; set; }

        public int? CarryForwardDays { get; set; }

        [MaxLength(50)]
        public string Designation { get; set; }
    }

}
