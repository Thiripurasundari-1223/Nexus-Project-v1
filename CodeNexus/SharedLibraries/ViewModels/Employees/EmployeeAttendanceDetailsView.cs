using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeAttendanceDetailsView
    {
        public List<EmployeeAttendanceDetails> AttendanceDetailsList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<Designation> DesignationList { get; set; }
        public List<EmployeeLocation> LocationList { get; set; }
    }
}