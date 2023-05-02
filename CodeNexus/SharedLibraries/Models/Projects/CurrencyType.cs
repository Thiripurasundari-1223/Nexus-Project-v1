using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class CurrencyType
    {
        [Key]
        public int CurrencyTypeId { get; set; }
        public string CurrencyCode { get; set; }
    }
}