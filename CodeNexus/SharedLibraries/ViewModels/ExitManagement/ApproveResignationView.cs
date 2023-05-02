using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ApproveResignationView
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeResignationDetailsId { get; set; }
        public DateTime RelievingDate { get; set; }
        public int LevelId { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ResignDate { get; set; }
        public string WithdrawalReason { get; set; }
        public string OverAllStatus { get; set; }
        public int? NextLevelApproverId { get; set; }
        public string ResignReason { get; set; }
        public string ApprovarName { get; set; }
        public List<ExitManagementEmailTemplate> EmailTemplateList { get; set; }
    }
}
