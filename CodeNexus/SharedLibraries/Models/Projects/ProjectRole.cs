using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class ProjectRole
    {
        [Key]
        public int ProjectRoleID { get; set; }
        public string ProjectRoleName { get; set; }
    }
}
