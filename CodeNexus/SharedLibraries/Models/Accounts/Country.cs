using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}