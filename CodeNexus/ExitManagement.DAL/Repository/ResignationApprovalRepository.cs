using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IResignationApprovalRepository : IBaseRepository<ResignationApproval>
    {
        List<ResignationApproval> GetResignationApproval();
    }
    public class ResignationApprovalRepository : BaseRepository<ResignationApproval>, IResignationApprovalRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ResignationApprovalRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<ResignationApproval> GetResignationApproval()
        {
            return dbContext.ResignationApproval.ToList();
        }
    }
}
