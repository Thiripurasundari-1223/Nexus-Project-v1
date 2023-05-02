using System;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class DocumentTagView
    {
        public int DocumentTagId { get; set; }
        public string TagName { get; set; }
        public string PlaceHolderName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
    }
}