using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AccountInput
    {
        public int EmployeeId { get; set; }
        public int PageNumber { get; set; }
        public int NoOfRecord { get; set; }
        public bool IsAllAccount { get; set; } = false;
        public string AccountName { get; set; }
        public string AccountStatus { get; set; }
        public string AccountNameSortBy { get; set; }
        public int AccountManagerId { get; set; }

        public int AccountTypeId{get;set;}

        public int CountryId { get; set; }

        public List<int> EmployeeList { get; set; }
        public List<string> ManagementRole { get; set; }

        public string RoleName { get; set; }
        public bool IsDraft { get; set; }
    }
}
