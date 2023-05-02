using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class VersionCustomerSPOCDetails
    {
        [Key]
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int? CustomerSPOCDetailsID { get; set; }
        //public int? ProjectID { get; set; }
        public int? CustomerSPOCId { get; set; }
        public string CustomerSPOCDetailsName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set;}
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set;}


      
    }
}
