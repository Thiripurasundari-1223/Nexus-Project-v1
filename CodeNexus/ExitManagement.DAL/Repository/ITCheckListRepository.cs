
using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IITCheckListRepository : IBaseRepository<ITCheckList>
    {
        ITCheckList GetITResignationChecklistByID(int checklistId);
    }


    public class ITCheckListRepository : BaseRepository<ITCheckList>, IITCheckListRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ITCheckListRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public ITCheckList GetITResignationChecklistByID(int checklistId)
        {
            return dbContext.ITCheckList.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
    }
}


