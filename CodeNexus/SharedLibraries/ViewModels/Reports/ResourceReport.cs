using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ResourceReport
    {
        public int EmployeeId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string ResourceName { get; set; }
        public string SKillSet { get; set; }
        public string Role { get; set; }
        public int ProjectId { get; set; }
        public string FormattedProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Billability { get; set; }
        public decimal? Utilization { get; set; }
        public int TimesheetStatus { get; set; }

    }
}
