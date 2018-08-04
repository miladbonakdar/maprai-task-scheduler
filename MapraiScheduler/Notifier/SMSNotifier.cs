using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier.Services;

namespace MapraiScheduler.Notifier
{
    public class SmsNotifier : ISmsNotifier
    {
        private NotifyDTO _notify;
        private List<NotifyDTO> _Notifiers;

        public SmsNotifier()
        {
        }

        public INotifier CreateNotify(NotifyDTO notifyDto)
        {
            _notify = notifyDto;
            return this;
        }

        public void SendNotify()
        {
            SmsService.SendSms(_notify.PhoneNumber, SmsService.CreateNotifySmsTemplate(_notify, NotifySetting.SmsStatics.SmsNotifyTemplate));
        }

        public INotifier CreateNotifyRange(List<NotifyDTO> notifyDtoList)
        {
            _Notifiers = notifyDtoList;
            return this;
        }

        public void SendNotifyRange()
        {
            _notify = _Notifiers.FirstOrDefault();
            if (_notify == null)
                return;

            SmsService.SendSmsRange(_Notifiers.Select(item => item.PhoneNumber).ToList(),
                SmsService.CreateNotifySmsTemplate(_notify, NotifySetting.SmsStatics.SmsNotifyTemplate));
        }
    }
}