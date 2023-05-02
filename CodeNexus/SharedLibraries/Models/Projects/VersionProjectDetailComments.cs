using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class VersionProjectDetailComments
    {
        [Key]
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int? ProjectDetailCommentId { get; set; }
        //public int? ProjectId { get; set; }
        public int? ChangeRequestId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
