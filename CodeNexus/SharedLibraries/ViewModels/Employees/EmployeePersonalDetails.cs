using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeePersonalDetails
    {
        public int EmployeeID { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? WeddingAnniversary { get; set; }
    }
}
