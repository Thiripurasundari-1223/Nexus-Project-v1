using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace IAM.DAL.Models
{
    public class UserRoles
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserID { get; set; }
        public int RoleId { get; set; }
    }
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(o => o.UserRoleId);
        }
    }
}