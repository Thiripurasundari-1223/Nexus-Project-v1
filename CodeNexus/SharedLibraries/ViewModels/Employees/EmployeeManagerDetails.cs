using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeManagerDetails
    {
        public int ManagerID { get; set; }  

        public string ManagerName { get; set; }
        public string  EmailAddress { get; set; }    
    }
}
