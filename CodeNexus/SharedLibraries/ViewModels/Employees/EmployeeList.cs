using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels
{
    public class EmployeeList
    {
        public int EmployeeId { get; set; }
        public string Employee_Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string profilePic { get; set; }
        public int? ManagerId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ShiftId { get; set; }
        public int? LocationId { get; set; }
        public string ShiftFromTime { get; set; }
        public string ShiftToTime { get; set; }
        public List<WeekendDetails> WeekendId { get; set; }
        public List<EmployeeShiftDetailsView> employeeShiftDetails { get; set; }
        public EmployeeShiftDetailsView defaultShiftDetails { get; set; }
        public DateTime? DOJ { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public int? ProbationStatusId { get; set; }
        public string ProbationStatusName { get; set; }
        public string EmpName { get; set; }
        public bool IsFlexyShift { get; set; }
        public int? SystemRoleId { get; set; }
        public string SystemRoleName { get; set; }
        public string EmployeeFullName { get; set; }
        public int? leaveCount { get;set; }
        public int? attendanceCount { get;set; }
        public int? timesheetCount { get;set; }
        public bool isGrantLeave { get; set; }


    }
    public class ReportingManagerEmployeeList
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public int? ReportingManagerId { get; set; }
        public string ReportingTo { get; set; }
        public string ReportingEmailId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string DesignationName { get; set; }
    }
    public class ProjectCustomerEmployeeList
    {
        public List<int> EmployeeList { get; set; }
        public string RoleName { get; set; }
        public List<int?> AccountIdList { get; set; }
        public List<string> ManagementRole { get; set; }
    }
    public class EmployeeDataForDropDown
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeFullName { get; set; }
        public string FormattedEmployeeId { get; set; }
    }

    public class EmployeeRequestCount
    {
        public int EmployeeId { get; set; }
        public int RequestCount { get; set; }
    }
}
