using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class Frequency
    {
        [Key]
        public int FrequencyId { get; set; }
        public string FrequencyDescription { get; set; }
    }
}