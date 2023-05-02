using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class AppliedLeaveView
    {
        public int LeaveId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal NoOfDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string LeaveType { get; set; }
        public List<AppliedLeaveDetailsView> AppliedLeaveDetails { get; set; }
    }
}
