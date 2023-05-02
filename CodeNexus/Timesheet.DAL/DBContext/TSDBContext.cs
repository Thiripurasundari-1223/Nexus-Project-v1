using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels;
using Timesheet.DAL.Models;

namespace Timesheet.DAL.DBContext
{
    public class TSDBContext : DbContext
    {
        private readonly EntityConnections entityConnections;
        public TSDBContext(EntityConnections entityConnections)
        {
            this.entityConnections = entityConnections;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(entityConnections.ConnectionStrings);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        { }
        public DbSet<SharedLibraries.Models.Timesheet.Timesheet> Timesheet { get; set; }
        public DbSet<TimesheetLog> TimesheetLog { get; set; }
        public DbSet<TimesheetComments> TimesheetComments { get; set; }
        //public DbSet<ProjectDetails> ProjectDetails { get; set; }
        //public DbSet<Users> Users { get; set; }
        //public DbSet<ResourceAllocation> ResourceAllocation { get; set; }
        public DbSet<RejectionReason> RejectionReason { get; set; }
        //public DbSet<Notifications> Notifications { get; set; }
        public DbSet<TimesheetConfigurationDetails> TimesheetConfigurationDetails { get; set; }

    }
}