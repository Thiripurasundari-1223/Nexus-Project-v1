using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class DepartmentMasterData
    {
        
        public List<DepartmentDetailList> DepartmentDetailLists { get; set; }
        public List<KeyValue> ParentDepartmentId { get; set; }

        public List<DepartmentEmployeeList> DepartmentHeadEmployee { get; set; }



    }
}
