using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Appraisal
{
    public class EntityMaster
    {
        [Key]
        public int ENTITY_ID { get; set; }
        public string ENTITY_NAME { get; set; }
        public string ENTITY_SHORT_NAME { get; set; }
        public string ENTITY_DESCRIPTION { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}