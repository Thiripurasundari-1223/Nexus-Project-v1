using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveHolidayView
    {
        public List<ApplyLeaves> ApplyLeaves { get; set; }
        public List<Holiday> Holiday { get; set; }
        public LeaveBalanceList leaveBalance { get; set; }
        public List<AppliedLeaveTypeDetails> appliedLeaveDetails { get; set; }
        public HolidayDetailsView holidayDetails { get; set; }
    }
}
