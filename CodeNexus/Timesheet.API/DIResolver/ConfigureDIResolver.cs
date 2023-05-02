using Microsoft.Extensions.DependencyInjection;
using Timesheet.DAL.Repository;
using Timesheet.DAL.Services;

namespace Timesheet.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void TSDIResolver(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<TimesheetServices>();
            serviceCollection.AddScoped<ITimesheetRepository, TimesheetRepository>();
            serviceCollection.AddScoped<ITimesheetLogRepository, TimesheetLogRepository>();
            serviceCollection.AddScoped<ITimesheetCommentsRepository, TimesheetCommentsRepository>();
            serviceCollection.AddScoped<INotificationsRepository, NotificationsRepository>();
            serviceCollection.AddScoped<ITimesheetConfigurationRepository, TimesheetConfigurationRepository>();
        }
    }
}