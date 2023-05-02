using SharedLibraries.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class TimesheetUtilizationView
    {
        public int ProjectId { get; set; }
        public int ResourceId { get; set; }
        public string ProjectName { get; set; }
        public TimeSpan? PlannedHour { get;set; }
        public TimeSpan? ActualHour { get; set; }
        public DateTime Date { get; set; }
        public string WorkItem { get; set; }
    }
    public class TimesheetUtilizationReport
    {
        public List<ProjectNames> projectNamesList { get; set; }
        public List<TimesheetUtilizationView> timesheetUtilization { get; set; }
    }
}
