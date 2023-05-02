using System;
using SharedLibraries.Models.Leaves;
using System.Collections.Generic;
using System.Text;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeavesMasterDataView
    {
        public List<LeaveMaxLimitAction> LeaveMaxLimitActionList { get; set; }
        //public List<ProbationStatus> ProbationStautsList { get; set; }
        public List<MonthList> MonthList { get; set; }
        public List<DaysList> DaysList { get; set; }
        public List<AllowUser> AllowUserList { get; set; }
        public List<BalanceToBeDisplay> BalanceToBeDisplayList { get; set; }
        public List<Department> EmployeeDepartmentList { get; set; }
        public List<RoleName> RoleNamesList { get; set; }
        public List<EmployeesTypes> EmployeeTypeList { get; set; }
        public List<EmployeeList> EmployeeList { get; set; }
        public List<EmployeeLocation> EmployeeLocationList { get; set; }
        public List<Designation> EmployeeDesignationList { get; set; }
        public List<AppConstantsView> LeaveDurationList { get; set; }
        public List<AppConstantsView> ReportConfigurationList { get; set; }
        public List<HolidayViewDetails> CurrentFinancialYearHolidayList { get; set; }
        public List<LeaveTypesDetailView> ActiveLeaveTypeList { get; set; }
        public List<AppConstantsView> LeaveTypeList { get; set; }
        public List<AppConstantsView> LeaveAccuredList { get; set; }
        public List<AppConstantsView> AllowRequestPeriod { get; set; }
        public List<AppConstantsView> SpecificEmployeeLeaveList { get; set; }
        public List<ProbationStatusView> ProbationStatusList { get; set; }
        public List<AppConstantsView> CarryForwardList { get; set; }
        public List<AppConstantsView> ReimbursementList { get; set; }
        public List<AppConstantsView> BalanceBasedOn { get; set; }
        public List<AppConstantsView> GrantLeaveRequestPeriod { get; set; }
        public List<AppConstantsView> GrantLeaveApproval { get; set; }
    }
    public class ProbationStatus
    {
        public int Id { get; set; }
        public string ProbastionStatus { get; set; }
    }
    public class MonthList
    {
        public int Id { get; set; }
        public string Month { get; set; }
    }
    public class DaysList
    {
        public int Id { get; set; }
        public string Day { get; set; }
    }
    public class AllowUser
    {
        public int Id { get; set; }
        public string AllowUsers { get; set; }
    }
    public class BalanceToBeDisplay
    {
        public int Id { get; set; }
        public string Balance { get; set; }
    }
}



