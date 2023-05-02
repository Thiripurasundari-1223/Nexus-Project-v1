using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace IAM.DAL.Models
{
    public class RoleSetup
    {
        [Key]
        public int RoleSetupID { get; set; }
        public int RoleID { get; set; }
        public int AccountManagement { get; set; }
        public int ProjectManagement { get; set; }
        public int ResourceManagement { get; set; }
        public int TimeSheet { get; set; }
        public int TeamTimeSheet { get; set; }
        public int MyPerformance { get; set; }
        public int TeamAppraisal { get; set; }
        public int ContinuousFeedback { get; set; }
        //public bool? ConfigurationAccess { get; set; }
        //public bool? EditAccess { get; set; }
        //public bool? ViewAccess { get; set; }
        //public bool? DeleteAccess { get; set; }
        //public bool? PrintAccess { get; set; }
        //public string ListOfVMenu { get; set; }
    }
    public class RoleSetupConfiguration : IEntityTypeConfiguration<RoleSetup>
    {
        public void Configure(EntityTypeBuilder<RoleSetup> builder)
        {
            builder.ToTable("RoleSetup");
            builder.HasKey(o => o.RoleSetupID);
        }
    }
}