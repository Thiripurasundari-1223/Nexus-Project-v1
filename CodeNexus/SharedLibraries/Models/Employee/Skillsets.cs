using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    [Table("Skillset")]
    public class Skillsets
    {
        [Key]
        public int SkillsetId { get; set; }
        public string Skillset { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? SkillsetCategoryId { get; set; }
    }
}
