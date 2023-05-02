using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
   public class AddNewEmployeeLeaveView
    {
        public int EmployeeID { get; set; }
        public int? EmployeeTypeID { get; set; }
        public int? DepartmentID { get; set; }
        public int? RoleID { get; set; }
        public bool? IsActive { get; set; }
        public string Gender { get; set; }
        public int? LocationID { get; set; }
        public bool? GenderMale { get; set; }
        public bool? GenderFemale { get; set; }
        public bool? GenderOther { get; set; }
        public string MaritalStatus { get; set; }
        public bool? MaritalStatusSingle { get; set; }
        public bool? MaritalStatusMarried { get; set; }
        public int? DesignationID { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfContract { get; set; }
        public int? ProbationStatusID { get; set; }
    }
}
