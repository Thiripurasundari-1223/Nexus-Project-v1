using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class EmployeeAppraisalConversation
    {
        [Key]
        public int COMMENT_ID { get; set; }
        public int? APP_CYCLE_ID { get; set; }
        public int? EMPLOYEE_ID { get; set; }
        public string COMMENT { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
