using System;
using System.ComponentModel.DataAnnotations;

namespace Timesheet.DAL.Models
{
    public class Users
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
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? ZipId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public string UserRole { get; set; }
    }
}