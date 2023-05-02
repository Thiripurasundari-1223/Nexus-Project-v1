using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class EmployeesAttendanceDetails
    {
        public int EmployeeId { get; set; }
        public DateTime? Date { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Location { get; set; }
        public string WorkedHours { get; set; }
        public List<EmployeeAttendanceDetails> attendanceDetail { get; set; }

    }
}
