using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
	public class AddApproveandRejectByManagerView
	{
		public bool IsSubmit { get; set; }
		public string Status { get; set; }
		public int EntityID { get; set; }
		public int ManagerID { get; set; }
		public int AppCycleId { get; set; }
		public int EmployeeId { get; set; }
		public List<EmployeeKeyResultRatingStatus> EmployeeKeyResultRatingStatus { get; set; }

		public List<EmployeeGroupSelectionStatus> EmployeeGroupSelectionStatus { get; set; }

	}

	public class EmployeeKeyResultRatingStatus
	{

		public int APP_CYCLE_ID { get; set; }
		public int EMPLOYEE_ID { get; set; }
		public int OBJECTIVE_ID { get; set; }
		public int KEY_RESULT_ID { get; set; }
		public int KEY_RESULT_STATUS { get; set; }
		public int UPDATED_BY { get; set; }
		public DateTime UPDATED_DATE { get; set; }
		public bool IsApproved { get; set; }

	}

	public class EmployeeGroupSelectionStatus
	{
		public int APP_CYCLE_ID { get; set; }
		public int EMPLOYEE_ID { get; set; }
		public int OBJECTIVE_ID { get; set; }
		public int KEY_RESULT_ID { get; set; }
		public int KEY_RESULTS_GROUP_ID { get; set; }
		public int? INDIVIDUAL_KEYRES_STATUS { get; set; }
		public int UPDATED_BY { get; set; }
		public DateTime UPDATED_DATE { get; set; }
		public bool IsApproved { get; set; }
	}
}
