using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface ICompensationDetailRepository : IBaseRepository<CompensationDetail>
    {
        CompensationDetail GetCompendationDetailByCTCId(int CTCId);
        List<CompensationDetail> GetCompensationDetailByEmployeeId(int employeeId);
        List<CompensationDetailView> GetCompensationDetailViewsByEmployeeId(EmployeeCompensationCompareView compensationCompareView);
        CompensationDetail GetCompensationDetailByEffectiveFromDate(int employeeId, DateTime effectiveFromDate);
    }
    public class CompensationDetailRepository : BaseRepository<CompensationDetail>, ICompensationDetailRepository
    {
        private readonly IAMDBContext dbContext;
        public CompensationDetailRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public CompensationDetail GetCompendationDetailByCTCId(int CTCId)
        {
            return dbContext.CompensationDetail.Where(x => x.CTCId == CTCId).FirstOrDefault();
        }

        public List<CompensationDetail> GetCompensationDetailByEmployeeId(int employeeId)
        {
            return dbContext.CompensationDetail.Where(x => x.EmployeeID == employeeId).ToList();
        }
        public List<CompensationDetailView> GetCompensationDetailViewsByEmployeeId(EmployeeCompensationCompareView compensationCompareView)
        {

            if (compensationCompareView.IsCompareMode == true && compensationCompareView.Year != 0 )
            {
                DateTime firstDay = new DateTime(compensationCompareView.Year, 1, 1);
                DateTime lastDay = new DateTime(compensationCompareView.Year, 12, 31);
                return dbContext.CompensationDetail.Where(x => x.EmployeeID == compensationCompareView.EmployeeId && x.EffectiveFromDate >= firstDay && x.EffectiveFromDate <= lastDay).Select(x =>
                new CompensationDetailView
                {
                    EffectiveFromDate = x.EffectiveFromDate,
                    CTCId = x.CTCId,
                    EmployeeID = x.EmployeeID,
                    MonthlyCurrentCTC = x.MonthlyCurrentCTC,
                    MonthlyBasicPay = x.MonthlyBasicPay,
                    MonthlyHRA = x.MonthlyHRA,
                    MonthlySatutoryBonus = x.MonthlySatutoryBonus,
                    MonthlyFBP = x.MonthlyFBP,
                    MonthlyGrossPay = x.MonthlyGrossPay,
                    MonthlyPF = x.MonthlyPF,
                    MonthlyESI = x.MonthlyESI,
                    MonthlyCBP = x.MonthlyCBP,
                    MonthlyCBPPercentage = x.MonthlyCBPPercentage,
                    AnnualCurrentCTC = x.AnnualCurrentCTC,
                    AnnualBasicPay = x.AnnualBasicPay,
                    AnnualHRA = x.AnnualHRA,
                    AnnualSatutoryBonus = x.AnnualSatutoryBonus,
                    AnnualFBP = x.AnnualFBP,
                    AnnualGrossPay = x.AnnualGrossPay,
                    AnnualPF = x.AnnualPF,
                    AnnualESI = x.AnnualESI,
                    AnnualCBP = x.AnnualCBP,
                    AnnualCBPPercentage = x.AnnualCBPPercentage,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn
                }).OrderByDescending(x=>x.EffectiveFromDate).ToList();
            }
            else if(compensationCompareView.IsCompareMode == true && compensationCompareView.Year ==0)
            {
                List<CompensationDetailView> compensationDetails = new List<CompensationDetailView>(); 
                CompensationDetailView detailView = dbContext.CompensationDetail.Where(x => x.EmployeeID == compensationCompareView.EmployeeId).Select(x =>
               new CompensationDetailView
               {
                   EffectiveFromDate = x.EffectiveFromDate,
                   CTCId = x.CTCId,
                   EmployeeID = x.EmployeeID,
                   MonthlyCurrentCTC = x.MonthlyCurrentCTC,
                   MonthlyBasicPay = x.MonthlyBasicPay,
                   MonthlyHRA = x.MonthlyHRA,
                   MonthlySatutoryBonus = x.MonthlySatutoryBonus,
                   MonthlyFBP = x.MonthlyFBP,
                   MonthlyGrossPay = x.MonthlyGrossPay,
                   MonthlyPF = x.MonthlyPF,
                   MonthlyESI = x.MonthlyESI,
                   MonthlyCBP = x.MonthlyCBP,
                   MonthlyCBPPercentage = x.MonthlyCBPPercentage,
                   AnnualCurrentCTC = x.AnnualCurrentCTC,
                   AnnualBasicPay = x.AnnualBasicPay,
                   AnnualHRA = x.AnnualHRA,
                   AnnualSatutoryBonus = x.AnnualSatutoryBonus,
                   AnnualFBP = x.AnnualFBP,
                   AnnualGrossPay = x.AnnualGrossPay,
                   AnnualPF = x.AnnualPF,
                   AnnualESI = x.AnnualESI,
                   AnnualCBP = x.AnnualCBP,
                   AnnualCBPPercentage = x.AnnualCBPPercentage,
                   CreatedOn = x.CreatedOn,
                   CreatedBy = x.CreatedBy,
                   ModifiedBy = x.ModifiedBy,
                   ModifiedOn = x.ModifiedOn
               }).OrderByDescending(x=>x.EffectiveFromDate).FirstOrDefault();
                compensationDetails.Add(detailView);
                return compensationDetails;
            }
            else
            {
                return dbContext.CompensationDetail.Where(x => x.EmployeeID == compensationCompareView.EmployeeId).Select(x =>
               new CompensationDetailView
               {
                   EffectiveFromDate = x.EffectiveFromDate,
                   CTCId = x.CTCId,
                   EmployeeID = x.EmployeeID,
                   MonthlyCurrentCTC = x.MonthlyCurrentCTC,
                   MonthlyBasicPay = x.MonthlyBasicPay,
                   MonthlyHRA = x.MonthlyHRA,
                   MonthlySatutoryBonus = x.MonthlySatutoryBonus,
                   MonthlyFBP = x.MonthlyFBP,
                   MonthlyGrossPay = x.MonthlyGrossPay,
                   MonthlyPF = x.MonthlyPF,
                   MonthlyESI = x.MonthlyESI,
                   MonthlyCBP = x.MonthlyCBP,
                   MonthlyCBPPercentage = x.MonthlyCBPPercentage,
                   AnnualCurrentCTC = x.AnnualCurrentCTC,
                   AnnualBasicPay = x.AnnualBasicPay,
                   AnnualHRA = x.AnnualHRA,
                   AnnualSatutoryBonus = x.AnnualSatutoryBonus,
                   AnnualFBP = x.AnnualFBP,
                   AnnualGrossPay = x.AnnualGrossPay,
                   AnnualPF = x.AnnualPF,
                   AnnualESI = x.AnnualESI,
                   AnnualCBP = x.AnnualCBP,
                   AnnualCBPPercentage = x.AnnualCBPPercentage,
                   CreatedOn = x.CreatedOn,
                   CreatedBy = x.CreatedBy,
                   ModifiedBy = x.ModifiedBy,
                   ModifiedOn = x.ModifiedOn
               }).OrderByDescending(x=>x.EffectiveFromDate).ToList();
            }

        }
        public CompensationDetail GetCompensationDetailByEffectiveFromDate(int employeeId , DateTime effectiveFromDate)
        {
            return dbContext.CompensationDetail.Where(x => x.EmployeeID == employeeId && x.EffectiveFromDate == effectiveFromDate).FirstOrDefault();
        }
    }
}
