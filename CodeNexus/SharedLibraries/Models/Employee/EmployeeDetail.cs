using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
namespace SharedLibraries.Models.Employee
{
    public class EmployeeDetail
    {
		public int EmployeeID { get; set; }
		public string FormattedEmployeeID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string EmployeeType { get; set; }
		public string Department { get; set; }
		public string Role { get; set; }
		public DateTime? DateOfJoining { get; set; }
		public DateTime? DateOfContract { get; set; }
		public DateTime? DateOfRelieving { get; set; }
		public string ReportingManager { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
		public string Gender { get; set; }
		public string Maritalstatus { get; set; }
		public string ProbationStatus { get; set; }
		public string Location { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public DateTime? WeddingAnniversary { get; set; }
		public string PersonalMobileNumber { get; set; }
		public string SystemRole { get; set; }
		public Employees Employee { get; set; }
		public string Skillset { get; set; }
		public string EmployeeDependent { get; set; }
		public List<EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
	}
}