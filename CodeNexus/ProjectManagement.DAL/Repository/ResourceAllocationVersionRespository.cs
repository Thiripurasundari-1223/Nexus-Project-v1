using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{


    public interface IResourceAllocationVersionRespository : IBaseRepository<VersionResourceAllocation>
    {
        void AddResourceAllocationVersion(VersionResourceAllocation versionResourceAllocation);
      
    }

    public class ResourceAllocationVersionRespository : BaseRepository<VersionResourceAllocation>, IResourceAllocationVersionRespository
    {
        private readonly PMDBContext _dbContext;
        public ResourceAllocationVersionRespository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;

        }

       
        void IResourceAllocationVersionRespository.AddResourceAllocationVersion(VersionResourceAllocation versionResourceAllocation)
        {
            _dbContext.VersionResourceAllocation.Add(versionResourceAllocation);
        }
    }
}