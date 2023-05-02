using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.ExitManagement
{
    public class ResignationApprovalStatus
    {
        [Key]
        public int ResignationApprovalStatusId { get; set; }
        public int? EmployeeResignationDetailsId { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public int? LevelId { get; set; }
        public string FeedBack { get; set; }
        public string Status { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ApprovalType { get; set; }
        public int? ApprovedBy { get; set; }
        public string ApproverType { get; set; }
    }
}
