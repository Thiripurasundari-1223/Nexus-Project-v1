using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationChecklistView
    {
        public int ResignationChecklistId { get; set; }
        public int EmployeeID { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsAgreeCheckList { get; set; }
        public bool? IsSubmit { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ResignDate { get; set; }
        public string Role { get; set; }
        public List<string> CheckListView { get; set; }
        public ManagerCheckList ManagerCheckList { get; set; }
        public PMOCheckList PMOCheckList { get; set; }
        public ITCheckList ITCheckList { get; set; }
        public AdminCheckList AdminCheckList { get; set; }
        public FinanceCheckList FinanceCheckList { get; set; }
        public HRCheckList HRCheckList { get; set; }
        public ResignationChecklist ResignationChecklist { get; set; }
        public ExitManagementEmailTemplate ExitCheckListTemplate { get; set; }
        public bool IsManagerSubmited { get; set; }
        public bool IsPMOSubmited { get; set; }
        public bool IsITSubmited { get; set; }
        public bool IsAdminSubmited { get; set; }
        public bool IsFinanceSubmited { get; set; }
        public string CheckListType { get; set; }
        public EmployeeResignationChecklistDetailsView employeeResignationChecklistDetails { get; set; }

    }
}
