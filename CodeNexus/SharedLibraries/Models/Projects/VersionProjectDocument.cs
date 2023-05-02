﻿using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class VersionProjectDocument
    {
        [Key]
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int? ProjectDocumentID { get; set; }
        //public int? ProjectID { get; set; }
        //public int? ProjectID { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
    }
}
