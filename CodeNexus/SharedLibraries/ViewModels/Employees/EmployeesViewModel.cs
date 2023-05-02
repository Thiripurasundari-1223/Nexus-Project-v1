using SharedLibraries.Models.Employee;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels
{
    public class EmployeesViewModel
    {
        public Models.Employee.Employees Employee { get; set; }
        public EmployeePersonalInfoView EmployeesPersonalInfo { get; set; }
        public EmployeeWorkAndEducationDetailView EmployeeWorkAndEducationDetail { get; set; }
        public List<Skillsets> Skillset { get; set; }
        public List<EmployeeSpecialAbility> EmployeespecialAbilities { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentHead { get; set; }
        public string EmployeeCategoryName { get; set; }
        public List<EmployeeDependentView> EmployeeDependent { get; set; }
        public List<EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
        public List<ResignationReason> ResignationReason { get; set; }
        public string SystemRoleName { get; set; }
        public string ProfilePicBase64 { get; set; }
        public List<DocumentsToUpload> AddressProof { get; set; }
        public DocumentsToUpload ProfilePicture { get; set; }
        public SupportingDocumentsView supportingDocumentsViews { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        public string CurrentWorkPlace { get; set; }
        public List<EmployeesDesignationHistory> designationHistory { get; set; }
        public List<DocumentsToUpload> RequestProofDocument { get; set; }
        public List<EmployeeRequestDetailsView> employeeRequestDetails { get; set; }
        public bool IsTriggerNotification { get; set; }
        public DocumentsToUpload CommunicationAddressProof { get; set; }
        public DocumentsToUpload PermanentAddressProof { get; set; }
        public DocumentsToUpload MaritalStatusProof { get; set; }
    }
}