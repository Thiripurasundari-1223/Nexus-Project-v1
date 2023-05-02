using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class DetailsView
    {
        public int EmployeeId { get; set; }
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public string TotalHours { get; set; }
        public string BreakHours { get; set; }
        public string OverTime { get; set; }
    }
}
