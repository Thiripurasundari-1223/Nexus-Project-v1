//using Authentication.Manager;
//using IAM.DAL.Services;
using IAM.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Notifications.DAL.Repository;
using ProjectManagement.DAL.Repository;
using ProjectManagement.DAL.Services;

namespace ProjectManagement.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void PMDIResolver(this IServiceCollection services)
        {
            services.AddScoped<PMServices>();
            services.AddScoped<IProjectDetailsRepository, ProjectDetailsRepository>();
            services.AddScoped<IChangeRequestDetailRepository, ChangeRequestDetailRepository>();
            services.AddScoped<IResouceAllocationRepository, ResourceAllocationRepository>();
            services.AddScoped<IProjectDetailCommentsRepository, ProjectDetailCommentsRepository>();
            services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
            services.AddScoped<ICustomerSPOCDetailsRepository, CustomerSPOCDetailsRepository>();
            services.AddScoped<IFixedIterationRepository, FixedIterationRepository>();
            services.AddScoped<DAL.Repository.IAuditRepository, DAL.Repository.AuditRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
          //  services.AddScoped<IStatusRepository, StatusRepository>();



    }
    }
}