using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibraries.Models.Leaves
{
    [Table("LeaveRestrictions")]
	public class LeaveRestrictions
	{
		[Key]
		public int LeaveRestrictionsId { get; set; }
		//public int? WeekendCountAfterDays { get; set; }
		//public int? HolidayCountAfterDays { get; set; }
		public bool? ExceedLeaveBalance { get; set; }
		//public int? DurationsAllowedId { get; set; }
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
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
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
		public decimal? GrantResetLeaveAfterDays { get; set; }
		public decimal? ToBeAdvanced { get; set; }
	}
}