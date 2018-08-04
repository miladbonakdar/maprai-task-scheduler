using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.DTO;

namespace MapraiScheduler.Notifier
{
    public interface INotifier
    {
        INotifier CreateNotify(NotifyDTO notifyDto);

        void SendNotify();

        INotifier CreateNotifyRange(List<NotifyDTO> notifyDtoList);

        void SendNotifyRange();
    }
}