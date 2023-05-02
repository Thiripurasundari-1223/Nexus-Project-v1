using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface IProjectDocumentRepository : IBaseRepository<ProjectDocument>
    {
       
        ProjectDocument GetProjectDocumentByID(int projectId);
        
    }

    public class ProjectDocumentRepository : BaseRepository<ProjectDocument>, IProjectDocumentRepository
    {
        private readonly PMDBContext _dbContext;
        public ProjectDocumentRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ProjectDocument GetProjectDocumentByID(int projectId)
        {
            if (projectId > 0)
            {
                return _dbContext.ProjectDocument.Where(r => r.ProjectID == projectId).FirstOrDefault();
            }
            return null;
        }

       
    }
}
