using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class HRCheckList
    {
        public int HRCheckListId { get; set; }
        public int? ResignationChecklistId { get; set; }
        public int? ApprovedBy { get; set; }
        public string Status { get; set; }
        public string NoticePayId { get; set; }
        public decimal? NoticePayDay { get; set; }
        public string NoticePayRemark { get; set; }
        public string ELBalanceId { get; set; }
        public decimal? ELBalanceDay { get; set; }
        public string ELBalanceRemark { get; set; }
        public string NoticePeriodWaiverRequestId { get; set; }
        public string NoticePeriodWaiverRequestRemark { get; set; }
        public string LeaveBalanceSummaryId { get; set; }
        public string LeaveBalanceSummaryRemark { get; set; }
        public string RehireEligibleId { get; set; }
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
