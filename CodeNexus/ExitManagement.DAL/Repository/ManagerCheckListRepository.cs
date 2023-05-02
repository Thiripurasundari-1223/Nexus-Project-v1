using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
 
    public interface IManagerCheckListRepository : IBaseRepository<ManagerCheckList>
    {
        ManagerCheckList GetManagerResignationChecklistByID(int checklistId);
    }


    public class ManagerCheckListRepository : BaseRepository<ManagerCheckList>, IManagerCheckListRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ManagerCheckListRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public ManagerCheckList GetManagerResignationChecklistByID(int checklistId)
        {
            return dbContext.ManagerCheckList.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
    }
}
