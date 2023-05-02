using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appraisal.DAL.Repository;
using Appraisal.DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appraisal.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void AppraisalDIResolver(this IServiceCollection services)
        {
            services.AddScoped<WorkDayDetailServices>();
            services.AddScoped<IWorkDayDetailRepository, WorkDayDetailRepository>();
            services.AddScoped<AppraisalServices>();
            services.AddScoped<IAppraisalRepository, AppraisalRepository>();
            services.AddScoped<IAppraisalCycleRepository, AppraisalCycleRepository>();
            services.AddScoped<IAppraisalObjectiveRepository, AppraisalObjectiveRepository>();
            services.AddScoped<IAppraisalKeyResultRepository, AppraisalKeyResultRepository>();
            services.AddScoped<IVersionDepartmentRoleRepository, VersionDepartmentRoleRepository>();
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<IVersionKeyResultsRepository, VersionKeyResultsRepository>();
            services.AddScoped<IVersionBenchmarksRepository, VersionBenchmarksRepository>();
            services.AddScoped<IEmployeeKeyResultsRatingRepository, EmployeeKeyResultsRatingRepository>();
            services.AddScoped<IEmpObjectiveRatingRepository, EmpObjectiveRatingRepository>();
            services.AddScoped<IEmployeeKResultCommentRepository, EmployeeKResultCommentRepository>();
            services.AddScoped<IEmployeeAppraisalCommentRepository, EmployeeAppraisalCommentRepository>();
            services.AddScoped<IAppConstantTypeRepository, AppConstantTypeRepository>();
            services.AddScoped<IAppConstantsRepository, AppConstantsRepository>();
            services.AddScoped<IVersionKeyResultsGroupRepository, VersionKeyResultsGroupRepository>();
            services.AddScoped<IVersionKeyResultsGroupDetailsRepository, VersionKeyResultsGroupDetailsRepository>();
            services.AddScoped<IEmployeeAppraisalMasterRepository, EmployeeAppraisalMasterRepository>();
            services.AddScoped<IEmployeeKeyResultAttachmentRepository, EmployeeKeyResultAttachmentRepository>();
            services.AddScoped<IVersionDepartmentRoleObjectiveRepository, VersionDepartmentRoleObjectiveRepository>();
            services.AddScoped<IEmployeeGroupSelectionRepository, EmployeeGroupSelectionRepository>();
            services.AddScoped<IEmployeeGroupRatingRepository, EmployeeGroupRatingRepository>();
            services.AddScoped<IAppBUHeadCommentsRepository, AppBUHeadCommentsRepository>();
            services.AddScoped<IWorkDayRepository, WorkDayRepository>();
            services.AddScoped<IWorkdayObjectiveRepository, WorkdayObjectiveRepository>();
            services.AddScoped<IWorkdayKRARepository, WorkdayKRARepository>();
        }
    }
}