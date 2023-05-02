using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveTypes
	{
		[Key]
		public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
		public string LeaveCode { get; set; }
		public int? LeaveAccruedType { get; set; }
		public string LeaveAccruedDay { get; set; }
		public decimal? LeaveAccruedNoOfDays { get; set; }
		public string LeaveDescription { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public int? LeaveTypesId { get; set; }
		public bool? ProRate { get; set; }
		public DateTime? EffectiveFromDate { get; set; }
		public DateTime? EffectiveToDate { get; set; }
		public int? BalanceBasedOn { get; set; }
		public bool? IsActive { get; set; }
		public bool? AllowTimesheet { get; set; }
	}
}