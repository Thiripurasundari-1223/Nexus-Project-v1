using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class TimesheetReportDetails
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ResourceId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string ResourceName { get; set; }
        public string PlannedHours { get; set; }
        public string ClockedHours { get; set; }
        public string FormattedProjectId { get; set; }
        public bool IsSubmitted { get; set; }
        public string RoleName { get; set; }
        public string SKillSet { get; set; }
    }
}
