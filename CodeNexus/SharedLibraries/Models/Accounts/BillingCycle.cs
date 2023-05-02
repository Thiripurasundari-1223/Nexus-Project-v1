using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class BillingCycle
    {
        [Key]
        public int BillingCycleId { get; set; }
        public string BillingCycleDescription { get; set; }
    }
}