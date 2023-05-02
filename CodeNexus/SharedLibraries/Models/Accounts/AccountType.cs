using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Accounts
{
    public class AccountType
    {
        [Key]
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
}