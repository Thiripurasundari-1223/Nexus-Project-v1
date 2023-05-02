using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Projects;

namespace ProjectManagement.DAL.DBContext
{
    public class PMDBContext : DbContext
    {
        public PMDBContext(DbContextOptions<PMDBContext> options) : base(options)
        { }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }
        public DbSet<ChangeRequest> ChangeRequest { get; set; }
        public DbSet<ResourceAllocation> ResourceAllocation { get; set; }
        public DbSet<CurrencyType> CurrencyType { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<RateFrequency> RateFrequency { get; set; }
        public DbSet<Allocation> Allocation { get; set; }
        ///public DbSet<RequiredSkillSet> RequiredSkillSet { get; set; }
        public DbSet<ProjectDetailComments> ProjectDetailComments { get; set; }
        public DbSet<ChangeRequestType> ChangeRequestType { get; set; }
    }
}