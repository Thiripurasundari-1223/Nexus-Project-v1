using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeListByDepartment
    {
        public List<int> EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int managerId { get; set; }
    }
}
