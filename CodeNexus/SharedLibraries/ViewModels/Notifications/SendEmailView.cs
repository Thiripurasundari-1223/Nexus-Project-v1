using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Notifications
{
    public class SendEmailView
    {
		public string ToEmailID { get; set; }
		public string Subject { get; set; }
		public string MailBody { get; set; }
		public string FromEmailID { get; set; }
		public int Port { get; set; }
		public string Host { get; set; }
		public string FromEmailPassword { get; set; }
		public string ResourceEmail { get; set; }
		public string CC { get; set; }
	}
}
