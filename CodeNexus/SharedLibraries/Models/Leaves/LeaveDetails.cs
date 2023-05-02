using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibraries.Models.Leaves
{
	[Table("LeaveDetails")]
	public class LeaveDetails
	{
		[Key]
		public int LeaveDetailsId { get; set; }
        public string LeaveType { get; set; }
		public string LeaveCode { get; set; }
		public int LeaveAccruedMonthlyId { get; set; }
		public int LeaveAccruedLastDayId { get; set; }
		public int LeaveAccruedNoOfDays { get; set; }
		public string LeaveDescription { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
    }
}
