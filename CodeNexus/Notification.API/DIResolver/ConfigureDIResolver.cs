using Microsoft.Extensions.DependencyInjection;
using Notifications.DAL;
using Notifications.DAL.Repository;
using Notifications.DAL.Services;

namespace Notifications.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void NotificationsDIResolver(this IServiceCollection services)
        {
            services.AddScoped<NotificationsServices>();
            services.AddScoped<INotificationsRepository, NotificationsRepository>();
            services.AddScoped<ISupportingDocumentsRepository, SupportingDocumentsRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITimesheetConfigurationWeekdayRepository, TimesheetConfigurationWeekdayRepository>();
        }
    }
}