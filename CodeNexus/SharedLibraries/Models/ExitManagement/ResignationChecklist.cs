using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class ResignationChecklist
    {
        public int ResignationChecklistId { get; set; }
        public int? EmployeeID { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsAgreeCheckList { get; set; }
        public string ManagerStatus { get; set; }
        public string PMOStatus { get; set; }
        public string ITStatus { get; set; }
        public string AdminStatus { get; set; }
        public string FinanceStatus { get; set; }
        public string HRStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ResignationDetailsId { get; set; }
    }
}
