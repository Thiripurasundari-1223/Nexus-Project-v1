using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IStateRepository : IBaseRepository<EmployeeState>
    {
        List<EmployeeState> GetAllState();
        string GetStateNameById(int stateId);
    }
    public class StateRepository : BaseRepository<EmployeeState>, IStateRepository
    {
        private readonly IAMDBContext dbContext;
        public StateRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<EmployeeState> GetAllState()
        {
            return dbContext.EmployeeState.ToList();
        }
        public string GetStateNameById(int stateId)
        {
            return dbContext.EmployeeState.Where(x => x.StateId == stateId).Select(x => x.StateName).FirstOrDefault();
        }
    }
}
