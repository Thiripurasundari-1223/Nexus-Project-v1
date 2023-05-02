using SharedLibraries.ViewModels.Notifications;
using System.Net;
using System.Net.Mail;


namespace SharedLibraries
{
    public class SendEmail 
    {        
        public static string Sendmail(SendEmailView sendEmail)
        {
                        
            try
            {
                if (sendEmail.ToEmailID  != null && sendEmail.ToEmailID != "")
                {
                    var client = new SmtpClient(sendEmail.Host, sendEmail.Port)
                    {
                        UseDefaultCredentials = false,
                        DeliveryMethod=SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(sendEmail.FromEmailID, sendEmail.FromEmailPassword),
                        EnableSsl = true,                      
                    };
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress(sendEmail.FromEmailID,"Nexus");
                    mailMsg.To.Add(sendEmail.ToEmailID);
                    mailMsg.Subject = sendEmail.Subject;
                    mailMsg.Body = sendEmail.MailBody;
                    mailMsg.IsBodyHtml = true;

                    if (sendEmail.CC != null && sendEmail.CC != "")
                    {
                        string[] CCID = sendEmail.CC.Split(',');
                        foreach (string CCEmail in CCID)
                        {
                            if(!string.IsNullOrEmpty(CCEmail))
                            {
                                mailMsg.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC Email ID 
                            }                            
                        }
                    }
                    //mailMsg.CC.Add(new MailAddress(CC));
                    //mailMsg.Sender = new MailAddress("");
                    //mailMsg.ReplyTo = new MailAddress("");

                    client.Send(mailMsg);                 
                    //client.Send(resourceEmail, toEmailId, subject, mailBody);
                    return "Success";
                }
                return "";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

    }

}