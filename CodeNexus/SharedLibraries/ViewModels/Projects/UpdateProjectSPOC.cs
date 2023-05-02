using System;

namespace SharedLibraries.ViewModels
{ 
    public class UpdateProjectSPOC
    {
        public int ResourceAllocationId { get; set; }
        public int ProjectId { get; set; }
        public int ModifiedBy { get; set; }
        public int ProjectSpoc { get; set; }
        public bool IsBillable { get; set; }
        public int? AllocationId { get; set; }
        public int? RequiredSkillSetId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }        
    }
    public class UpdateResourceAllocation
    {
        public int ProjectId { get; set; }
        public int ResourceAllocationId { get; set; }
        public int EmployeeId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsBillable { get; set; }
        public decimal? Experience { get; set; }
        public int? RequiredSkillSetId { get; set; }
        public decimal? SkillRate { get; set; }
        public int? RateFrequencyId { get; set; }
        public int? AllocationId { get; set; }
        public bool? IsAdditionalResource { get; set; }
    }
    public class UpdateProjectLogo
    {
        public int ProjectId { get; set; }
        public int ModifiedBy { get; set; }
        public string Logo { get; set; }
    }
    public class RemoveProjectLogo
    {
        public int ProjectId { get; set; }
    }
}