using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationApprovalStatusView
    {
        public int ResignationApprovalStatusId { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public string ApproverEmployeeName { get; set; }
        public string ApproverType { get; set; }
        public int? LevelId { get; set; }
        public string FeedBack { get; set; }
        public string Status { get; set; }
        public string ApprovalType { get; set; }
        public int? ApproveById{ get; set; }
        public string ApprovedByEmployeeName { get; set; }


    }
}
