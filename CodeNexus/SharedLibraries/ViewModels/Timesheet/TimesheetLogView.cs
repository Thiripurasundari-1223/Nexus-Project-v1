using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class TimesheetLogView
    {
        public int TimesheetLogId { get; set; }
        public int? ProjectId { get; set; }
        public int? ResourceId { get; set; }
        public DateTime PeriodSelection { get; set; }
        public string RequiredHours { get; set; }
        public string ClockedHours { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? TimesheetId { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public Guid? WeekTimesheetId { get; set; }
        public string ProjectName { get; set; }
        public string WorkItem { get; set; }
    }
}
