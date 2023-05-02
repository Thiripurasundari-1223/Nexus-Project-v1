using SharedLibraries.Models.Timesheet;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class TimesheetMasterData
    {
       public List<SharedLibraries.Models.Timesheet.Timesheet> Timesheet { get; set; }
        public List<TimesheetLog> TimesheetLog { get; set; }
    }
}
