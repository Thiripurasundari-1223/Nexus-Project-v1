using Microsoft.Extensions.DependencyInjection;
using Reports.DAL.Repository;
using Reports.DAL.Services;

namespace Reports.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void RDIResolver(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ReportServices>();
            serviceCollection.AddScoped<IReportRepository, ReportRepository>();
        }
    }
}