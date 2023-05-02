using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class CustomerSPOC
    {
        [Key]
        public int CustomerSPOCId { get; set; }
        public string CustomerSPOCName { get; set; }
    }
}
