using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
   public class AccountReport
    {
        public int AccountId { get; set; }
        public string FormattedAccountId { get; set; }
        public string AccountName { get; set; }
        public int ProjectCount { get; set; }
        public string OwnerName { get; set; }
        public string ContactPersonName { get; set; }
        public string AccountStatus { get; set; }
    }
}
