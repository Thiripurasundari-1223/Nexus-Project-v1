using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveDetailsView
	{
        public int LeaveTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveCode { get; set; }
        public int? LeaveAccruedType { get; set; }
        public string LeaveAccruedDay { get; set; }
        public decimal? LeaveAccruedNoOfDays { get; set; }
        public string LeaveDescription { get; set; }
        //public bool? IsActive { get; set; }
        public int? LeaveTypesId { get; set; }
        public bool? ProRate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public int? BalanceBasedOn { get; set; }
        public List<ProRateMonthList> ProRateMonthList { get; set; }
        public LeaveApplicableView LeaveApplicable { get; set; }
        public LeaveExceptionView LeaveException { get; set; }
        public LeaveEntitlementView LeaveEntitlement { get; set; }
        public LeaveRestrictionsView LeaveRestrictions { get; set; }
        public List<int> LeaveApplicableDepartmentId { get; set; }
        public List<int> LeaveApplicableDesginationId { get; set; }
        public List<int> LeaveApplicableLocationId { get; set; }
        public List<int> LeaveApplicableRoleId { get; set; }
        public List<int> LeaveExceptionDepartmentId { get; set; }
        public List<int> LeaveExceptionDesginationId { get; set; }
        public List<int> LeaveExceptionLocationId { get; set; }
        public List<int> LeaveExceptionRoleId { get; set; }
        public List<int?> LeaveApplicableEmployeeId { get; set; }
        public List<int?> LeaveEmployeeTypeId { get; set; }
        public List<int?> LeaveProbationStatusId { get; set; }
        public List<int?> LeaveExceptionEmployeeTypeId { get; set; }
        public List<int?> LeaveExceptionProbationStatusId { get; set; }
        public List<int?> LeaveExceptionEmployeeId { get; set; }
        public int? CreatedBy { get; set; }
        public List<GrantLeaveApprovalView> GrantLeaveApprovalList { get; set; }
        public bool? AllowTimesheet { get; set; }
    }
	public class LeaveEntitlementView
	{
		public int LeaveEntitlementId { get; set; }
		public int? MaxLeaveAvailedYearId { get; set; }
		public decimal? MaxLeaveAvailedDays { get; set; }
		//public decimal? MaxLimitDays { get; set; }
		//public decimal? RestrictLeaveApplicationDays { get; set; }
		//public decimal? DocumentMandatoryDays { get; set; }
		//public bool? AllowEncashmentCarryForward { get; set; }
		//public decimal? NoOfDays { get; set; }
		//public int? LeaveMaxLimitActionId { get; set; }
		public int? LeaveTypeId { get; set; }
        public int? CarryForwardId { get; set; }
        public decimal? MaximumCarryForwardDays { get; set; }
        public int? ReimbursementId { get; set; }
        public decimal? MaximumReimbursementDays { get; set; }
        public int? ResetYear { get; set; }
        public int? ResetMonth { get; set; }
        public string ResetDay { get; set; }
    }
    public class LeaveApplicableView
    {
        public int? LeaveApplicableId { get; set; }
        public bool? Gender_Male { get; set; }
        public bool? Gender_Female { get; set; }
        public bool? Gender_Others { get; set; }
        public bool? MaritalStatus_Single { get; set; }
        public bool? MaritalStatus_Married { get; set; }
        public int? EmployeeTypeId { get; set; }
        public int? ProbationStatus { get; set; }
        public int? LeaveTypeId { get; set; }
    }

    public class LeaveRestrictionsView
    {
        public int LeaveRestrictionsId { get; set; }
        //public int? WeekendCountAfterDays { get; set; }
        //public int? HolidayCountAfterDays { get; set; }
        public bool? ExceedLeaveBalance { get; set; }
        public List<DurationAllowed> DurationsAllowedId { get; set; }
        public int? AllowUsersViewId { get; set; }
        public decimal? BalanceDisplayedId { get; set; }
        public decimal? DaysInAdvance { get; set; }
        public string AllowRequestDates { get; set; }
        public decimal? AllowRequestNextDays { get; set; }
        public decimal? DatesAppliedAdvance { get; set; }
        public decimal? MaximumLeavePerApplication { get; set; }
        public decimal? MinimumGapTwoApplication { get; set; }
        public decimal? MaximumConsecutiveDays { get; set; }
        public decimal? EnableFileUpload { get; set; }
        public decimal? MinimumNoOfApplicationsPeriod { get; set; }
        public int? AllowRequestPeriodId { get; set; }
        public bool? MaximumLeave { get; set; }
        public bool? MinimumGap { get; set; }
        public bool? MaximumConsecutive { get; set; }
        public bool? EnableFile { get; set; }
        public string CannotBeTakenTogether { get; set; }
        public int? LeaveTypeId { get; set; }
        public bool? AllowPastDates { get; set; }
        public bool? AllowFutureDates { get; set; }
        public bool? IsAllowRequestNextDays { get; set; }
        public bool? IsToBeApplied { get; set; }
        public bool? Weekendsbetweenleaveperiod { get; set; }
        public bool? Holidaybetweenleaveperiod { get; set; }
        public decimal? GrantMinimumNoOfRequestDay { get; set; }
        public decimal? GrantMaximumNoOfRequestDay { get; set; }
        public int? GrantMaximumNoOfPeriod { get; set; }
        public int? GrantMaximumNoOfDay { get; set; }
        public decimal? GrantMinimumGapTwoApplicationDay { get; set; }
        public decimal? GrantUploadDocumentSpecificPeriodDay { get; set; }
        public bool? IsGrantRequestPastDay { get; set; }
        public int? GrantRequestPastDay { get; set; }
        public bool? IsGrantRequestFutureDay { get; set; }
        public int? GrantRequestFutureDay { get; set; }
        public List<ActiveLeaveList> activeLeaveType { get; set; }
        public List<ActiveHolidayList> activeHoliday { get; set; }
        public List<SpecificEmployeeDetailLeaveList> SpecificEmployeeDetailLeaveList { get; set; }
        public decimal? GrantResetLeaveAfterDays { get; set; }
        public decimal? ToBeAdvanced { get; set; }
    }
    public class DurationAllowed
    {
        public int DurationId { get; set; }
        public string DurationValue { get; set; }
    }
    public class ActiveLeaveList
    {
        public int? leaveTypeId { get; set; }
        public string leaveType { get; set; }
    }
    public class ActiveHolidayList
    {
        public int? holidayID { get; set; }
        public string holidayName { get; set; }
    }
    public class ProRateMonthList
    {
        public string Fromday { get; set; }
        public string Today { get; set; }
        public decimal? Count { get; set; }
    }
    public class SpecificEmployeeDetailLeaveList
    {
        public int? EmployeeDetailLeaveId { get; set; }
        public string EmployeeDetailLeaveValue { get; set; }
    }
    public class LeaveExceptionView
    {
        public int? LeaveApplicableId { get; set; }
        public bool? Gender_Male_Exception { get; set; }
        public bool? Gender_Female_Exception { get; set; }
        public bool? Gender_Others_Exception { get; set; }
        public bool? MaritalStatus_Single_Exception { get; set; }
        public bool? MaritalStatus_Married_Exception { get; set; }
        public int? LeaveTypeId { get; set; }
    }
}