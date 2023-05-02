using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ProjectTeamDetail
    {
        public int? EmployeeId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string  EmployeeName{ get; set; }
        public string ProfileImage { get; set; }
    }
}
