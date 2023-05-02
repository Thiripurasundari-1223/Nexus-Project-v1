using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SharedLibraries.Models.Employee
{
    public class EmployeeRequestList
    {
        public List<EmployeeRequest> EmployeeRequestlst { get; set; }
    }
}
