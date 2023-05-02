using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedLibraries.ViewModels.Notifications;

namespace SharedLibraries.ViewModels.Employee
{
    [Table("EmployeeRequestDetails")]
    public class EmployeeRequestDetailsView
    {
        [Key]
        public int EmployeeRequestDetailId { get; set; }
        public Guid ChangeRequestId { get; set; }
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DocumentsToUpload requestProof { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? sourceId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
