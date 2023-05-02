using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAM.DAL.Models
{
    #region User 
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? ZipId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public int? AccountTypeId { get; set; }
        public string AttemptStatus { get; set; }
        public string LogInIP { get; set; }
        public int? AttemptCount { get; set; }
        public string Notes { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public string UserRole { get; set; }
    }
    #endregion
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(o => o.UserId);
        }
    }
}