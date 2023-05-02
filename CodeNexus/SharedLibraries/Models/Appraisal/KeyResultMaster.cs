using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class KeyResultMaster
    {
        [Key]
        public int KEY_RESULT_ID { get; set; }
        public string KEY_RESULT_NAME { get; set; }
        public string KEY_RESULT_SHORT_NAME { get; set; }
        public string KEY_RESULT_DESCRIPTION { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
