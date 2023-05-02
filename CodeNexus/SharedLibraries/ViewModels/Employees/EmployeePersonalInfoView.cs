using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeePersonalInfoView
    {
		public int? PersonalInfoId { get; set; }
		public int? EmployeeId { get; set; }
		public string HighestQualification { get; set; }
		public string OtherEmail { get; set; }
		public string PersonalMobileNumber { get; set; }
		public int? BloodGroup { get; set; }
		public string BloodGroupName { get; set; }
		public string FathersName { get; set; }
		public string SpouseName { get; set; }
		public string PermanentAddressLine1 { get; set; }
		public string PermanentAddressLine2 { get; set; }
		public string PermanentCity { get; set; }
		public int? PermanentState { get; set; }
		public string PermanentStateName { get; set; }
		public int? PermanentCountry { get; set; }
		public string PermanentCountryName { get; set; }
		public string PermanentAddressZip { get; set; }
		public string CommunicationAddressLine1 { get; set; }
		public string CommunicationAddressLine2 { get; set; }
		public string CommunicationCity { get; set; }
		public int? CommunicationState { get; set; }
		public string CommunicationStateName { get; set; }
		public int? CommunicationCountry { get; set; }
		public string CommunicationCountryName { get; set; }
		public string CommunicationAddressZip { get; set; }
		public string Nationality { get; set; }
		public string NationalityName { get; set; }
		public string PANNumber { get; set; }
		public string UANNumber { get; set; }
		public string DrivingLicense { get; set; }
		public string AadhaarCardNumber { get; set; }
		public string PassportNumber { get; set; }
		public string EmergencyContactName { get; set; }
		public string EmergencyContactRelation { get; set; }
		public string EmergencyMobileNumber { get; set; }
		public string ReferenceContactName { get; set; }
		public string ReferenceEmailId { get; set; }
		public string ReferenceMobileNumber { get; set; }
		public bool? IsJoiningBonus { get; set; }
		public decimal? JoiningBonusAmmount { get; set; }
		public string JoiningBonusCondition { get; set; }
		public string AccountHolderName { get; set; }
		public string BankName { get; set; }
		public string IFSCCode { get; set; }
		public decimal? AccountNumber { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public List<DocumentsToUpload> employeeProofDocument { get; set; }

	}
}
