using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class CustomerContactDetails
    {
        [Key]
        public int CustomerContactDetailId { get; set; }
        public int AccountId { get; set; }
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
