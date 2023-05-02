using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class WeekMonthAttendanceView
    {
        public int EmployeeId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        //public bool IsMonth { get; set; }
        public int TotalDays { get; set; }
        public int DepartmentId { get; set; }
        public int? ShiftDetailsId { get; set; }
        public int LocationId { get; set; }
        public DateTime ShiftDate { get; set; }
        public List<EmployeeShiftDetailsView> EmployeeShiftDetailsView { get; set; }
        public int EmployeeCategoryId { get; set; }
        public DateTime DOJ { get; set; }
        public EmployeeDetailsForLeaveView EmployeeDetails { get; set; }
        public bool IsMail { get; set; }
    }
}
