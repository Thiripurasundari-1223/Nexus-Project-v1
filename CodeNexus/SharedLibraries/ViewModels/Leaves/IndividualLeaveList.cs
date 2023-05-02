using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class IndividualLeaveList
    {
        //public List<AppliedLeaveView> applyLeavesView { get; set; }
        //public List<LeaveTypesView> leaveTypesView { get; set; }
        //public List<AvailableLeaveDetailsView> AvailableLeaveDetails { get; set; }
        public List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveDetails { get; set; }
        public List<ApplyLeavesView> AppliedLeaveList { get; set; }
        public List<Holiday> HolidayList { get; set; }
        public EmployeePersonalDetails employeePersonalDetails { get; set; }
        public EmployeeShiftDetailsListView EmployeeShiftDetailsList { get; set; }
        public HolidayDetailsView HolidayDetails { get; set; }
        public List<AppliedLeaveTypeDetails> AppliedLeaveDetails { get; set; }
        public DateTime? FromDate { get; set; }
        public int? TotalCount { get; set; }

    }
    public class EmployeeShiftDetailsListView
    {
        public List<EmployeeShiftDetailsView> employeeShifts { get; set; }
        public EmployeeShiftDetailsView DefaultShiftView { get; set; }
    }
}
