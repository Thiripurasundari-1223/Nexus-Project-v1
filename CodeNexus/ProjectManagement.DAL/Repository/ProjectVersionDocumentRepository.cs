using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;

namespace ProjectManagement.DAL.Repository
{


    public interface IProjectVersionDocumentRepository : IBaseRepository<VersionProjectDocument>
    {
        

    }

    public class ProjectVersionDocumentRepository : BaseRepository<VersionProjectDocument>, IProjectVersionDocumentRepository
    {
        private readonly PMDBContext _dbContext;
        public ProjectVersionDocumentRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }

}
