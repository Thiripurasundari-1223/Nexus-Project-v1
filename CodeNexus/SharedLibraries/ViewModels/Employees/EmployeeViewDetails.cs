using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeViewDetails
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeType { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string ReportingTo { get; set; }
        public string ReportingEmail { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string SystemRoleName { get; set; }
    }
    public class ReporteesChecklistEmployeeView
    {
        public List<EmployeeViewDetails> EmployeeDetails { get; set; }
        public List<string> Role { get; set; }
    }
}