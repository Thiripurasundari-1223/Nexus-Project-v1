using System;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveAdjustment
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public int? DepartmentId { get; set; }
        public string Designation { get; set; }
        public decimal? LeaveBalance { get; set; }
        public string FormattedEmployeeId { get; set; }
        public DateTime? DOJ { get; set; }
    }
}