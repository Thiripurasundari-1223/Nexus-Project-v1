using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeListAndDepartment
    {
        public List<int> employeeids { get; set; }
        public int departmentId { get; set; }
        public bool IsAllReportees { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class EmployeeAppraisalListView
    {
        public List<EmployeeAppraisalMasterDetailView> EmployeeAppraisalMasterDetailView { get; set; }
        public List<AppraisalMilestonedetails> AppraisalMilestonedetails { get; set; }
        public List<AppraisalBUHeadCommentsView> AppraisalBUHeadCommentsView { get; set; }
    }
    public class EmployeeAppraisalMasterView
    {
        public int Employee_Dept_Id { get; set; }
        public int? Appraisal_Status_Id { get; set; }
        public string Apraisal_Status { get; set; }
        public int EmployeeCount { get; set; }
        public decimal Appraisal_Percentage { get; set; }
    }
    public class TeamRatingSummary
    {
        public int Employee_Dept_Id { get; set; }
        public decimal? Employee_Rating { get; set; }
        public int Employee_Count { get; set; }
    }
    public class AppraisalStatusGridview
    {
        public int Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeEmailId { get; set; }
        public int Manager_Id { get; set; }
        public string Manager_Name { get; set; }
        public int Department_Id { get; set; } 
        public string Department_Name { get; set; }
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }
        public decimal? Rating { get; set; }
        public int? AppraisalStatusId { get; set; }
        public string AppraisalStatus { get; set; }
        public string ReportingTo { get; set; }
        public string ReportingEmailId { get; set; }
        public int AppCycleID { get; set; }
        public bool? IsBUHeadRevert { get; set; }
        public bool? IsRevertRating { get; set; }
        public bool? IsBuheadApproved { get; set; }
    }
    public class AppraisalStatusReport
    {
       public List<EmployeeAppraisalMasterView> AppraisalStatus { get; set; }
       public List<TeamRatingSummary> TeamRatingSummary { get; set; }
       public List<AppraisalStatusGridview> appraisalStatusGridviews { get; set; }
        public List<DepartmentDetails> DepartmentDetails { get; set; }
    }
    public class AppraisalStatusReportCount
    {
        public string AppraisalCycle { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
    }
    public class DepartmentDetails
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
