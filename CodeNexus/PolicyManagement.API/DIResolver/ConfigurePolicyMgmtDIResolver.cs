using PolicyManagement.DAL.Repository;
using PolicyManagement.DAL.Services;

namespace PolicyManagement.API.DIResolver
{
    public static class ConfigurePolicyMgmtDIResolver
    {
        public static void PolicyMgmtDIResolver(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPolicyDocumentRepository, PolicyDocumentRepository>();
            serviceCollection.AddScoped<IFolderRepository, FolderRepository>();
            serviceCollection.AddScoped<ISharePolicyDocumentRepository, SharePolicyDocumentRepository>();
            serviceCollection.AddScoped<IRequestedDocumentRepository, RequestedDocumentRepository>();
            serviceCollection.AddScoped<IDocumentTypesRepository, DocumentTypesRepository>();
            serviceCollection.AddScoped<IDocumentTagRepository, DocumentTagRepository>();
            serviceCollection.AddScoped<IPolicyAcknowledgedRepository, PolicyAcknowledgedRepository>();
            serviceCollection.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            serviceCollection.AddScoped<PolicyDocumentService>();
            serviceCollection.AddScoped<RequestedDocumentService>();
            serviceCollection.AddScoped<AnnouncementService>();
        }
    }
}