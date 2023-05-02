using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class AppraisalBUHeadComments
    {
        [Key]
        public int AppraisalBUHeadCommentsId { get; set; }
        public int AppCycle_Id { get; set; }
        public int Department_Id { get; set; }
        public int Employee_Id { get; set; }
        public string Comment { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_On { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_On { get; set; }
    }
}
