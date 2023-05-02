using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class ObjectiveMaster
    {
        [Key]
        public int OBJECTIVE_ID { get; set; }
        public string OBJECTIVE_NAME { get; set; }
        public string OBJECTIVE_SHORT_NAME { get; set; }
        public string OBJECTIVE_DESCRIPTION { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
