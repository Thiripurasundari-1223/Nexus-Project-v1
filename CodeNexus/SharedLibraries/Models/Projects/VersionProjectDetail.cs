using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class VersionProjectDetail
    {
        [Key]
        public int VersionId { get; set; }
        public int ProjectId { get; set; }
        public int? AccountId { get; set; }
        public string VersionName { get; set; }
        public string ProjectLogo { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? FrequencyId { get; set; }
        public string FrequencyValue { get; set; }
        public int? CurrencyTypeId { get; set; }
        public decimal? TotalSOWAmount { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string ProjectDescription { get; set; }
        public decimal? ProjectDuration { get; set; }
        public string ProjectStatus { get; set; }   
        public int? NumberOfIteration { get; set; }
        public string ProjectStatusCode { get; set; }
        public string ProjectChanges { get; set; }
        public int? bUAccountableForProject { get; set; }
        public string FormattedProjectId { get; set; }
        public int? FinanceManagerId { get; set; }
        public int? EngineeringLeadId { get; set; }
        public string EmployeeLeavesRetained { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDraft { get; set; }

    }
}
