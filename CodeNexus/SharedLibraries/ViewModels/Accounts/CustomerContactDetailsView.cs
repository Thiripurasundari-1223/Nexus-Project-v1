using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Accounts
{
    public class CustomerContactDetailsView
    {
        public int CustomerContactDetailId { get; set; }
        public int AccountId { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string ContactPersonEmailAddress { get; set; }
        public string DesignationName { get; set; }
        public int? CountryId { get; set; }
        public string AddressDetail { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string Postalcode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }

    }
}
