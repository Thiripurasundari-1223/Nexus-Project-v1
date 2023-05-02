using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels.Projects
{
    public class CustomerSPOCDetailsList
    {
        public int CustomerSPOCDetailsID { get; set; }
        public int? ProjectID { get; set; }
        public int? CustomerSPOCId { get; set; }
        public string CustomerSPOCDetailsName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
