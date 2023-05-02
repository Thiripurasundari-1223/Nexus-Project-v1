using CustomerOnBoarding.DAL.Repository;
using CustomerOnBoarding.DAL.Services;
//using IAM.DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOnBoarding.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void COBDIResolver(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<COBServices>();
            serviceCollection.AddScoped<IAccountDetailsRepository, AccountDetailsRepository>();
            serviceCollection.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
            serviceCollection.AddScoped<ICountryRepository, CountryRepository>();
            serviceCollection.AddScoped<IStateRepository, StateRepository>();
            serviceCollection.AddScoped<IAccountChangeRequestRepository, AccountChangeRequestRepository>();
            serviceCollection.AddScoped<IAccountCommentsRepository, AccountCommentsRepository>();
            //serviceCollection.AddScoped<INotificationsRepository, NotificationsRepository>();
            serviceCollection.AddScoped<IBillingCycleRepository, BillingCycleRepository>();
            serviceCollection.AddScoped<ICustomerContactDetailsRepository, CustomerContactDetailsRepository>();
            serviceCollection.AddScoped<IAppConstantsRepository, AppConstantsRepository>();
            serviceCollection.AddScoped<IVersionAccountDetailsRepository, VersionAccountDetailsRepository>();
            serviceCollection.AddScoped<IVersionCustomerContactDetailsRepository, VersionCustomerContactDetailsRepository>();
        }
    }
}