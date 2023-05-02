using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeDepartmentAndLocationView : PaginationViewModel
    {
        public int DepartmentId { get; set; }
        public int LocationId { get; set; }
        public int ShiftId { get; set; }
        public int employeeId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string FormattedEmployeeId { get; set; }
        public DateTime DOJ { get; set; }
        public List<EmployeeList> ReportingEmployeeList { get; set; }
        public List<EmployeeShiftDetailsView> EmployeeShiftDetails { get; set; }
        public EmployeeDetailsForLeaveView EmployeeDetails { get; set; }
    }
    public class EmployeeLeavesForTimeSheetViewInput
    {
        public int resourceId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
    public class EmployeeNameDepartmentAndLocationView
    {
        public DateTime? BirthDate { get; set; }
        public DateTime? WeddingAnniversary { get; set; }
        public List<EmployeeName> EmployeeName { get; set; }
        public EmployeeDepartmentAndLocationView EmployeeDepartmentAndLocationDetails { get; set; }
        public List<EmployeeShiftDetailsView> EmployeeShiftDetails { get; set; }
    }
    public class EmployeeNameDepartmentLocation
    {
        public int EmployeeId { get; set; }
        public List<int> EmployeeList { get; set; }
    }
}