using System;
using System.Collections.Generic;
using System.Text;
using SharedLibraries.Models.Appraisal;

namespace SharedLibraries.ViewModels.Appraisal
{
	public class IndividualAppraisalAddView
	{
		public bool IsSubmit { get; set; }
		public int EntityID { get; set; }
		public string Status { get; set; }
		public decimal? overAllRating { get; set; }
		public List<EmployeeKeyResultRating> EmployeeKeyResultRatings { get; set; }
		public List<EmployeeObjectiveRating> EmployeeObjectiveRatings { get; set; }
		public List<EmployeeGroupSelection> EmployeeGroupSelections { get; set; }
		public List<EmployeeGroupRating> EmployeeGroupRatings { get; set; }

	}

	public class EmployeeKeyResultRatings
    {
		public int? APP_CYCLE_ID { get; set; }
		public int EMPLOYEE_ID { get; set; }
		public int OBJECTIVE_ID { get; set; }
		public int KEY_RESULT_ID { get; set; }
		public decimal KEY_RESULT_ACTUAL_VALUE { get; set; }
		public decimal KEY_RESULT_MAX_RATING { get; set; }
		public decimal KEY_RESULT_RATING { get; set; }

	}
	public class EmployeeObjectiveRatings
    {
		public int APP_CYCLE_ID { get; set; }
		public int EMPLOYEE_ID { get; set; }
		public int OBJECTIVE_ID { get; set; }
		public decimal OBJECTIVE_MAX_RATING { get; set; }
		public decimal	OBJECTIVE_RATING { get; set; }

	}
	public class EmployeeGroupSelections
    {
		public int? APP_CYCLE_ID { get; set; }
		public int? EMPLOYEE_ID { get; set; }
		public int? OBJECTIVE_ID { get; set; }
		public int? KEY_RESULT_ID { get; set; }
		public int? KEY_RESULTS_GROUP_ID { get; set; }
		public decimal? GRP_KEYRES_ACTUAL_VALUE { get; set; }
		public decimal? INDIVIDUAL_GRPITEM_RATING { get; set; }
		public int? INDIVIDUAL_KEYRES_STATUS { get; set; }
	}
	public class EmployeeGroupRatings
    {
		public int APP_CYCLE_ID { get; set; }
		public int EMPLOYEE_ID { get; set; }
		public int OBJECTIVE_ID { get; set; }
		public int KEY_RESULTS_GROUP_ID { get; set; }
		public decimal? KEY_RESULTS_GROUP_MAX_RATING { get; set; }
		public decimal? KEY_RESULTS_GROUP_RATING  { get; set; } 
		
	}

	//public class SendMailbyIndividual
    //   {
	//	public string ToEmailID { get; set; } 
	//	public string Subject { get; set; } 
	//	public string MailBody { get; set; } 
	//	public string FromEmailID { get; set; } 
	//	public int Port { get; set; }
	//	public string Host { get; set; } 
	//	public string FromEmailPassword { get; set; }
	//	public string ResourceEmail { get; set; }
	//	public string CC { get; set; } 

	//}
}
