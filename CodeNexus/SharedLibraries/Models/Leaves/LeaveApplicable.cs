using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibraries.Models.Leaves
{
	[Table("LeaveApplicable")]
	public class LeaveApplicable
	{
		[Key]
		public int LeaveApplicableId {get;set;}
        public bool? Gender_Male { get; set; }
		public bool? Gender_Female { get; set; }
		public bool? Gender_Others { get; set; }
		public bool? MaritalStatus_Single { get; set; }
		public bool? MaritalStatus_Married { get; set; }
		public int? EmployeeTypeId { get; set; }
		public int? ProbationStatus { get; set; }
		public int? LeaveTypeId { get; set; }
		public DateTime? CreatedOn { get; set; }
	    public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public bool? Gender_Male_Exception { get; set; }
		public bool? Gender_Female_Exception { get; set; }
		public bool? Gender_Others_Exception { get; set; }
		public bool? MaritalStatus_Single_Exception { get; set; }
		public bool? MaritalStatus_Married_Exception { get; set; }
		public string Type { get; set; }
	}
}
