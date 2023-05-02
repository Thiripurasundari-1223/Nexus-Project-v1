using System;
using System.ComponentModel.DataAnnotations;

namespace Timesheet.DAL.Models
{
    public class Notifications
    {
        [Key]
        public int NotificationId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string NotificationSubject { get; set; }
        public string NotificationBody { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? PrimaryKeyId { get; set; }
        public string ButtonName { get; set; }
        public string SourceType { get; set; }
    }
    public class NotificationView
    {
        [Key]
        public int NotificationId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string NotificationSubject { get; set; }
        public string NotificationBody { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy{ get; set; }
        public string CreatedByName { get; set; }
        public int? PrimaryKeyId { get; set; }
        public string ButtonName { get; set; }
        public string SourceType { get; set; }
    }
}