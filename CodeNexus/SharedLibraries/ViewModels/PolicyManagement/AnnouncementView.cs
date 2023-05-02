using System;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class AnnouncementView
    {
        public int AnnouncementId { get; set; }
        public string Topic { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
    }
}