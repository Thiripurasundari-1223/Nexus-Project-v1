using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeAttendanceDetails
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime? Date { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public string Location { get; set; }
        public int? ShiftDetailId { get; set; }
        public string ShiftName { get; set; }
        public string WorkedHours { get; set; }
        public List<EmployeesAttendanceDetails> attendanceDetails { get; set; }
        public string FormattedEmployeeId { get; set; }
        public List<EmployeeShiftDetailsView> employeeShiftDetails { get; set; }
    }
}