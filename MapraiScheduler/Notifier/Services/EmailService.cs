using MapraiScheduler.Models.DTO;
using System;
using System.Net;
using System.Net.Mail;

namespace MapraiScheduler.Notifier.Services
{
    public static class EmailService
    {
        //https://github.com/konsav/email-templates/
        public static string CreateMessageBody(NotifyEmailTemplate Dto, string rawTempalte)
        {
            var props = Dto.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                rawTempalte = rawTempalte.Replace("{" + propertyInfo.Name + "}", (string)propertyInfo.GetValue(Dto));
            }
            return rawTempalte;//but this is not raw any more :)
        }

        public static string CreateMessageBody(NotifyEmailTemplate Dto)
        {
            return CreateMessageBody(Dto, GetRawTemplate());
        }

        /// <summary>
        /// reads from file . html file
        /// </summary>
        /// <param name="notifyTypeUniqueName"></param>
        /// <returns></returns>
        public static string GetRawTemplate()
        {
            return NotifySetting.EmailStatics.EmailTemplate;
        }

        public static void SendEmail(Mailer mailer)
        {
            //https://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
            var smtp = new SmtpClient
            {
                Host = NotifySetting.EmailStatics.GoogleSmtpAddress,
                Port = NotifySetting.EmailStatics.GoogleSmtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailer.FromAddress.Address, mailer.Password)
            };
            try
            {
                using (var message = new MailMessage(mailer.FromAddress, mailer.ToAddress)
                {
                    Subject = mailer.subject,
                    Body = mailer.body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void SendEmailRange(Mailer mailer)
        {
            //https://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
            var smtp = new SmtpClient
            {
                Host = NotifySetting.EmailStatics.GoogleSmtpAddress,
                Port = NotifySetting.EmailStatics.GoogleSmtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailer.FromAddress.Address, mailer.Password)
            };
            try
            {
                using (var message = new MailMessage(mailer.FromAddress, mailer.ToAddress)
                {
                    Subject = mailer.subject,
                    Body = mailer.body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}