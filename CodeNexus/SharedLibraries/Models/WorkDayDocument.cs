using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models
{
    public class WorkDayDocument
    {

        public int WorkDayDocumentId { get; set; }
        public int WorkDayDetailId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
