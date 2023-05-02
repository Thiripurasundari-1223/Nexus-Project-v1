using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace IAM.DAL.Repository
{
    public interface IEmployeeDesignationHistoryRepository : IBaseRepository<EmployeesDesignationHistory>
    {
        EmployeesDesignationHistory CheckDesignationHistoryByEmployeeId(int? employeeId, int? designationId, DateTime? effectiveFromDate);
        List<EmployeesDesignationHistory> GetEmployeeDesignationHistoeryByEmployeeId(int? employeeId);
        EmployeesDesignationHistory GetPreviousDesignationHistoryByEmployeeId(int? employeeId, int? designationHistoryId);
        List<EmployeesDesignationHistory> GetEmployeeDesignationHistoryByEffectiveDate(DateTime date);
    }
    public class EmployeeDesignationHistoryRepository : BaseRepository<EmployeesDesignationHistory>, IEmployeeDesignationHistoryRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeDesignationHistoryRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeesDesignationHistory CheckDesignationHistoryByEmployeeId(int? employeeId, int? designationId, DateTime? effectiveFromDate)
        {
            string query = "EmployeeId == " + employeeId + " && DesignationId == " + designationId;
            if (effectiveFromDate != null)
            {
                query = query + " && EffiectiveFromDate.Value.Date== @0";
            }
            EmployeesDesignationHistory employeesDesignationHistory = dbContext.EmployeesDesignationHistory.Where(query, effectiveFromDate == null ? null : effectiveFromDate.Value.Date).OrderBy(x => x.DesignationHistoryId).LastOrDefault();
            return employeesDesignationHistory;
        }
        public List<EmployeesDesignationHistory> GetEmployeeDesignationHistoeryByEmployeeId(int? employeeId)
        {
            return dbContext.EmployeesDesignationHistory.Where(x => x.EmployeeId == employeeId).OrderByDescending(x => x.CreatedBy).ToList();
        }
        public EmployeesDesignationHistory GetPreviousDesignationHistoryByEmployeeId(int? employeeId, int? designationHistoryId)
        {
            return dbContext.EmployeesDesignationHistory.Where(x => x.EmployeeId == employeeId && x.DesignationHistoryId < designationHistoryId).OrderByDescending(x => x.DesignationHistoryId).FirstOrDefault();
        }
        public List<EmployeesDesignationHistory> GetEmployeeDesignationHistoryByEffectiveDate(DateTime date)
        {
            return dbContext.EmployeesDesignationHistory.Where(x => x.EffiectiveFromDate != null && x.EffiectiveFromDate.Value.Date == date.Date).OrderBy(x => x.DesignationHistoryId).ToList();
        }
    }
}
