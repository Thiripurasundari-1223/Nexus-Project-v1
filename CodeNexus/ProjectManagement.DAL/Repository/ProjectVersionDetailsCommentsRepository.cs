using ProjectManagement.DAL.DBContext;
using ProjectManagement.DAL.Repository;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface IProjectVersionDetailsCommentsRepository : IBaseRepository<VersionProjectDetailComments>
    {
      
    }

    public class ProjectVersionDetailsCommentsRepository : BaseRepository<VersionProjectDetailComments>, IProjectVersionDetailsCommentsRepository
{
    private readonly PMDBContext _dbContext;
    public ProjectVersionDetailsCommentsRepository(PMDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

   
}
}