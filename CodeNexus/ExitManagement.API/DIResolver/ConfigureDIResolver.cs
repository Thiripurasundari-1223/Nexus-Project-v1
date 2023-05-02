using ExitManagement.DAL.Repository;
using ExitManagement.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExitManagement.API.DIResolver
{
    public static class ConfigureDIResolver
    {
        public static void ExitManagementDIResolver(this IServiceCollection services)
        {
            services.AddScoped<ExitManagementServices>();
            services.AddScoped<IEmployeeResignationDetailsRepository, EmployeeResignationDetailsRepository>();
            services.AddScoped<IResignationReasonRepository, ResignationReasonRepository>();
            services.AddScoped<IAppConstantsRepository, AppConstantsRepository>();
            services.AddScoped<IResignationApprovalRepository, ResignationApprovalRepository>();
            services.AddScoped<IResignationApprovalStatusRepository, ResignationApprovalStatusRepository>();
            services.AddScoped<IExitManagementEmailTemplateRepository, ExitManagementEmailTemplateRepository>();
            services.AddScoped<IResignationInterviewRepository, ResignationInterviewRepository>();
            services.AddScoped<IResignationFeedbackToManagementRepository, ResignationFeedbackToManagementRepository>();
            services.AddScoped<IReasonLeavingPositionRepository, ReasonLeavingPositionRepository>();
            services.AddScoped<IResignationChecklistRepository, ResignationChecklistRepository>();
            services.AddScoped<IAdminCheckListRepository, AdminCheckListRepository>();
            services.AddScoped<IHRCheckListRepository, HRCheckListRepository>();
            services.AddScoped<IPMOCheckListRepository, PMOCheckListRepository>();
            services.AddScoped<IFinanceCheckListRepository, FinanceCheckListRepository>();
            services.AddScoped<IManagerCheckListRepository, ManagerCheckListRepository>();
            services.AddScoped<IITCheckListRepository, ITCheckListRepository>();
            services.AddScoped<ICheckListViewRepository, CheckListViewRepository>();

        }
    }
}
