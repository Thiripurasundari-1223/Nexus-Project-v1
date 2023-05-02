using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class EmployeeAbsentConfigurationView
    {
        public List<WeeklyMonthlyAttendance> MarkedAttendance { get; set; }
        public EmployeeDetailsForLeaveView EmployeeDetails { get; set; }
    }
}
