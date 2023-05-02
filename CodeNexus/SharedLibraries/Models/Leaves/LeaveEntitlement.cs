using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibraries.Models.Leaves
{
    [Table("LeaveEntitlement")]
    public class LeaveEntitlement
    {
		[Key]
        public int LeaveEntitlementId { get; set; }
        public int? MaxLeaveAvailedYearId { get; set; }
		public decimal? MaxLeaveAvailedDays { get; set; }
		//public decimal? MaxLimitDays { get; set; }
		//public bool? AllowEncashmentCarryForward { get; set; }
		//public decimal? NoOfDays { get; set; }
		//public int? LeaveMaxLimitActionId { get; set; }
		public int? LeaveTypeId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; } 
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public int? CarryForwardId { get; set; }
		public decimal? MaximumCarryForwardDays { get; set; }
		public int? ReimbursementId { get; set; }
		public decimal? MaximumReimbursementDays { get; set; }
		public int? ResetYear { get; set; }
		public int? ResetMonth { get; set; }
		public string ResetDay { get; set; }
	}
}