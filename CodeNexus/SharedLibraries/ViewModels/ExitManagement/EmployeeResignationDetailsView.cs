using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class EmployeeResignationDetailsList
    {
        public List<int> AllReportees { get; set; }
        public List<EmployeeResignationDetailsView> EmployeeResignationDetailsView { get; set; }
        public List<ResignationEmployeeMasterView> EmployeeListView { get; set; }
    }
    public class EmployeeResignationDetailsView
    {
        public int EmployeeResignationDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string EmployeeDesignation { get; set; }
       // public string EmployeeDesignationName { get; set; }
        public string ResidanceContactNumber { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? ResignationReasonId { get; set; }
        public string ResignationReason { get; set; }
        public string ResignationStatus { get; set; }
        public int? ModifiedBy { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? ActualRelievingDate { get; set; }
        public DateTime? RelievingDate { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? StateId { get; set; }
        public string State { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public string WithdrawalReason { get; set; }
        public int? DepartmentID { get; set; }
        public int? ReportingManagerID { get; set; }
        public int? LocationId { get; set; }
        public string DepartmentName { get; set; }
        public string Location { get; set; }
        public string ResignationType { get; set; }
        public string Remarks { get; set; }
        public string ProfilePicture { get; set; }
        public string ReportingManager { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public ResignationApproverView ResignationApprover { get; set; }
        public int NoticePeriod { get; set; }
        public List<ResignationApprovalStatusView> ResignationApproverList { get; set; }
        public bool IsWithdrawal { get; set; }
        public bool IsActiveEmployee { get; set; }
        public string EmployeeType { get; set; }
        public ExitManagementEmailTemplate ApprovalEmailTemplate { get; set; }
        public ExitManagementEmailTemplate WithdrawalEmailTemplate { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public bool? IsAgreeCheckList { get; set; }
        public int? ResignationInterviewDetailId { get; set; }
        public DateTime? WithdrawalSubmmitedDate { get; set; }

    }

    public class EmployeeResignationChecklistDetailsView
    {
        public int EmployeeResignationDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FormattedEmployeeId { get; set; }
    }
}
