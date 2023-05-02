using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class CompensationDetail
    {
        [Key]
        public int CTCId { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public decimal? MonthlyCurrentCTC { get; set; }
        public decimal? MonthlyBasicPay { get; set; }
        public decimal? MonthlyHRA { get; set; }
        public decimal? MonthlySatutoryBonus{ get; set; }
        public decimal? MonthlyFBP { get; set; }
        public decimal? MonthlyGrossPay { get; set; }
        public decimal? MonthlyPF { get; set; }
        public decimal? MonthlyESI { get; set; }
        public decimal? MonthlyCBP { get; set; }
        public decimal? MonthlyCBPPercentage { get; set; }
        public decimal? AnnualCurrentCTC { get; set; }
        public decimal? AnnualBasicPay { get; set; }
        public decimal? AnnualHRA { get; set; }
        public decimal? AnnualSatutoryBonus { get; set; }
        public decimal? AnnualFBP { get; set; }
        public decimal? AnnualGrossPay { get; set; }
        public decimal? AnnualPF { get; set; }
        public decimal? AnnualESI { get; set; }
        public decimal? AnnualCBP { get; set; }
        public decimal? AnnualCBPPercentage { get; set; }
      
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
