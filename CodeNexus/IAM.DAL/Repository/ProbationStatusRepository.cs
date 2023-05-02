using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAM.DAL.Repository
{
    public interface IProbationStatusRepository : IBaseRepository<ProbationStatus>
    {
        List<ProbationStatus> GetProbationStatusList();
        string GetProbationStatusNameById(int? probationStatusId);
    }
    public class ProbationStatusRepository : BaseRepository<ProbationStatus>, IProbationStatusRepository
    {
        private readonly IAMDBContext dbContext;
        public ProbationStatusRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<ProbationStatus> GetProbationStatusList()
        {
            return dbContext.ProbationStatus.ToList();
        }

        public string GetProbationStatusNameById(int? probationStatusId)
        {
            return dbContext.ProbationStatus.Where(x => x.ProbationStatusId == probationStatusId).Select(x => x.ProbationStatusName).FirstOrDefault();
        }
    }
}
