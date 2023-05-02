using System;

namespace SharedLibraries.ViewModels
{
    public class ProjectListView
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string AccountName { get; set; }
        public int? AccountId { get; set; }
        public int SPOCId { get; set; }
        public string SPOC { get; set; }
        public string Timesheets { get; set; }
        public string Associates { get; set; }
        public string ProjectStatus { get; set; }
        public string UserRole { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public int? CreatedBy { get; set; }
        public decimal? ProjectDuration { get; set; }
        public int? ResourceCount { get; set; }
        public string ProjectStatusCode { get; set; }
        public string ProjectStatusName { get; set; }
        public string ProjectChanges { get; set; }
        public string Logo { get; set; }
        public string LogoBase64 { get; set; }
    }
    public class ApproveOrRejectProject
    {
        public int ProjectId { get; set; }
        public string ProjectStatus { get; set; }
        public string Comments { get; set; }
        public int DepartmentHeadId { get; set; }
        
        public int BUManagerId { get; set; }
        public int ProjectManagerOfficerId { get; set; }
        
    }
    public class ProjectManager
    {
        public int ProjectManagerId { get; set; }
        public string ProjectManagerName { get; set; }
        public string UserRole { get; set; }
    }
}