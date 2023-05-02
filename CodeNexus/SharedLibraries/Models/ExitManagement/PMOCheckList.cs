using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class PMOCheckList
    {
        public int PMOCheckListId { get; set; }
        public int? ResignationChecklistId { get; set; }
        public int? ApprovedBy { get; set; }
        public string Status { get; set; }
        public string TimesheetsId { get; set; }
        public string TimesheetsRemark { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
