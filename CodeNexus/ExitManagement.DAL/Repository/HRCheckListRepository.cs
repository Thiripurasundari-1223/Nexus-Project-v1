
using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IHRCheckListRepository : IBaseRepository<HRCheckList>
    {
        HRCheckList GetHRResignationChecklistByID(int checklistId);
    }

    public class HRCheckListRepository : BaseRepository<HRCheckList>, IHRCheckListRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public HRCheckListRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public HRCheckList GetHRResignationChecklistByID(int checklistId)
        {
            return dbContext.HRCheckList.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
    }
}


