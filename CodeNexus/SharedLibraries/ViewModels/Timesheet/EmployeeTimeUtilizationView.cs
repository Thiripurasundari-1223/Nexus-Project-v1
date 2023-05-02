using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class EmployeeTimeUtilizationView
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string IsBillable { get; set; }
        public TimeSpan? PlannedHour { get; set; }
        public TimeSpan? ActualHour { get; set; }
        public decimal Utilization { get; set; }
        public int ResourceId { get; set; }
        public DateTime Date { get; set; }     
        public string Status { get; set; }
        public string WorkItem { get; set; }

    }
}
