using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class Allocation
    {
        [Key]
        public int AllocationId { get; set; }
        public string AllocationDescription { get; set; }
    }
}