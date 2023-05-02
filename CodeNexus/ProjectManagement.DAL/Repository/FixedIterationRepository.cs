using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface IFixedIterationRepository : IBaseRepository<FixedIteration>
    {
        FixedIteration GetFixedIterationByID(int IterationID, int projectID);
       

      //  FixedIteration InsertAllIterations(int projectId);
    }

    public class FixedIterationRepository : BaseRepository<FixedIteration>, IFixedIterationRepository
    {
        private readonly PMDBContext _dbContext;
        public FixedIterationRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public FixedIteration GetFixedIterationByID(int IterationID, int projectID)
        {
            if (IterationID > 0)
            {
                return _dbContext.FixedIteration.Where(r => r.IterationID == IterationID && r.ProjectID == projectID).FirstOrDefault();
            }
            return null;
        }

        
    }
}