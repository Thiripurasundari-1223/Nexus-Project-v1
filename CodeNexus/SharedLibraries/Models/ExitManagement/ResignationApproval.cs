using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SharedLibraries.Common;

namespace SharedLibraries.Models.ExitManagement
{
    public class ApprovalConfiguration
    {
        public List<KeyWithValue> ApprovalType { get; set; }
        public List<ResignationApproval> ResignationApproval { get; set; }
    }
    public class ResignationApproval
    {
        [Key]
        public int ResignationApprovalId { get; set; }
        public int? LevelId { get; set; }
        public int? LevelApprovalId { get; set; }
        public int? LevelApprovalEmployeeId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
   
}
