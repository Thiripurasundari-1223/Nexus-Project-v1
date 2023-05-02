using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class AppConstants
    {
        [Key]
        public int AppConstantId { get; set; }
        public string AppConstantType { get; set; }
        public string DisplayName { get; set;}
        public string AppConstantValue { get; set;}  
    }
}
