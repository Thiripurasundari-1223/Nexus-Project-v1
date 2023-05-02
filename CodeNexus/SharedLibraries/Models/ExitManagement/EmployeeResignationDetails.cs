using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class EmployeeResignationDetails
    {
        [Key]
        public int EmployeeResignationDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string EmployeeDesignation { get; set; }
        public string ResidanceContactNumber { get; set; }
        public string MobileNumber{ get; set; }
        public string PersonalEmailAddress{ get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? ResignationReasonId { get; set; }
        public string ResignationStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ActualRelievingDate { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? RelievingDate { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string WithdrawalReason { get; set; }
        public string ResignationReason { get; set; }
        public int? ReportingManagerId { get; set; }

        public string ReportingManager { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string ResignationType { get; set; }
        public string Remarks { get; set; }
        public string ProfilePicture { get; set; }
        //  public string Designation { get; set; }
        public DateTime? WithdrawalSubmmitedDate { get; set; }


    }
}
