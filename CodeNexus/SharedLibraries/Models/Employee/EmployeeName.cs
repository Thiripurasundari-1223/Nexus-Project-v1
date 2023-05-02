using System;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeName
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string EmployeeEmailId { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public EmailClass ReportingManagerData { get; set; }
        public string profilePic { get; set; }
        public int? LeaveCount { get; set; }
        public bool isGrantLeave{get;set;}
    }

    public class EmailClass
    {
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmailId { get; set; }
    }
}
