using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AccountNames
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
    }
    public class ProjectListWithTimesheet
    {
        public int ProjectId { get; set; }
        public string TimesheetStatus { get; set; }
    }
}
