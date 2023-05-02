using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Notifications
{
    public class SupportingDocuments
    {
        [Key]
        public int DocumentId { get; set; }
        public int? SourceId { get; set; }
        public string DocumentCategory { get; set; }
        public bool IsApproved { get; set; }
        public string SourceType{ get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public decimal DocumentSize { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
