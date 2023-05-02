using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class EmployeeObjectiveRating
    {
        [Key]
        public int APP_CYCLE_ID { get; set; }
        [Key]
        public int EMPLOYEE_ID { get; set; }
        [Key]
        public int OBJECTIVE_ID { get; set; }
        public decimal? OBJECTIVE_MAX_RATING { get; set; }
        public decimal? OBJECTIVE_RATING { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
