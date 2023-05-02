using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class ProjectDocument
    {
        [Key]
        public int ProjectDocumentID { get; set; }
        public int? ProjectID { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get;set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set;}
    }
}
