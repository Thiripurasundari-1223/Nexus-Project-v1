using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels
{
    public class SubmitTimesheet
    {
        public List<TimesheetLogView> TimesheetLog { get; set; }
        public int TimesheetId { get; set; }
        public int ReportingPersonId { get; set; }
        public string TotalClockedHours { get; set; }
        public string TotalApprovedHours { get; set; }
        public string Comments { get; set; }
        public List<ProjectSPOC> ListOfProjectSPOC { get; set; }
        public string TotalRequiredHours { get; set; }
    }
}