using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class ResignationEmployeeMasterView
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? LocationId { get; set; }
        public int? DesignationId { get; set; }
        public string Designation { get; set; }
        public string FormattedEmployeeID { get; set; }
        public DateTime? RelievingDate { get; set; }
        public int? ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmail { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string EmployeeEmail { get; set; }
        public DateTime? ResignationDate { get; set; }
        public string EmployeeType { get; set; }
        public bool? IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ResignationDetailsId { get; set; }
        public string ResignationStatus { get; set; }
        public int? ResignationChecklistId { get; set; }
        public string ManagerStatus { get; set; }
        public string ITStatus { get; set; }
        public string AdminStatus { get; set; }
        public string FinanceStatus { get; set; }
        public string HRStatus { get; set; }
        public string RoleName { get; set; }
        public EmployeesPersonalInfoView PersonalInfo { get; set; }
        public string ProfilePic { get; set; }
    }
    public class EmployeesPersonalInfoView
    {
        public int EmployeeID { get; set; }
        public string FormattedEmployeeID { get; set; }
        public string EmergencyMobileNumber { get; set; }
        public string OtherEmail { get; set; }
        public string PersonalMobileNumber { get; set; }
        public string PermanentAddressLine1 { get; set; }
        public string PermanentAddressLine2 { get; set; }
        public string PermanentCity { get; set; }
        public int? PermanentState { get; set; }
        public int? PermanentCountry { get; set; }
        public string PermanentAddressZip { get; set; }
        public string CommunicationAddressLine1 { get; set; }
        public string CommunicationAddressLine2 { get; set; }
        public string CommunicationCity { get; set; }
        public int? CommunicationState { get; set; }
        public int? CommunicationCountry { get; set; }
        public string CommunicationAddressZip { get; set; }
    }
}
