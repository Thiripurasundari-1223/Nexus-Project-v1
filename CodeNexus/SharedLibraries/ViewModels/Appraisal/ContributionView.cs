using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class WorkdayListView
    {
        public int WorkDayId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime WorkDate { get; set; }
        public List<WorkdayObjectiveView> ObjectiveDetail { get; set; }

    }
    public class WorkdayKRADetailView
    {
        public int WorkDayDetailId { get; set; }
        public int WorkDayId { get; set; }
        public int? ObjectiveId { get; set; }
        public int? KRAId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string WorkHours { get; set; }
        public string EmployeeRemark { get; set; }
        public string Status { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRemark { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class WorkdayObjectiveView
    {
        public int? ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public List<WorkdayKRAView> KRADetail { get; set; }
    }
    public class WorkdayKRAView
    {
        public int? KRAId { get; set; }
        public string KRAName { get; set; }
        public List<WorkdayKRADetailView> ContributionDetail { get; set; }
    }
    public class ApproveOrRejectWorkdayDetailView
    {
        public int[] WorkDayDetailId { get; set; }
        public int WorkDayId { get; set; }
        public string Status { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRemark { get; set; }
    }
    public class ApproveOrRejectWorkdayListView
    {
        public int[] WorkDayIds { get; set; }
        public string Status { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRemark { get; set; }
    }
    public class WorkdayInputView
    {
        public int WorkDayId { get; set; }
        public int WorkDayDetailId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime WorkDate { get; set; }
        public int? ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public int? KRAId { get; set; }
        public string KRAName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string WorkHours { get; set; }
        public string EmployeeRemark { get; set; }
        public string Status { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRemark { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}