using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Appraisal.DAL.Repository
{
    public interface IAppConstantTypeRepository : IBaseRepository<AppConstantType>
    {
        List<AppConstantType> GetAllAppConstantType();
    }
    public class AppConstantTypeRepository : BaseRepository<AppConstantType>, IAppConstantTypeRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppConstantTypeRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<AppConstantType> GetAllAppConstantType()
        {
            return dbContext.AppConstantType.ToList();
        }
    }
}
