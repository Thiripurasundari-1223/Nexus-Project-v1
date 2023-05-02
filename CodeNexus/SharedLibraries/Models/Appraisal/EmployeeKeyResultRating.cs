using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class EmployeeKeyResultRating
    {
        [Key]
        public int APP_CYCLE_ID { get; set; }
        [Key]
        public int EMPLOYEE_ID { get; set; }
        [Key]
        public int OBJECTIVE_ID { get; set; }
        [Key]
        public int KEY_RESULT_ID { get; set; }
        public decimal? KEY_RESULT_ACTUAL_VALUE { get; set; }
        public decimal? KEY_RESULT_MAX_RATING { get; set; }
        public decimal? KEY_RESULT_RATING { get; set; }
        public int? KEY_RESULT_STATUS { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public int? IS_ADDRESSED { get; set; }
    }
}
