using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
    
    public interface IResignationApprovalStatusRepository : IBaseRepository<ResignationApprovalStatus>
    {
        ResignationApprovalStatus GetResignationApprovalStatus(int resignationDetailId, int levelId, string approvalType);
        int GetResignationApprovalLastLevel(int resignationDetailId);
        List<ResignationApprovalStatus> GetResignationApprovalStatusById(int resignationDetailId, string approvalType);
        ResignationApprovalStatus GetResignationLastApprovalStatus(int resignationDetailId, string approvalType);
    }
    public class ResignationApprovalStatusRepository : BaseRepository<ResignationApprovalStatus>, IResignationApprovalStatusRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ResignationApprovalStatusRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public ResignationApprovalStatus GetResignationApprovalStatus(int resignationDetailId, int levelId, string approvalType)
        {
            return dbContext.ResignationApprovalStatus.Where(x=>x.EmployeeResignationDetailsId== resignationDetailId && x.LevelId== levelId && x.ApprovalType== approvalType).FirstOrDefault();
        }
        public int GetResignationApprovalLastLevel(int resignationDetailId)
        {
            return dbContext.ResignationApprovalStatus.Where(x => x.EmployeeResignationDetailsId == resignationDetailId).Select(x=>x.LevelId==null?0: (int)x.LevelId).OrderByDescending(x=>x).FirstOrDefault();
        }
        public List<ResignationApprovalStatus> GetResignationApprovalStatusById(int resignationDetailId, string approvalType)
        {
            return dbContext.ResignationApprovalStatus.Where(x => x.EmployeeResignationDetailsId == resignationDetailId && x.ApprovalType== approvalType).ToList();
        }
        public ResignationApprovalStatus GetResignationLastApprovalStatus(int resignationDetailId, string approvalType)
        {
            return dbContext.ResignationApprovalStatus.Where(x => x.EmployeeResignationDetailsId == resignationDetailId && x.ApprovalType == approvalType).Select(x => x).OrderByDescending(x=>x.LevelId).FirstOrDefault();
        }
    }
}
