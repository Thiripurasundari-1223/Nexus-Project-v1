using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class WorkDay
    {
        [Key]
        public int WorkDayId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }      
        public DateTime WorkDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}