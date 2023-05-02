using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class TotalAvailableView
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public decimal? TotalLeaves { get; set; }
        public List<AppliedLeavesView> AppliedLeavesView { get; set; }
    }
    public class AppliedLeavesView
    {
        public int LeaveTypeId { get; set; }
        public int EmployeeId { get; set; }
        public decimal? NoOfLeavesTaken { get; set; }
    }
    public class AvailableLeaveDetailsView
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public decimal? AvailableLeaves { get; set; }
        public decimal? BookedLeaves { get; set; }
        public decimal? NoOfAbsentDays { get; set; }
        public List<DateTime> AbsentDatesList { get; set; }
         public LeaveRestrictionsViewDetails LeaveRestrictionsViewDetails { get; set; }
    }
    public class AvailableLeaveAndDurationDetailsView
    {
        public List<AvailableLeaveDetailsView> AvailableLeaveDetailsView { get; set; }
        public List<LeaveDurationListView> LeaveDurationListView { get; set; }
    }
}
