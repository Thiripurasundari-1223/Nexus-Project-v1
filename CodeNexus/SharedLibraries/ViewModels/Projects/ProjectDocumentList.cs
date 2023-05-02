using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Projects
{
    public class ProjectDocumentList
    {
        [Key]
        public int ProjectDocumentID { get; set; }
        public int? ProjectID { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
