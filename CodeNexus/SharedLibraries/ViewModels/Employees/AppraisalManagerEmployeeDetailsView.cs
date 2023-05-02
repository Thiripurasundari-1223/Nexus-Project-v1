using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeelistForAppraisalMaster
    {
        public List<int> listEmployeeID { get; set; }
        public int ManagerID { get; set; }
    }
    public class AppraisalManagerEmployeeDetailsView
    {
        public int EmployeeID { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? ReportingManagerRoleID { get; set; }
        public int? ReportingManagerDeptID { get; set; }
        public string ReportingManagerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmployeeName> EmployeeDetails { get; set; }
        public string FormattedEmployeeId { get; set; }
    }
    public class EmployeeandManagerView
    {
        public int EmployeeID { get; set; }
        public int? ReportingManagerID { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public string EmployeeEmailID { get; set; }
        public string ManagerEmailID { get; set; }
        public string FormattedEmployeeId { get; set; }
        public int? LocationId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? RelivingDate { get; set; }
    }
}