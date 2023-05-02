using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels
{
    public class RejectionReason
    {
        [Key]
        public int RejectionReasonId { get; set; }
        public string ReasonForRejection { get; set; }
    }
}