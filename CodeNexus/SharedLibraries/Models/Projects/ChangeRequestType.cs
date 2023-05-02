using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class ChangeRequestType
    {
        [Key]
        public int ChangeRequestTypeId { get; set; }
        public string ChangeRequestTypeDescription { get; set; }
    }
}