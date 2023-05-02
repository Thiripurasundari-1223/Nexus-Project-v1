//using Authentication.Manager;
//using IAM.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<IProjectDocumentRepository, ProjectDocumentRepository>();
            services.AddScoped<ICustomerSPOCDetailsRepository, CustomerSPOCDetailsRepository>();
            services.AddScoped<IFixedIterationRepository, FixedIterationRepository>();
            services.AddScoped<IProjectVersionDetailsCommentsRepository, ProjectVersionDetailsCommentsRepository>();
            services.AddScoped<IResourceAllocationVersionRespository, ResourceAllocationVersionRespository>();
            services.AddScoped<IResourceAllocationVersionRespository, ResourceAllocationVersionRespository>();
            services.AddScoped<ICustomerSPOCVersionDetailsRepository, CustomerSPOCVersionDetailsRepository>();
            services.AddScoped<IProjectVersionDocumentRepository, ProjectVersionDocumentRepository>();
           services.AddScoped<IFixedIterationVersionRepository, FixedIterationVersionRepository>();

        }
    }
}