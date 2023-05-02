using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class UpdateEmployeeRelievingDate
    {
        public int EmployeeId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? RelievingDate { get; set; }
        public bool IsRevertRelievingDate { get; set; }
        public string ExitType { get; set; }
        public DateTime? ResignationDate { get; set; }
        public string ResignationReason { get; set; }
        public string ResignationStatus { get; set; }
        public string PersonalMobileNumber { get; set; }
        public string PersonalEmailId { get; set; }
    }
}
