using SharedLibraries.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Timesheet
{
    public class ProjectTimesheetView
    {
        public int ResourceId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<ProjectNames> projectList { get; set; }
    }
}
