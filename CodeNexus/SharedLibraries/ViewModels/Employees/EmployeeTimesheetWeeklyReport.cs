using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeTimesheetWeeklyReport
    {
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Utilization { get; set; }
        public string Status { get; set; }
        public List<WeekDatesandHours> WeekDatesandHours { get; set; }
    }
    public class WeekDatesandHours
    {
        public DateTime? Dates { get; set; }
        public string Hours { get; set; }
    }
}
