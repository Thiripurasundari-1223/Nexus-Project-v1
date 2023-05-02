using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class DepartmentDetailList
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentShortName { get; set; }
        public string DepartmentDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsEnableBUAccountable { get; set; }

        public int? ParentDepartmentId { get; set; }

        public int? DepartmentHeadEmployeeId { get; set; }

        public int DepartmentCount { get; set; }
       
    }
}
