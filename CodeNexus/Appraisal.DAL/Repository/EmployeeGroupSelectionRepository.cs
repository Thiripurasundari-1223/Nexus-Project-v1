using System;
using System.Collections.Generic;
using System.Text;
using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System.Linq;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{

    public interface IEmployeeGroupSelectionRepository : IBaseRepository<EmployeeGroupSelection>
    {

        EmployeeGroupSelection GetEmployeeGroupSelectionbyID (int appCycleID, int employeeID, int ObjectiveID,int KraID,int KeyResultsGroupID);
        List<EmployeeGroupSelection> GetEmployeeGroupSelection(int appCycleID, int employeeID);

    }


    public class EmployeeGroupSelectionRepository :BaseRepository<EmployeeGroupSelection>,IEmployeeGroupSelectionRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmployeeGroupSelectionRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeGroupSelection GetEmployeeGroupSelectionbyID(int appCycleID, int employeeID, int ObjectiveID, int KRAID, int KeyResultsGroupID)
        {
            return dbContext.EmployeeGroupSelection.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_ID == employeeID && x.OBJECTIVE_ID == ObjectiveID && x.KEY_RESULT_ID == KRAID && x.KEY_RESULTS_GROUP_ID == KeyResultsGroupID).FirstOrDefault();
        }
        public List<EmployeeGroupSelection> GetEmployeeGroupSelection(int appCycleID, int employeeID)
        {
            return dbContext.EmployeeGroupSelection.Where(x => x.APP_CYCLE_ID == appCycleID && x.EMPLOYEE_ID == employeeID).ToList();
        }
    }
}
