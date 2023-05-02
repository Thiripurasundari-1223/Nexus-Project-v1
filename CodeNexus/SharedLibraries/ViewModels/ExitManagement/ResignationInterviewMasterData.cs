using SharedLibraries.Common;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationInterviewMasterData
    {
        public List<ResignationEmployeeMasterView> EmployeeList { get; set; }
        public List<KeyWithValue> ResignationInterview { get; set; }
        public List<KeyWithValue> ReasonRelievingPosition { get; set; }
    }
}
