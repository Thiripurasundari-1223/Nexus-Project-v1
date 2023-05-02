using SharedLibraries.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Accounts
{
    public class CountryAndState
    {
        public List<Country> CountryList { get; set; }
        public List<State> StateList { get; set; }
    }
    public class CountryNameAndStateName
    {
        public Country Country { get; set; }
        public State State { get; set; }
    }
}
