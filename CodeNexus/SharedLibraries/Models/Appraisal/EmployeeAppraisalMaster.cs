using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class EmployeeAppraisalMaster 
    {
        [Key]
        public int APP_CYCLE_ID { get; set; }
        [Key]
        public int EMPLOYEE_ID { get; set; }
        public int ENTITY_ID { get; set; }
        public int EMPLOYEE_ROLE_ID { get; set; }
        public int EMPLOYEE_DEPT_ID { get; set; }
        public int EMPLOYEE_MANAGER_ID { get; set; }
        public decimal? EMPLOYEE_SELF_RATING { get; set; }
        public decimal? EMPLOYEE_APPRAISER_RATING { get; set; }
        public decimal? EMPLOYEE_FINAL_RATING { get; set; }
        public int? APPRAISAL_STATUS { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public bool? IsBUHeadRevert { get; set; }
        public bool? IsRevertRating { get; set; }
        public bool? IsBUHeadApproved { get; set; }


    }
}
