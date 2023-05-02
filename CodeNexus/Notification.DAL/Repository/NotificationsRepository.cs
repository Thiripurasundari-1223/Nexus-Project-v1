using Notification.DAL.DBContext;
using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace Notifications.DAL.Repository
{
    public interface INotificationsRepository : IBaseRepository<SharedLibraries.Models.Notifications.Notifications>
    {
        List<NotificationView> GetNotificationsByResourceId(int pResourceId);
        int GetNotificationUnReadByResourceId(int pResourceId);
        List<SharedLibraries.Models.Notifications.Notifications> GetListNotificationUnReadByResourceId(int pResourceId);
    }
    public class NotificationsRepository : BaseRepository<SharedLibraries.Models.Notifications.Notifications>, INotificationsRepository
    {
        private readonly NotificationsDBContext dbContext;
        public NotificationsRepository(NotificationsDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<NotificationView> GetNotificationsByResourceId(int pResourceId)
        {
            List<SharedLibraries.Models.Notifications.Notifications> notifications = dbContext.Notifications.Where(x => x.ToId == pResourceId).OrderByDescending(x => x.NotificationId).ToList();
            List<NotificationView> notificationViews = new List<NotificationView>();
            foreach (SharedLibraries.Models.Notifications.Notifications notification in notifications)
            {
                NotificationView notificationView = new NotificationView
                {
                    CreatedByName = "",
                    CreatedBy = notification.CreatedBy,
                    CreatedOn = notification.CreatedOn,
                    FromUserName = "",
                    FromId = notification.FromId,
                    MarkAsRead = notification.MarkAsRead,
                    NotificationSubject = notification.NotificationSubject,
                    NotificationBody = notification.NotificationBody,
                    ToUserName = "",
                    ToId = notification.ToId,
                    PrimaryKeyId = notification.PrimaryKeyId,
                    ButtonName = notification.ButtonName,
                    SourceType = notification.SourceType,
                    NotificationId = notification.NotificationId,
                    Data = notification.Data
                };
                notificationViews.Add(notificationView);
            }
            return notificationViews;
        }
        public int GetNotificationUnReadByResourceId(int pResourceId)
        {
            return dbContext.Notifications.Where(x => x.ToId == pResourceId && x.MarkAsRead == false).Count();
        }
        public List<SharedLibraries.Models.Notifications.Notifications> GetListNotificationUnReadByResourceId(int pResourceId)
        {
            return dbContext.Notifications.Where(x => x.ToId == pResourceId && x.MarkAsRead == false).ToList();
        }
    }
}