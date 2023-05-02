using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class RateFrequency
    {
        [Key]
        public int RateFrequencyId { get; set; }
        public string RateFrequencyDescription { get; set; }
    }
}