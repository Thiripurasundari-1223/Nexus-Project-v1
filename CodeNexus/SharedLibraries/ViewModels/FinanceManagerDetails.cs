using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels
{
    public class FinanceManagerDetails
    {
        public int FinanceId { get; set; }
        public string FinanceName { get; set; }
        public string FinanceEmail { get; set; }
        public string CreatedByEmailId { get; set; }
        public string AccountManagerEmailId { get; set; }
    }
}
