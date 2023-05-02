using System;
using System.Collections.Generic;
using static SharedLibraries.ViewModels.Leaves.WeeklyOverviewReportView;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class EmployeeTimesheetHourView
    {
        public int employeeId { get; set; }
        public int projectId { get; set; }
        public DateTime date { get; set; }
        public TimeSpan? RequiredHours { get; set; }
        public TimeSpan? clockedHours { get; set; }
        public string WorkItem { get; set; }
    }
    public class EmployeeTimesheetDetailsView
    {
        public string Type { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Hours { get; set; }
        public decimal Percentage { get; set; }
    }
    public class EmployeeTimesheetGridView
    {
        public string Type { get; set; }
        public int? EmployeeId { get; set; }
        public int? projectId { get; set; }
        public string projectName { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? RequiredHour { get; set; }
        public TimeSpan? clockedHour { get; set; } 
        public string Utilization { get; set; }
        public string status { get; set; }
        public string WorkItem { get; set; }
    }
    public class Timesheetgrid
    {
        public string Type { get; set; }
        public int EmployeeId { get; set; }
        public int projectId { get; set; }
        public string projectName { get; set; }
        public string Utilization { get; set; }
        public string status { get; set; }
        public List<DatesandHours> DatesandHours { get; set; }
        public List<DatesandHours> TotalEmployeeTime { get; set; }
    }
    public class DatesandHours
    {
        public DateTime? Dates { get; set; }
        public string Hours { get; set; }
    }
    public class HomeTimesheetReportData
    {
        public List<EmployeeTimesheetDetailsView> EmployeeTimesheetDetails { get; set; }
        public List<Timesheetgrid> Timesheetgrid { get; set; }
        public List<WeeklyOverview> WeeklyOverview { get; set; }
    }
}
