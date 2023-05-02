using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class ProjectDetailView
    {
        [Key]
        public int ProjectId { get; set; }
        public int? AccountId { get; set; }
        public string Logo { get; set; }
        public string ProjectName { get; set; }
        public int ProjectTypeId { get; set; }
        public string FormattedProjectId { get; set; }
        public int? FrequencyId {get;set; }
        public string FrequencyValue { get; set; }
        public int CurrencyTypeId { get; set; }
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
        public int? FinanceManagerId { get; set; }
        public int? EngineeringLeadId { get; set; }
        public int? AppConstantId { get; set; }
        public string EmployeeLeavesRetained { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDraft { get; set; }
        
        public List<CustomerSPOCDetailsList> CustomerSPOCDetails { get; set; } 
        public List<FixedIterationList> FixedIteration { get; set; }
        public List<ProjectDocumentList> ProjectDocuments { get; set; } 
        public List<ResourceAllocationList> ResourceAllocation { get; set; }
        public List<ProjectDetailCommentsList> ProjectDetailComments { get; set; }

        public string Documents { get; set; }        
        public string ProjectType { get; set; }
        public int? ProjectSPOC { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectManagerRole { get; set; }
        public string ProjectManagerEmpId { get; set; }
        public string CurrencyType { get; set; }
        public string SkillSet { get; set; }
        public string Experience { get; set; }
        public string SkillRate { get; set; }       
        public string AccountName { get; set; }
        public string AdditionalComments { get; set; }
        public string CreatedByName { get; set; }
        public string UserRole { get; set; }
        public string ProjectStatusName { get; set; }
        public List<ChangeRequestView> ChangeRequestList { get; set; }
        public List<DocumentDetails> ListOfDocuments { get; set; }
        public string BUHeadNameForProject { get; set; }        
        public string LogoBase64 { get; set; }
        public string ProjectTimesheetStatus { get; set; }
        public string RateFrequency { get; set; }
    }
}