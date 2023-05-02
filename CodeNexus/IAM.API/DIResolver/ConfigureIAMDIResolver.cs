using IAM.API.AzureAuthenticationManager;
using IAM.DAL.Models;
using IAM.DAL.Repository;
using IAM.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraries.Authentication.Handler;
using SharedLibraries.Authentication.Manager;

namespace IAM.API.DIResolver
{
    public static class ConfigureIAMDIResolver
    {
        public static void IAMDIResolver(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITokenManager, TokenManager>();
            serviceCollection.AddScoped<IAzureADToken, AzureADToken>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IRoleRepository, RoleRepository>();
            serviceCollection.AddScoped<IRoleSetupRepository, RoleSetupRepository>();
            serviceCollection.AddScoped<IUserRolesRepository, UserRolesRepository>();
            serviceCollection.AddScoped<IUserAccessBagRepository, UserAccessBagRepository>();
            serviceCollection.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();
            serviceCollection.AddScoped<IA2DAuthenticationManager, A2DAuthenticationManager>();
            serviceCollection.AddScoped<IUserTokenRepository, UserTokenRepository>();
            //serviceCollection.AddScoped<AuthHandler>();
            serviceCollection.AddScoped<IIAMService, IAMService>();
            //Employee
            serviceCollection.AddScoped<EmployeeService>();
            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IDepartmentRepository, DepartmentRepository>();
            serviceCollection.AddScoped<ISkillsetRepository, SkillsetRepository>();
            serviceCollection.AddScoped<IEmployeesSkillsetRepository, EmployeesSkillsetRepository>();
            serviceCollection.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
            serviceCollection.AddScoped<IEmployeeCategoryRepository, EmployeeCategoryRepository>();
            //Roles related
            serviceCollection.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            serviceCollection.AddScoped<IEmployeeRelationshipRepository, EmployeeRelationshipRepository>();
            serviceCollection.AddScoped <IEmployeeDependentRepository, EmployeeDependentRepository>();
            serviceCollection.AddScoped<IEmployeeShiftDetailsRepository, EmployeeShiftDetailsRepository>();
            serviceCollection.AddScoped<IProbationStatusRepository, ProbationStatusRepository>();
            serviceCollection.AddScoped<ISystemRoleRepository, SystemRoleRepository>();
            serviceCollection.AddScoped<IWorkHistoryRepository, WorkHistoryRepository>();
            serviceCollection.AddScoped<IEducationDetailrepository, EducationDetailrepository>();
            serviceCollection.AddScoped<ICompensationDetailRepository, CompensationDetailRepository>();
            serviceCollection.AddScoped<IEmployeeDocumentRepository, EmployeeDocumentRepository>();
            serviceCollection.AddScoped<IEmployeeAppConstantRepository, EmployeeAppConstantRepository>();
            serviceCollection.AddScoped<IEmployeesPersonalInfoRepository, EmployeesPersonalInfoRepository>();
            serviceCollection.AddScoped<ICountryRepository, CountryRepository>();
            serviceCollection.AddScoped<IStateRepository, StateRepository>();
            serviceCollection.AddScoped<IDesignationRepository, DesignationRepository>();
            serviceCollection.AddScoped<ISkillsetHistoryRepository, SkillsetHistoryRepository>();
            serviceCollection.AddScoped<IEmployeeRequestRepository, EmployeeRequestRepository>();
            serviceCollection.AddScoped<IEmployeeSpecialAbilityRepository, EmployeeSpecialAbilityRepository>();

            serviceCollection.AddScoped<IEmployeeDesignationHistoryRepository, EmployeeDesignationHistoryRepository>();
            serviceCollection.AddScoped<IEmployeeNationalityRepository, EmployeeNationalityRepository>();
            serviceCollection.AddScoped<IAuditRepository, AuditRepository>();
            serviceCollection.AddScoped<IEmployeeRequestDetailsRepository, EmployeeRequestDetailsRepository>();
            serviceCollection.AddScoped<IEmployeeRequestDocumentRepository, EmployeeRequestDocumentRepository>();
            serviceCollection.AddScoped<IEmployeeLocationRepository, EmployeeLocationRepository>();

        }
    }
}