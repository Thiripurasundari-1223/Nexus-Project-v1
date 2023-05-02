using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class TeamLeaveView
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public decimal NoOfDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public string LeaveType { get; set; }
        public bool? IsRegularize { get; set; }
        public int AttendanceDetailsId { get; set; }
        public List<SupportingDocuments> ListOfDocuments { get; set; }
        public List<AppliedLeaveDetailsView> AppliedLeaveDetails { get; set; }
        public int? LeaveGrantDetailId { get; set; }
        public bool? IsGrantLeaveRequest { get; set; }
        public int? LevelId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string EmployeeFormattedId { get; set; }
        public string ManagerFormattedId { get; set; }
    }
    public class ApproveOrRejectLeave
    {
        public string LeaveTypeName { get; set; }
        public string ApproverName { get; set; }
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public int LeaveRejectionReasonId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsRegularize { get; set; }
        public List<AppliedLeaveApproveOrReject> AppliedLeaveApproveOrReject { get; set; }
        public TimeLogApproveOrRejectView TimeLog { get; set; }
        public bool? IsGrantLeaveRequest { get; set; }
        public int? LeaveGrantDetailId { get; set; }
        public int? LevelId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
       public decimal NoOfDays { get; set; }
        public int ApproverManagerId { get; set; }
        public string Reason { get; set; }
        public string ApproveRejectName { get; set; }
    }
    public class AppliedLeaveApproveOrReject
    {
        public int AppliedLeaveDetailsID { get; set; }
        public bool? AppliedLeaveStatus { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsFullDay { get; set; }
        public bool IsFirstHalf { get; set; }
        public bool IsSecondHalf { get; set; }
        public decimal? NoOfDays { get; set; }
        public DateTime? LeaveDate { get; set; }
        
    }
    public class MailLeaveList
    {
        public string? LeaveDate { get; set; }
        public string? LeaveDuration { get; set; }
        public string? LeaveStatus { get; set; }
    }

    public class StatusandApproverDetails
    {
        public string Status { get; set; }
        public int? ApproverID { get; set; }
        public int? ApproverManagerId { get; set; }
        public int LevelId { get; set; }
        public int LeaveId { get; set; }
        public int? LeaveGrantDetailId { get; set; }
    }
    
}
