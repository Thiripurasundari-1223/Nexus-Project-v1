using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Accounts
{
    public class ImportExcelView
    {
        public string Base64Format { get; set; }
        public List<EmployeeDetail> EmployeeDetails { get; set; }
    }
}
