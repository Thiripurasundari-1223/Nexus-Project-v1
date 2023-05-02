
using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IAdminCheckListRepository : IBaseRepository<AdminCheckList>
    {
        AdminCheckList GetAdminResignationChecklistByID(int checklistId);
    }


    public class AdminCheckListRepository : BaseRepository<AdminCheckList>, IAdminCheckListRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public AdminCheckListRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public AdminCheckList GetAdminResignationChecklistByID(int checklistId)
        {
            return dbContext.AdminCheckList.Where(x => x.ResignationChecklistId == checklistId).FirstOrDefault();
        }
    }
}

