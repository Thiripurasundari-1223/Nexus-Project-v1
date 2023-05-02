using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class ImportExcelView
    {
        public string Base64Format { get; set; }
        public List<Department> Department { get; set; }
        public List<Roles> Roles { get; set; }
        public List<EmployeesTypes> EmployeesTypes { get; set; }
    }
}
