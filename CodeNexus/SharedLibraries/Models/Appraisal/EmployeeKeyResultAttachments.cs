using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class EmployeeKeyResultAttachments
    {
       
        [Key]
        public int DOC_ID { get; set; }
        public int APP_CYCLE_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
        public int OBJECTIVE_ID { get; set; }
        public int KEY_RESULT_ID { get; set; }
        public string DOC_NAME { get; set; }
        public string DOC_DESCRIPTION { get; set; }
        public string DOC_URL { get; set; }
        public int DOC_UPLOADED_BY { get; set; }
        public string DOC_TYPE { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }


    }
}
