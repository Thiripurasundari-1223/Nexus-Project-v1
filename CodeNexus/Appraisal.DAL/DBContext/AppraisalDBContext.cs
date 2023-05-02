using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.Models.Attendance;

namespace Appraisal.DAL.DBContext
{
    public class AppraisalDBContext : DbContext
    {
        public AppraisalDBContext(DbContextOptions<AppraisalDBContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VersionDepartmentRoleMapping>().HasKey(table => new {
                table.VERSION_ID,
                table.DEPT_ID,
                table.ROLE_ID
            });
            builder.Entity<VersionDepartmentRoleObjective>().HasKey(table => new {
                table.VERSION_ID,
                table.DEPT_ID,
                table.ROLE_ID,
                table.OBJECTIVE_ID
            });
            builder.Entity<VersionKeyResults>().HasKey(table => new {
                table.VERSION_ID,
                table.DEPT_ID,
                table.ROLE_ID,
                table.OBJECTIVE_ID,
                table.KEY_RESULT_ID
            });
            builder.Entity<EmployeeObjectiveRating>().HasKey(table => new {
                table.APP_CYCLE_ID,
                table.EMPLOYEE_ID,
                table.OBJECTIVE_ID
            });
            builder.Entity<EmployeeKeyResultRating>().HasKey(table => new {
                table.APP_CYCLE_ID,
                table.EMPLOYEE_ID,
                table.OBJECTIVE_ID,
                table.KEY_RESULT_ID
            });
            builder.Entity<VersionKeyResultsGroupDetails>().HasKey(table => new {
                table.VERSION_ID,
                table.DEPT_ID,
                table.ROLE_ID,
                table.OBJECTIVE_ID,
                table.KEY_RESULTS_GROUP_ID,
                table.KEY_RESULT_ID
            });
            builder.Entity<EmployeeAppraisalMaster>().HasKey(table => new {
                table.APP_CYCLE_ID,
                table.EMPLOYEE_ID
            });
            builder.Entity<EmployeeGroupSelection>().HasKey(table => new {
                table.APP_CYCLE_ID,
                table.EMPLOYEE_ID,
                table.OBJECTIVE_ID,
                table.KEY_RESULT_ID,
                table.KEY_RESULTS_GROUP_ID
            });
            builder.Entity<EmployeeGroupRating>().HasKey(table => new {
                table.APP_CYCLE_ID,
                table.EMPLOYEE_ID,
                table.OBJECTIVE_ID,
                table.KEY_RESULTS_GROUP_ID
            });

        }
        public DbSet<EntityMaster> EntityMaster { get; set; }
        public DbSet<AppraisalMaster> AppraisalMaster { get; set; }
        public DbSet<ObjectiveMaster> ObjectiveMaster { get; set; }
        public DbSet<KeyResultMaster> KeyResultMaster { get; set; }
        public DbSet<VersionDepartmentRoleMapping> VersionDepartmentRoleMapping { get; set; }
        public DbSet<VersionMaster> VersionMaster { get; set; }
        public DbSet<VersionKeyResults> VersionKeyResults { get; set; }
        public DbSet<VersionBenchMarks> VersionBenchMarks { get; set; }
        public DbSet<EmployeeKeyResultRating> EmployeeKeyResultRating { get; set; }
        public DbSet<EmployeeObjectiveRating> EmployeeObjectiveRating { get; set; }
        public DbSet<EmployeeKeyResultConversation> EmployeeKeyResultConversation { get; set; }
        public DbSet<EmployeeAppraisalConversation> EmployeeAppraisalConversation { get; set; }
        public DbSet<AppConstantType> AppConstantType { get; set; }
        public DbSet<AppConstants> AppConstants { get; set; } 
        public DbSet<VersionDepartmentRoleObjective> VersionDepartmentRoleObjective { get; set; }
        public DbSet<VersionKeyResultsGroup> VersionKeyResultsGroup { get; set; }
        public DbSet<VersionKeyResultsGroupDetails> VersionKeyResultsGroupDetails { get; set; }
        public DbSet<EmployeeAppraisalMaster> EmployeeAppraisalMaster { get; set; }

        public DbSet<EmployeeKeyResultAttachments> EmployeeKeyResultAttachments { get; set; }

        public DbSet<EmployeeGroupSelection> EmployeeGroupSelection { get; set; }

        public DbSet<EmployeeGroupRating> EmployeeGroupRating { get; set; }
        public DbSet<AppraisalBUHeadComments> AppraisalBUHeadComments { get; set; }

        public DbSet<WorkDayDetail> WorkDayDetail { get; set; }
        public DbSet<WorkDay> WorkDay { get; set; }
        public DbSet<WorkDayDocument> WorkDayDocument { get; set; }
        public DbSet<WorkdayKRA> WorkdayKRA { get; set; }
        public DbSet<WorkdayObjective> WorkdayObjective { get; set; }
    }
}
