using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class EmployeeLeaveTypeView
    {
        public int? EmployeeId { get; set; }
        public int? LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveCode { get; set; }
        public string LeaveAccruedMonthly { get; set; }
        public string LeaveAccruedLastDay { get; set; }
        public decimal? LeaveAccruedNoOfDays { get; set; }
        public List<DurationsAllowedDetails> DurationsAllowedDetails { get; set; }
    }
    public class LeaveRestrictionsViewDetails
    {
        public int LeaveRestrictionsId { get; set; }
        //public int? WeekendCountAfterDays { get; set; }
        //public int? HolidayCountAfterDays { get; set; }
        public bool? ExceedLeaveBalance { get; set; }
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
        public List<DurationsAllowedDetails> DurationsAllowedDetails { get; set; }
        public List<ActiveLeaveList> activeLeaveType { get; set; }
        public List<ActiveHolidayList> activeHoliday { get; set; }

    }
    public class DurationsAllowedDetails
    {
        public int LeaveTypeId { get; set; }
        public int DurationId { get; set; }
        public string DisplayName { get; set; }
        public string AppConstantValue { get; set; }
    }
    public class BalanceBasedOnDetails
    {
        public int BalanceBasedOnId { get; set; }
        public string BalanceBasedOnText { get; set; }
        public string BalanceBasedOnValue { get; set; }
    }
    public class EmployeeAvailableLeaveDetails
    {
        public int? EmployeeID { get; set; }
        public int? LeaveTypeID { get; set; }
        public decimal? BalanceLeave { get; set; }
        public decimal? BookedLeaveCount { get; set; }
        public string LeaveType { get; set; }
        public string LeaveCode { get; set; }
        public int? LeaveAccruedType { get; set; }
        public string LeaveAccruedDay { get; set; }
        public decimal? LeaveAccruedNoOfDays { get; set; }
        public string LeaveAccruedTypeName { get; set; }
        public int? LeaveTypesId { get; set; }
        public bool? ProRate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public bool? ExceedLeaveBalance { get; set; }
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
        public bool? AllowPastDates { get; set; }
        public bool? AllowFutureDates { get; set; }
        public bool? IsAllowRequestNextDays { get; set; }
        public bool? IsToBeApplied { get; set; }
        public bool? Weekendsbetweenleaveperiod { get; set; }
        public bool? Holidaybetweenleaveperiod { get; set; }
        public List<DurationsAllowedDetails> DurationsAllowedDetails { get; set; }
        public List<ActiveLeaveList> activeLeaveType { get; set; }
        public List<ActiveHolidayList> activeHoliday { get; set; }
        public decimal? NoOfAbsentDays { get; set; }
        public decimal? NoOfLopDays { get; set; }
        public List<DateTime> AbsentDatesList { get; set; }
        //public decimal? MaxLimitDays { get; set; }
        //public bool? AllowEncashmentCarryForward { get; set; }
        //public decimal? NoOfDays { get; set; }
        //public int? LeaveMaxLimitActionId { get; set; }
        public List<SpecificEmployeeDetailLeaveView> SpecificEmployeeDetailLeaveList { get; set; }
        public decimal? AdjustmentBalanceLeave { get; set; }
        public DateTime? AdjustmentEffectiveFromDate { get; set; }
        public int? CarryForwardId { get; set; }
        public decimal? MaximumCarryForwardDays { get; set; }
        public int? ReimbursementId { get; set; }
        public decimal? MaximumReimbursementDays { get; set; }
        public int? ResetYear { get; set; }
        public int? ResetMonth { get; set; }
        public string ResetDay { get; set; }
        public List<BalanceBasedOnDetails> BalanceBasedOn { get; set; }
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
        public DateTime? GrantEffectiveFromDate { get; set; }
        public decimal? ActualBalanceLeave { get; set; }
        public DateTime? LeaveResetOn { get; set; }
        public string ResetPeriod {get;set;}
        public List<AppliedLeaveTypeDetails> AppliedLeaveDates { get; set; }
        public List<LeaveAdjustmentDetails> LeaveAdjustmentDetails { get; set; }
        public List<LeaveCarryForward> LeaveCarryForward { get; set; }
        public decimal? ToBeAdvanced { get; set; }
        public decimal? ResetLeaveAfter { get; set; }
        public List<LeaveGrantRequestDetails> LeaveGrantRequestDetails { get; set; }
        public decimal? DisplayBalanceLeave { get; set; }
    }
}
