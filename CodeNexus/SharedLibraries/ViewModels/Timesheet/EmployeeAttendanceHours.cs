using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class EmployeeAttendanceHours
    {
        public int EmployeeId { get; set; }
        public string TotalHours { get; set; }
        public DateTime Date { get; set; }

    }
}
