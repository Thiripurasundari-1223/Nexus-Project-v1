using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Accounts
{

    public class VersionCustomerContactDetails
    {
        [Key]
        public int VersionCustomerContactDetailId { get; set; }
        public int VersionId { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string ContactPersonEmailAddress { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string DesignationName { get; set; }
        public int? CountryId { get; set; }
        public string AddressDetail { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string Postalcode { get; set; }
    }
}
