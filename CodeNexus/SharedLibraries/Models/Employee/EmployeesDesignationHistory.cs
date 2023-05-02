using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class EmployeesDesignationHistory
    {
        [Key]
        public int DesignationHistoryId { get; set; }
        public int? EmployeeId { get; set; }
        public int? DesignationId { get; set; }
        public DateTime? EffiectiveFromDate { get; set; }
        public DateTime? EffiectiveToDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
