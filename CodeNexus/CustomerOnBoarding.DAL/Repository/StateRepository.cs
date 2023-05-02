using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface IStateRepository : IBaseRepository<State>
    {
        State GetByName(string pState, int pStateId = 0);
        State GetByID(int pStateId);
        List<State> GetAllState();
        List<State> GetAllStateByCountryId(int CountryId);
    }
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        private readonly COBDBContext dbContext;
        public StateRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public State GetByName(string pState, int pStateId = 0)
        {
            if (pStateId > 0)
            {
                return dbContext.State.Where(x => x.StateName == pState && x.StateId == pStateId).FirstOrDefault();
            }
            return dbContext.State.Where(x => x.StateId == pStateId).FirstOrDefault();
        }
        public State GetByID(int pStateId)
        {
            return dbContext.State.Where(x => x.StateId == pStateId).FirstOrDefault();
        }
        public List<State> GetAllState()
        {
            return dbContext.State.ToList();
        }
        public List<State> GetAllStateByCountryId(int CountryId)
        {
            return dbContext.State.Where(x => x.CountryId == CountryId).ToList();
        }
    }
}