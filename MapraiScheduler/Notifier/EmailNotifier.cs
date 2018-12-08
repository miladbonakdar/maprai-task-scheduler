using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier.Services;
using System.Collections.Generic;

namespace MapraiScheduler.Notifier
{
    public class EmailNotifier : IEmailNotifier
    {
        private Mailer _mailer;
        private List<Mailer> _mailerRange;

        public EmailNotifier()
        {
        }

        public INotifier CreateNotify(NotifyDTO notifyDto)
        {
            _mailer = new Mailer(notifyDto.ToEmail)
            {
                body = EmailService.CreateMessageBody(new NotifyEmailTemplate
                {
                    EventDescription = notifyDto.EventDescription,
                    NotifyColor = notifyDto.NotifyColor,
                    PersianDateTime = notifyDto.PersianDateTime,
                    PriorityName = notifyDto.PriorityName,
                    ProjectAdminDetail = notifyDto.ProjectAdminDetail,
                    ProjectDetail = notifyDto.ProjectDetail,
                    ProjectDetailUrl = notifyDto.ProjectDetailUrl,
                    ProjectPhoneDetail = notifyDto.ProjectPhoneDetail,
                    ProjectPhoneDetailUrl = notifyDto.ProjectPhoneDetailUrl
                }),
                subject = notifyDto.EventDescription + " - " + notifyDto.PriorityName
            };
            return this;
        }

        public void SendNotify()
        {
            EmailService.SendEmail(_mailer);
        }

        public INotifier CreateNotifyRange(List<NotifyDTO> notifyDtoList)
        {
            _mailerRange = new List<Mailer>();
            foreach (var notifyDto in notifyDtoList)
            {
                _mailerRange.Add(new Mailer(notifyDto.ToEmail)
                {
                    body = EmailService.CreateMessageBody(new NotifyEmailTemplate
                    {
                        EventDescription = notifyDto.EventDescription,
                        NotifyColor = notifyDto.NotifyColor,
                        PersianDateTime = notifyDto.PersianDateTime,
                        PriorityName = notifyDto.PriorityName,
                        ProjectAdminDetail = notifyDto.ProjectAdminDetail,
                        ProjectDetail = notifyDto.ProjectDetail,
                        ProjectDetailUrl = notifyDto.ProjectDetailUrl,
                        ProjectPhoneDetail = notifyDto.ProjectPhoneDetail,
                        ProjectPhoneDetailUrl = notifyDto.ProjectPhoneDetailUrl
                    }),
                    subject = notifyDto.EventDescription + " - " + notifyDto.PriorityName
                });
            }
            return this;
        }

        public void SendNotifyRange()
        {
            foreach (var mailer in _mailerRange)
            {
                EmailService.SendEmail(mailer);
            }
        }
    }
}