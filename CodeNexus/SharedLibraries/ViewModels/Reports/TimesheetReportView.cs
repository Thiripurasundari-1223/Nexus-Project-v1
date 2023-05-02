using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class TimesheetReportView
    {
        public List<TimesheetReport> ProjectWiseTimesheetPlannedActualReport { get; set; }
        public List<TimesheetReport> ResourceWiseWeeklyTimesheetReport { get; set; }
        public List<TimesheetReport> ResourceWiseTimesheetPlannedActualReport { get; set; }
        public List<TimesheetReportDetails> TimesheetReportDetails { get; set; }
        public List<Skillsets> Skillsets { get; set; }

    }
}
