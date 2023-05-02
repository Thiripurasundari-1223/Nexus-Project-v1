using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class KeyResultMasterView
    {
        public int KeyResultId { get; set; }
        public string KeyResultName { get; set; }
        public string KeyResultCode { get; set; }
        public string KeyResultDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
