using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeorganizationchartDetailsView
    {
       
        public String name { get; set; }
        public string title { get; set; }
        
        public String image { get; set; }
        public string cssClass { get; set; }
        public List<EmployeeorganizationchartDetailsView> childs { get; set; }
    }
    public class EmployeeorganizationcharView
    {
        public EmployeeorganizationchartDetailsView OrgnizationData { get; set; }
        public string SelectedEmpDepartment { get; set; }
        public string SelectedEmpEmailId { get; set; }
    }
}
