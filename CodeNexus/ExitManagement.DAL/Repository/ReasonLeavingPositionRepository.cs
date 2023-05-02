using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
   
    public interface IReasonLeavingPositionRepository : IBaseRepository<ReasonLeavingPosition>
    {
        ReasonLeavingPosition GetLeavingPositionById(int positionId);
        List<ReasonLeavingPosition> GetLeavingPositionByInterviewId(int interviewId);
        
    }
    public class ReasonLeavingPositionRepository : BaseRepository<ReasonLeavingPosition>, IReasonLeavingPositionRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ReasonLeavingPositionRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public ReasonLeavingPosition GetLeavingPositionById(int positionId)
        {
            return dbContext.ReasonLeavingPosition.Where(x=>x.ReasonLeavingPositionId==positionId).FirstOrDefault();
        }
        public List<ReasonLeavingPosition> GetLeavingPositionByInterviewId(int interviewId)
        {
            return dbContext.ReasonLeavingPosition.Where(x => x.ResignationInterviewId == interviewId).ToList();
        }
    }
}
