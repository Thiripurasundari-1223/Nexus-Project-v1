using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    
    public interface IPreventAbsentNotificationRepository : IBaseRepository<PreventAbsentNotification>
    {
        List<int> GetAllPreventAbsentNotification();

    }
    public class PreventAbsentNotificationRepository : BaseRepository<PreventAbsentNotification>, IPreventAbsentNotificationRepository
    {
        private readonly IAMDBContext dbContext;
        public PreventAbsentNotificationRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<int> GetAllPreventAbsentNotification()
        {
            return dbContext.PreventAbsentNotification.Select(x=>x.EmployeeId==null?0:(int)x.EmployeeId).ToList();
        }
        
    }
}
