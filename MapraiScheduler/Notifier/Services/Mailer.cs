using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using MapraiScheduler.Models.Database;

namespace MapraiScheduler.Notifier.Services
{
    public class Mailer
    {
        public MailAddress FromAddress { get; }
        public MailAddress ToAddress { get; }
        public string Password { get; } = NotifySetting.EmailStatics.FromEmailPassword;
        public string subject { set; get; }
        public string body { set; get; }

        public Mailer(string to)
        {
            FromAddress = new MailAddress(NotifySetting.EmailStatics.FromEmailAddress);
            ToAddress = new MailAddress(to);
        }
    }
}