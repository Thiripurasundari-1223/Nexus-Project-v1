using Notification.DAL.DBContext;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace Notifications.DAL.Repository
{
    public interface IStatusRepository : IBaseRepository<Status>
    {
        StatusViewModel GetStatusByName(string pStatusName);
        StatusViewModel GetStatusByID(int pStatusId);
        List<Status> GetAllStatus();
        List<StatusViewModel> GetAllStatusList();
        StatusViewModel GetStatusByCode(string pStatusCode);
        List<StatusViewModel> GetStatusByCode(List<string> pStatusCode);
    }
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        private readonly NotificationsDBContext dbContext;
        public StatusRepository(NotificationsDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public StatusViewModel GetStatusByName(string pStatusName)
        {
            return (from a in dbContext.Status.Where(x => x.StatusName == pStatusName)
                    select new StatusViewModel
            {
                StatusCode = a.StatusCode,
                StatusName = a.StatusName
            }).FirstOrDefault();
        }
        public StatusViewModel GetStatusByID(int pStatusId)
        {
            return (from a in dbContext.Status.Where(x => x.StatusId == pStatusId)
                select new StatusViewModel
            {
                StatusCode = a.StatusCode,
                StatusName = a.StatusName
            }).FirstOrDefault();
        }
        public List<Status> GetAllStatus()
        {
            return dbContext.Status.ToList();
        }

        public List<StatusViewModel> GetAllStatusList()
        {
            return (from a in dbContext.Status
                    select new StatusViewModel
                    {
                        StatusCode = a.StatusCode,
                        StatusName = a.StatusName
                    }).ToList();
        }
        public StatusViewModel GetStatusByCode(string pStatusCode)
        {
            return (from a in dbContext.Status.Where(x => x.StatusCode == pStatusCode)
                select new StatusViewModel
                {
                    StatusCode = a.StatusCode,
                    StatusName = a.StatusName
                }).FirstOrDefault();
        }
        public List<StatusViewModel> GetStatusByCode(List<string> pStatusCode)
        {
            return dbContext.Status.Where(x => pStatusCode.Contains(x.StatusCode)).Select(y=>new StatusViewModel
                    {
                        StatusCode = y.StatusCode,
                        StatusName = y.StatusName
                    }).ToList();
        }
    }
}