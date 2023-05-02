using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Projects
{
    public class EmployeeProjectNames
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAllocation { get; set; }
        public string IsBillable { get; set; }
    }
    public class EmployeeProjectList
    {
        public int employeeId;
        public DateTime Date { get; set; }
        public List<EmployeeProjectNames> ProjectList { get; set; }
    }

}
