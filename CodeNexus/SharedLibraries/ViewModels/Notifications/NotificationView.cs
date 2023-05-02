using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels.Notifications
{
    public class NotificationView
    {
        [Key]
        public int NotificationId { get; set; }
        public int FromId { get; set; }
        public string FromUserName { get; set; }
        public int ToId { get; set; }
        public string ToUserName { get; set; }
        public string NotificationSubject { get; set; }
        public string NotificationBody { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public int? PrimaryKeyId { get; set; }
        public string ButtonName { get; set; }
        public string SourceType { get; set; }
        public string Data { get; set; }
    }
    public class NotificationMarkAsRead
    {
        public int NotificationId { get; set; }
        public int ResourceId { get; set; }
    }
    public class NotificationViewList
    {
        public List<NotificationView> NotificationView { get; set; }
        public int unReadCount { get; set; }
    }
}