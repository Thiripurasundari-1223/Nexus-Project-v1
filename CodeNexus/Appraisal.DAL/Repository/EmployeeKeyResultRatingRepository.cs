using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IEmployeeKeyResultsRatingRepository : IBaseRepository<EmployeeKeyResultRating>
    {
       public List<EmployeeKeyResultRating> GetAllObjectiveKeyResults(int appCycleId,int employeeId);
       public EmployeeKeyResultRating GetKeyResultdetail(int appCycleId, int employeeId, int ObjectiveId, int KRAId);
       public List<EmployeeKeyResultRating> GetKeyResults(int appCycleId, int employeeId, int ObjectiveId);
    }
    public class EmployeeKeyResultsRatingRepository : BaseRepository<EmployeeKeyResultRating>, IEmployeeKeyResultsRatingRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmployeeKeyResultsRatingRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<EmployeeKeyResultRating> GetAllObjectiveKeyResults(int appCycleId, int employeeId)
        {
            return dbContext.EmployeeKeyResultRating.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId).ToList();              
        }
        public EmployeeKeyResultRating GetKeyResultdetail(int appCycleId, int employeeId, int ObjectiveId, int KRAId)
        {
            return dbContext.EmployeeKeyResultRating.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == ObjectiveId && x.KEY_RESULT_ID == KRAId).FirstOrDefault();
        }
        public List<EmployeeKeyResultRating> GetKeyResults(int appCycleId, int employeeId, int ObjectiveId)
        {
            return dbContext.EmployeeKeyResultRating.Where(x => x.APP_CYCLE_ID == appCycleId && x.EMPLOYEE_ID == employeeId && x.OBJECTIVE_ID == ObjectiveId).ToList();
        }
    }
}
