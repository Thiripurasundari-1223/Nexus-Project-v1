using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class ApplyLeavesView
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public decimal NoOfDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public int? LeaveRejectionReasonId { get; set; }
        public bool IsActive { get; set; }
        public string LeaveType { get; set; }
        public List<AppliedLeaveDetailsView> AppliedLeaveDetails { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TotalHours { get; set; }
        public string ShiftHour { get; set; }
        public int AttendaceId { get; set; }
        public bool IsFirstHalfAbsent { get; set; }
        public bool IsSecondHalfAbsent { get; set; }
        public bool? IsRegularize { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int AttendanceDetailsId { get; set; }
        public List<WeeklyMonthlyAttendanceDetail> WeeklyMonthlyAttendanceDetail { get; set; }
        public bool? IsGrantLeave { get; set; }
        public List<GrantLeaveApprovalView> GrantLeaveApprovalStatus{ get; set; }
        public int? LeaveGrantDetailId { get; set; }
        public string Lop { get; set; }
        public string GrantEffectiveFromDate { get; set; }
        public List<WeekendViewDefinition> WeekendList { get; set; }
        public int ShiftId { get; set; }
        public int DepartmentId { get; set; }
        public int LocationId { get; set; }
        public int ApproverManagerId { get; set; }
        public DateTime? RelivingDate { get; set; }
        public string ApproveRejectName { get; set; }
        public DateTime? ApproveRejectDate { get; set; }
        public bool? isFullDay { get; set; }
        public bool? isFirstHalf { get; set; }
        public bool? isSecondHalf { get; set; }
    }
}
