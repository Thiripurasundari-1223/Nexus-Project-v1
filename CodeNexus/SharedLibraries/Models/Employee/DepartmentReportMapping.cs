using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class DepartmentReportMapping
    {
        [Key]
        public int DepartmentReportMappingId { get; set; }
        public int DepartmentId { get; set; }
        public int EmployeeCategoryId { get; set; }
        public int ReportId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
