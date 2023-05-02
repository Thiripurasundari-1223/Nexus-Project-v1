using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class AttendanceWeekView
    {
        public int EmployeeId { get; set; }
        public DateTime DateTime { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


    }
}
