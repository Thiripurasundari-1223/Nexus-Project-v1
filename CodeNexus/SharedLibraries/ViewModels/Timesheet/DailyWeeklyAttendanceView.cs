using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class WeeklyMonthlyAttendance
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public bool IsCheckin { get; set; }
        public string ShiftHour { get; set; }
        public bool IsLeave { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsAbsent { get; set; }
        public string Remark { get; set; }
        public bool IsWeekend { get; set; }
        public bool? IsFirstHalf { get; set; }
        public bool? IsSecondHalf { get; set; }
        public bool? IsFullDay { get; set; }
        public bool? AppliedLeaveStatus { get; set; }
        public string LeaveType { get; set; }
        public string HolidayName { get; set; }
        public bool? IsFirstHalfAbsent { get; set; }
        public bool? IsSecondHalfAbsent { get; set; }
        public string LeaveTypeName { get; set; }

        public List<WeeklyMonthlyAttendanceDetail> WeeklyMonthlyAttendanceDetail { get; set; }
        public List<EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
        public string ShiftFromTime { get; set; }
        public string ShiftToTime { get; set; }
        public string FirstHalfRemark { get; set; }
        public string SecondHalfRemark { get; set; }
        public string FirstHalfLeaveType { get; set; }
        public string SecondHalfLeaveType { get; set; }
        public List<WeeklyMonthlyAttendanceDetail> WeeklyMonthlyRegularizationDetail { get; set; }
        public string TotalPendingRegularizedHours { get; set; }
        public bool IsFlexyShift { get; set; }
        public bool IsActiveShift { get; set; }
        public string LeaveStatus { get; set; }
        public string FirstHalfLeaveStatus { get; set; }
        public string SecondHalfLeaveStatus { get; set; }
        public string LeaveReason { get; set; }
        public decimal NoOfDays { get; set; }
        public int LeaveId { get; set; }
        public List<AppliedLeaveTypeDetails> LeaveDetails { get; set; }
    }
    public class WeeklyMonthlyAttendanceDetail
    {
        public int? AttendanceDetailId { get; set; }
        public int AttendanceId { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public DateTime? BreakinTime { get; set; }
        public DateTime? BreakoutTime { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public bool? IsRegularize { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Reason { get; set; }
        public string LeaveType { get; set; }
        public string Feedback { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? RegularizationDate { get; set; }
    }
}
