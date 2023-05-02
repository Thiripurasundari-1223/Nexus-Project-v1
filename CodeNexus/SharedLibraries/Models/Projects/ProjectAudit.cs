using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class ProjectAudit
    {
        [Key]
        public int ProjectAuditId { get; set; }
        public int? ChangeRequestID { get; set; }  
        public int? ProjectID { get; set; }
        public int? IterationID { get; set; }
        public string ActionType { get; set; } 
        public string Field { get; set;}
        public string OldValue { get; set;}
        public string NewValue { get; set;} 
        public string Status { get; set;}
        public string Remark { get; set; }
        public DateTime? ApprovedOn { get; set;}
        public int? ApprovedById { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
