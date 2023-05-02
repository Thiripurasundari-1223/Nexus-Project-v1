using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeManagerAndHeadDetailsView
    {
        public int EmployeeId { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string EmployeeFullName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
        public string MangerName { get; set; }
        public string ManagerFormattedId { get; set; }
        public string ManagerEmail { get; set; }
        public int BUHeadId { get; set; }
        public string BUHeadEmail { get; set; }
        public string EmployeeFormattedId { get; set; }
        public string ApproverEmail { get; set; }
        public string ApproverName { get; set; }
    }
}
