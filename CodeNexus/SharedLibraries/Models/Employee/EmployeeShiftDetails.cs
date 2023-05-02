using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeShiftDetails
    {
        [Key]
        public int EmployeeShiftDetailsId { get; set; }
        public int? EmployeeID { get; set; }
        public int? ShiftDetailsId { get; set; }
        public DateTime? ShiftFromDate { get; set; }
        public DateTime? ShiftToDate{ get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
