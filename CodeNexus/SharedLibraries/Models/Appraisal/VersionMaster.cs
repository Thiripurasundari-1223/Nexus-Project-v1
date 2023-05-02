using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class VersionMaster
    {
        [Key]
        public int VERSION_ID { get; set; }
        public string VERSION_NAME { get; set; }
        public string VERSION_CODE { get; set; }
        public string VERSION_DESC { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}