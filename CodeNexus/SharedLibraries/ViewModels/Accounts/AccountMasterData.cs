using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Employees;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Accounts
{
    public class AccountMasterData
    {
        public List<BillingCycle> BillingCycleList { get; set; }
        public List<AccountType> AccountTypeList { get; set; }
        public List<Country> CountryList { get; set; }
        public List<State> StateList { get; set; }
        public List<EmployeeList> EmployeeList { get; set; }
        public List<StringKeyWithValue> AccountStatusList { get; set; }
        public List<KeyWithValue> AccountFlowStatusList { get; set; }
        public List<KeyWithValue> AccountNameList { get; set; }
        public List<StringKeyWithValue> AccountManagerList { get; set; }
    }
}