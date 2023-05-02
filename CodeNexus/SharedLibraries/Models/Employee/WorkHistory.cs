using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class WorkHistory
    {
		[Key]
		public int WorkHistoryId { get; set; }
		public int? EmployeeId { get; set; }
		public string OrganizationName { get; set;}
	    public string Designation { get; set;}
        public int? EmployeeTypeId { get; set; }
        public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
        public decimal? LastCTC{ get; set; }
        public string LeavingReason { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
