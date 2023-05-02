using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{

    public interface IFixedIterationVersionRepository : IBaseRepository<VersionFixedIteration>
    {

     //   ProjectDocument GetProjectDocumentByID(int projectId);

    }

    public class FixedIterationVersionRepository : BaseRepository<VersionFixedIteration>, IFixedIterationVersionRepository
    {
        private readonly PMDBContext _dbContext;
        public FixedIterationVersionRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       
    }
}
