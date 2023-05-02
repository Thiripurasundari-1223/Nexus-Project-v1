using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class ProjectType
    {
        [Key]
        public int ProjectTypeId { get; set; }
        public string ProjectTypeDescription { get; set; }
    }
}