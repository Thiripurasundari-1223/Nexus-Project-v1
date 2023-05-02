using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeAppraisalMasterDetailView
    {
        public int APP_CYCLE_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public string FORMATTED_EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_EMAILADDRESS { get; set; }
        public int ENTITY_ID { get; set; }
        public string ENTITY_NAME { get; set; }
        public string ENTITY_SHORT_NAME { get; set; }
        public int EMPLOYEE_ROLE_ID { get; set; }
        public string EMPLOYEE_ROLE_NAME { get; set; }
        public int EMPLOYEE_DEPT_ID { get; set; }
        public string EMPLOYEE_DEPT_NAME { get; set; }
        public int EMPLOYEE_MANAGER_ID { get; set; }
        public string EMPLOYEE_MANAGER_NAME { get; set; }
        public decimal? EMPLOYEE_SELF_RATING { get; set; }
        public decimal? EMPLOYEE_APPRAISER_RATING { get; set; }
        public decimal? EMPLOYEE_FINAL_RATING { get; set; }
        public int? APPRAISAL_STATUS { get; set; }
        public string APPRAISAL_STATUS_NAME { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public bool? IsBUHeadRevert { get; set; }
        public bool? IsRevertRating { get; set; }
        public bool? IsBuheadApproved { get; set; }
    }
}
