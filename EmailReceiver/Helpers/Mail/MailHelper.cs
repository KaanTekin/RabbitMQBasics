using EmailReceiver.Helpers.Custom;
using EmailReceiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailReceiver.Helpers.Mail
{
    public class MailHelper
    {
        EmailModel EmailModel;

        public MailHelper(EmailModel emailModel)
        {
            EmailModel = emailModel;
        }

        public void SendMail()
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(EmailModel.From);
                List<string> tos = EmailModel.To.Split(';').ToList();

                tos.ForEach(to => message.To.Add(to));
                message.Subject = EmailModel.Subject;
                message.IsBodyHtml = EmailModel.IsBodyHtml; 
                message.Body = EmailModel.Body;
                smtp.Port = EmailModel.Port;
                smtp.Host = EmailModel.Host;
                smtp.EnableSsl = EmailModel.EnableSsl;
                smtp.UseDefaultCredentials = EmailModel.UseDefaultCredentials;
                smtp.Credentials = new NetworkCredential(EmailModel.From, EmailModel.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                CustomLogger.WriteLog(e.Message);
            }
        }
    }
}
