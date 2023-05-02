using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NexusWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<Worker>();
                    services.AddHostedService<TimesheetAlertSubmission>();
                    services.AddHostedService<TimesheetAlertApproval>();
                    services.AddHostedService<AccruedEmployeeLeaves>();
                    services.AddHostedService<CarryForwardLeaves>(); 
                    services.AddHostedService<AttendanceAlertAbsent>();
                    services.AddHostedService<DeactivateResignEmployee>();
                    services.AddHostedService<ExitChecklistSubmission>();
                    services.AddHostedService<ExitInterviewSubmission>();
                    services.AddHostedService<ContractClosureAlert>();
                    services.AddHostedService<UpdateEmployeeDesignation>();
                });
    }
}
