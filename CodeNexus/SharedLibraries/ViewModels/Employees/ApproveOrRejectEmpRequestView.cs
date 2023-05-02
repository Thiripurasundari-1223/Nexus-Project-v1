using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SharedLibraries.ViewModels.Employee
{
    public class ApproveOrRejectEmpRequestView
    {
        [Key]
        public int EmployeeRequestId { get; set; }
        public int EmployeeId { get; set; }
        public string RequestCategory { get; set; }
        public Guid ChangeRequestId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public string Remarks { get; set; }
        public string RemarkId { get; set; }

    }
}
