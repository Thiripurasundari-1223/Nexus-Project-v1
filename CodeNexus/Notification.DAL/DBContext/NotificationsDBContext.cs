using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Notifications;

namespace Notification.DAL.DBContext
{
    public class NotificationsDBContext : DbContext
    {
        public NotificationsDBContext(DbContextOptions<NotificationsDBContext> options) : base(options)
        { }
        public DbSet<SharedLibraries.Models.Notifications.Notifications> Notifications { get; set; }

        public DbSet<SupportingDocuments> SupportingDocuments { get; set; }

        public DbSet<Status> Status { get; set; }
        public DbSet<TimesheetConfigurationWeekDay> TimesheetConfigurationWeekDay { get; set; }
    }
}