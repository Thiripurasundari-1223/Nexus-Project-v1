using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Notifications
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
    }
}