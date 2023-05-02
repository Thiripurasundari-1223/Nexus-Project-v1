using Timesheet.DAL.DBContext;
using Timesheet.DAL.Models;

namespace Timesheet.DAL.Repository
{
    public interface INotificationsRepository : IBaseRepository<Notifications>
    {
        //Notifications GetByID(int pNotificationId);
        //List<NotificationView> GetNotificationsByResourceId(int pResourceId);
        //int GetNotificationUnReadByResourceId(int pResourceId);
    }
    public class NotificationsRepository : BaseRepository<Notifications>, INotificationsRepository
    {
        private readonly TSDBContext dbContext;
        public NotificationsRepository(TSDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        //public Notifications GetByID(int pNotificationId)
        //{
        //    return dbContext.Notifications.Where(x => x.NotificationId == pNotificationId).FirstOrDefault();
        //}
        //public List<NotificationView> GetNotificationsByResourceId(int pResourceId)
        //{
        //    List<Notifications> notifications = dbContext.Notifications.Where(x => x.ToId == pResourceId).ToList();
        //    List<NotificationView> notificationViews = new List<NotificationView>();
        //    foreach (Notifications notification in notifications)
        //    {
        //        NotificationView notificationView = new NotificationView
        //        {
        //            CreatedByName = dbContext.Users.Where(x => x.UserId == notification.CreatedBy).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),
        //            CreatedOn = notification.CreatedOn,
        //            FromUserName = dbContext.Users.Where(x => x.UserId == notification.FromId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),
        //            MarkAsRead = notification.MarkAsRead,
        //            NotificationSubject = notification.NotificationSubject,
        //            NotificationBody = notification.NotificationBody,
        //            ToUserName = dbContext.Users.Where(x => x.UserId == notification.ToId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault()
        //        };
        //        notificationViews.Add(notificationView);
        //    }
        //    return notificationViews;
        //}
        //public int GetNotificationUnReadByResourceId(int pResourceId)
        //{
        //    return dbContext.Notifications.Where(x => x.ToId == pResourceId && x.MarkAsRead == false).Count();
        //}
    }
}