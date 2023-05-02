using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class ResourceAllocationList
    {
        [Key]
        public int ResourceAllocationId { get; set; }
        public int? IterationID { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public int? ChangeRequestId { get; set; }
        public int? RequiredSkillSetId { get; set; }
        public decimal? SkillRate { get; set; }
        public int? FrequencyId { get; set; }
        public int? AllocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PlannedHours { get; set; }
        public decimal? Contribution { get; set; }
        public bool? IsBillable { get; set; }
        public bool? IsSPOC { get; set; }
        public bool? IsAdditionalResource { get; set; }
        public bool? IsActive { get; set; }
        public decimal? Experience { get; set; }
        public int? ProjectRoleID { get; set; }
        public int? DeliverySupervisorId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

        public string EmployeeName { get; set; }
        public string ProjectName { get; set; }
        public string RequiredSkillSet { get; set; }
        public int? RateFrequencyId { get; set; }
        public string RateFrequency { get; set; }
        public string Allocation { get; set; }
    }
}