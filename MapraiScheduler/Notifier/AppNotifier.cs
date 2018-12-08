using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Repositories;
using System;
using System.Collections.Generic;

namespace MapraiScheduler.Notifier
{
    public class AppNotifier : IAppNotifier
    {
        private Notify _notify;
        private List<Notify> _notifyRange;
        private readonly INotifyRepository _notifyRepository;

        public AppNotifier(INotifyRepository notifyRepository)
        {
            _notifyRepository = notifyRepository;
        }

        public INotifier CreateNotify(NotifyDTO notifyDto)
        {
            _notify = new Notify
            {
                CreationDate = DateTime.Now,
                NotifyTypeID = notifyDto.NotifyTypeID,
                PhoneID = notifyDto.PhoneID,
                Priority = notifyDto.Priority,
                ProjectID = notifyDto.PhoneID,
                Seen = false,
                UserID = notifyDto.UserID
            };
            return this;
        }

        public void SendNotify() => _notifyRepository.Insert(_notify);

        public INotifier CreateNotifyRange(List<NotifyDTO> notifyDtoList)
        {
            _notifyRange = new List<Notify>();
            foreach (var notifyDto in notifyDtoList)
            {
                _notifyRange.Add(new Notify
                {
                    CreationDate = DateTime.Now,
                    NotifyTypeID = notifyDto.NotifyTypeID,
                    PhoneID = notifyDto.PhoneID,
                    Priority = notifyDto.Priority,
                    ProjectID = notifyDto.ProjectID,
                    Seen = false,
                    UserID = notifyDto.UserID
                });
            }

            return this;
        }

        public void SendNotifyRange() => _notifyRepository.AddRange(_notifyRange);
    }
}