using System.ComponentModel.DataAnnotations;

namespace Timesheet.DAL.Models
{
    public class ResourceAllocation
    {
        [Key]
        public int ResourceAllocationId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string SkillSet { get; set; }
        public decimal? PlannedHours { get; set; }
        public decimal? Contribution { get; set; }
    }
}