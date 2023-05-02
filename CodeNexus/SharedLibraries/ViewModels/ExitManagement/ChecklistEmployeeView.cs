using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ChecklistEmployeeView
    {
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }       
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string FormattedEmployeeID { get; set; }
        public string Status { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ResignationDetailsId { get; set; }
        public int ResignationChecklistId { get; set; }

        public string IsActive { get; set; }
        public string RoleName { get; set; }
        public string ManagerStatus { get; set; }
        public string ITStatus { get; set; }
        public string AdminStatus { get; set; }
        public string FinanceStatus { get; set; }
        public string HRStatus { get; set; }
        public bool? IsAgreeCheckList { get; set; }
    }
}
