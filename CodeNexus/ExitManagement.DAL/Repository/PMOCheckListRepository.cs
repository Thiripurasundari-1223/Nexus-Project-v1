
using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IPMOCheckListRepository : IBaseRepository<PMOCheckList>
    {
        PMOCheckList GetPMOResignationChecklistByID(int checklistId);
    }


    public class PMOCheckListRepository : BaseRepository<PMOCheckList>, IPMOCheckListRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public PMOCheckListRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public PMOCheckList GetPMOResignationChecklistByID(int checklistId)
        {
            return dbContext.PMOCheckList.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
    }
}

