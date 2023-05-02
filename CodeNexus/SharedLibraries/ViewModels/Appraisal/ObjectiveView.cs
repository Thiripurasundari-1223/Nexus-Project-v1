using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class ObjectiveView
    {
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public string ObjectiveCode { get; set; }
        public string ObjectiveDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
