using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeAppraisalReportView
    {
        public List<EmployeeAppraisalStatus> EmployeeAppraisalStatus { get; set; }
        public List<EmployeeAppraisalRatingStatus> EmployeeAppraisalRatingStatus { get; set; }
        public List<EmployeeAppraisalStatusView> EmployeeAppraisalStatusView { get; set; }
        public List<AppraisalMilestonedetails> AppraisalMilestonedetails { get; set; }
        public List<AppraisalBUHeadCommentsView> appraisalBUHeadCommentsViews { get; set; }
        public EmployeeCategoryView EmployeeCategoryView { get; set; }
    }
    public class EmployeeAppraisalStatusView
    {
        public int AppCycleId { get; set; }
        public int EmployeeId { get; set; }
        public string Employee_Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeType { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityShortName { get; set; }
        public int EmployeeRoleId { get; set; }
        public string EmployeeRoleName { get; set; }
        public int EmployeeDepId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeManagerId { get; set; }
        public decimal? EmployeeSelfRating { get; set; }
        public decimal? EmployeeAppraiserRating { get; set; }
        public decimal? EmployeeFinalRating { get; set; }
        public int? AppraisalStatus { get; set; }
        public string AppraisalStatusName { get; set; }
        public string ReportingTo { get; set; }
        public string ReportingEmail { get; set; }
        public bool? IsBUHeadRevert { get; set; }
        public bool? IsRevertRating { get; set; }
        public bool? IsBuheadApproved { get; set; }
    }
    public class EmployeeAppraisalStatus
    {
        public int? AppraisalStatusId { get; set; }
        public string AppraisalStatusName { get; set; }
        public int? AppraisalStatusCount { get; set; }
    }
    public class EmployeeAppraisalRatingStatus
    { 
        public decimal RatingValue { get; set; }
        public int RatingCount { get; set; }
    }
    public class AppraisalMilestonedetails
    { 
        public string MilestoneName { get; set; }
        public DateTime? MilestoneDate { get; set; }
        public bool MilestoneStaus { get; set; }

    }
    public class AppraisalBUHeadCommentsView
    {
        public int AppraisalBUHeadCommentsId { get; set; }
        public int AppCycle_Id { get; set; }
        public int Department_Id { get; set; }
        public int Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public string Comment { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_On { get; set; }
    }
    public class EmployeeCategoryView
    {
        public int EmployeeCategoryId { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeCategoryName { get; set; }
        public string Description { get; set; }
    }
    public class IndividualEmployeeAppraisalRating
    {
        public int Employee_Id { get; set; }
        public int? Current_AppcycleId { get; set; }
        public int? Previous_AppcycleId { get; set; }
        public decimal? Current_Rating { get; set; }
        public string CurrentAppcycle_monthyear { get; set; }
        public decimal? Previous_Rating { get; set; }
        public string PreviousAppcycle_monthyear { get; set; }
    }
}

