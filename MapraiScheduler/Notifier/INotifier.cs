using MapraiScheduler.Models.DTO;
using System.Collections.Generic;

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