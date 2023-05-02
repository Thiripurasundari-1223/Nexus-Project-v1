using System;
using System.Collections.Generic;
using System.Text;
using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System.Linq;
using SharedLibraries.ViewModels.Appraisal;


namespace Appraisal.DAL.Repository
{
    public interface IEmployeeGroupRatingRepository : IBaseRepository<EmployeeGroupRating>
    {

        EmployeeGroupRating GetEmployeeGroupRatingbyID(int appCycleID, int employeeID, int ObjectiveID, int KeyResultsGroupID);
        List<EmployeeGroupRating> GetEmployeeGroupRating(int appCycleID, int employeeID);

    }
    public class EmployeeGroupRatingRepository : BaseRepository<EmployeeGroupRating>, IEmployeeGroupRatingRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmployeeGroupRatingRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeGroupRating GetEmployeeGroupRatingbyID(int appCycleID, int employeeID, int ObjectiveID, int KeyResultsGroupID)
        {
            return dbContext.EmployeeGroupRating.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_ID == employeeID && x.OBJECTIVE_ID == ObjectiveID && x.KEY_RESULTS_GROUP_ID == KeyResultsGroupID).FirstOrDefault();
        }
        public List<EmployeeGroupRating> GetEmployeeGroupRating(int appCycleID, int employeeID)
        {
            return dbContext.EmployeeGroupRating.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_ID == employeeID).ToList();
        }
    }
}
