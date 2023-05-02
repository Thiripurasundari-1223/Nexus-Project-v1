using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
  
    public interface IResignationReasonRepository : IBaseRepository<ResignationReasonRepository>
    {
            List<ResignationReason> GetResignationReasons();
           string GetResignationReasonsById(int resignationReasonId);
    }
    public class ResignationReasonRepository : BaseRepository<ResignationReasonRepository>, IResignationReasonRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ResignationReasonRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<ResignationReason> GetResignationReasons()
        {
            return dbContext.ResignationReason.Select(x => new ResignationReason { ResignationReasonId = x.ResignationReasonId, ResignationReasonName = x.ResignationReasonName, IsInvoluntary = x.IsInvoluntary }).ToList();
        }
        public string GetResignationReasonsById(int resignationReasonId)
        {
            return dbContext.ResignationReason.Where(x=>x.ResignationReasonId==resignationReasonId).Select(x => x.ResignationReasonName).FirstOrDefault();
        }
    }
}
