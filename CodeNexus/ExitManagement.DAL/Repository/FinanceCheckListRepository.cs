
using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IFinanceCheckListRepository : IBaseRepository<FinanceCheckList>
    {
        FinanceCheckList GetFinanceResignationChecklistByID(int checklistId);
    }


    public class FinanceCheckListRepository : BaseRepository<FinanceCheckList>, IFinanceCheckListRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public FinanceCheckListRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public FinanceCheckList GetFinanceResignationChecklistByID(int checklistId)
        {
            return dbContext.FinanceCheckList.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
    }
}

