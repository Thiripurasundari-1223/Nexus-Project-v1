using CustomerOnBoarding.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Accounts;

namespace CustomerOnBoarding.DAL.DBContext
{
    public class COBDBContext : DbContext
    {
        private readonly EntityConnections entityConnections;
        public COBDBContext(EntityConnections entityConnections)
        {
            this.entityConnections = entityConnections;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(entityConnections.ConnectionStrings);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        { }
        public DbSet<AccountDetails> AccountDetails { get; set; }
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<AccountRelatedIssue> AccountRelatedIssue { get; set; }
        public DbSet<AccountComments> AccountComments { get; set; }
        public DbSet<AccountChangeRequest> AccountChangeRequest { get; set; }
        public DbSet<BillingCycle> BillingCycle { get; set; }
        public DbSet<CustomerContactDetails> CustomerContactDetails { get;set;}
        public DbSet<AppConstants> AppConstants { get; set; }
        public DbSet<VersionAccountDetails> VersionAccountDetails { get; set; }
        public DbSet<VersionCustomerContactDetails> VersionCustomerContactDetails { get; set; }
    }
}