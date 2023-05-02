using SharedLibraries.Common;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationChecklistMasterData
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }        
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string FormattedEmployeeID { get; set; }
        public string EmployeeType { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public List<ResignationEmployeeMasterView> EmployeeList { get; set; }
        public List<KeyWithValue> ExitCheckListLetter { get; set; }
        public List<KeyWithValue> ExitCheckList { get; set; }
        public List<KeyWithValue> ManagerMailData { get; set; }
        public List<KeyWithValue> ITMailData { get; set; }
        public List<KeyWithValue> ELPayData { get; set; }
        public List<KeyWithValue> CheckListSubmission { get; set; }
        public List<KeyWithValue> NoticePayData { get; set; }
        public List<CheckListView> CheckListView { get; set; }
        public List<StringKeyWithValue> CheckListEdit { get; set; }
    }
}
