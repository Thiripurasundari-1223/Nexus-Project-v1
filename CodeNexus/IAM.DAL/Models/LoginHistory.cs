using System;
using System.ComponentModel.DataAnnotations;

namespace IAM.DAL.Models
{
    public class LoginHistory
    {
        [Key]
        public int LoginHistoryID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public DateTime? LogOutTime { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}