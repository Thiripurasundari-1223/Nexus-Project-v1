using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class WorkHistoryView
    {
        public int WorkHistoryId { get; set; }
        public int? EmployeeId { get; set; }
        public string OrganizationName { get; set; }
        public string Designation { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ReleivingDate { get; set; }
        public decimal? LastCTC { get; set; }
        public string LeavingReason { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DocumentsToUpload serviceLetter { get; set; }
        public DocumentsToUpload paySlip { get; set; }
        public DocumentsToUpload OfferLetter { get; set; }
        public List<DocumentsToUpload> DocumentDetails { get; set; }
        public List<EmployeeDocument> WorkHistorydocuments { get; set; }

    }
}
