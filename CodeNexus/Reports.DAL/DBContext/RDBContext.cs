using Microsoft.EntityFrameworkCore;
using Reports.DAL.Models;
using System.Collections.Generic;
using System.Data;

namespace Reports.DAL.DBContext
{
   public class RDBContext : DbContext
    {
        public RDBContext(DbContextOptions<RDBContext> options) : base(options)
        { }
        //public DbSet<AccountStatusReport> GetReport { get; set; }
        public DbSet<DataTable> GetReport { get; set; }

    }
}
