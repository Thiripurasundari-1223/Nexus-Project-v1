using SharedLibraries.Models.Employee;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationMasterData
    {
        public  List<ResignationEmployeeMasterView> employeeList { get; set; }
        public List<ResignationReason> ResignationReasonList { get; set; }
        public CountryAndState CountryAndStateList { get; set; }
    }

    public class NotificationMasterData
    {
        public List<int> EmployeeList { get; set; }
        public List<ResignationChecklist> ChecklistEmployeeList { get; set; }
        public ExitManagementEmailTemplate EmailTemplate { get; set; }
    }
}
