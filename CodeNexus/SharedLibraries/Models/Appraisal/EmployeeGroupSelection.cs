using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
   public class EmployeeGroupSelection
    {
		[Key]
		public int APP_CYCLE_ID { get; set; }
		[Key]
		public int EMPLOYEE_ID { get; set; }
		[Key]
		public int OBJECTIVE_ID { get; set; }
		[Key]
		public int KEY_RESULTS_GROUP_ID { get; set; }
		[Key]
		public int KEY_RESULT_ID { get; set; }
		public int? CREATED_BY { get; set; }
		public DateTime? CREATED_DATE { get; set; }
		public int? UPDATED_BY { get; set; }
		public DateTime? UPDATED_DATE { get; set; }
		public decimal? GRP_KEYRES_ACTUAL_VALUE { get; set; }
		public decimal? INDIVIDUAL_GRPITEM_RATING { get; set; }
		public int? INDIVIDUAL_KEYRES_STATUS { get; set; }
		public int? IS_ADDRESSED { get; set; }
		//public bool? IsSelected { get; set; }
	}
}
