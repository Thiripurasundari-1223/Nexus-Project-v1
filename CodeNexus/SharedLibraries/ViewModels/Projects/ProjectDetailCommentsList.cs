using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class ProjectDetailCommentsList
    {
        [Key]
        public int ProjectDetailCommentId { get; set; }
        public int? ProjectDetailId { get; set; }
        public int? ChangeRequestId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}