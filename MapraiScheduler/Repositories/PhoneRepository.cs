using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MD.PersianDateTime;
using Microsoft.EntityFrameworkCore;

namespace MapraiScheduler.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly INotifyRepository _notifyRepository;
        private readonly INotifyTypeRepository _notifyTypeRepository;
        private readonly MapRaiContex _mapRaiContex;

        public PhoneRepository(MapRaiContex mapRaiContex, INotifyTypeRepository notifyTypeRepository, INotifyRepository notifyRepository)
        {
            _notifyTypeRepository = notifyTypeRepository;
            _mapRaiContex = mapRaiContex;
            _notifyRepository = notifyRepository;
        }

        public async Task<List<NotifyDTO>> GetInvalidPhonesNotifies(List<long> phoneIds, string notifyUniqueName)
        {
            var notify = await _notifyTypeRepository.Get(notifyUniqueName);
            var notifyDtos = await _mapRaiContex.Phones.Join(_mapRaiContex.Depos, p => p.DepoID, d => d.DepoID,
                    (p, d) => new { Phone = p, Depo = d })
                .Select(item => new NotifyDTO
                {
                    EventDescription = "",
                    PriorityName = "",
                    NotifyID = 0,
                    NotifyTypeID = 0,
                    NotifyColor = "",
                    ProjectDetail = "نا مشخص",
                    ToEmail = "",
                    Priority = notify.Priority,
                    PhoneID = item.Phone.PhoneID,
                    Seen = 0,
                    UserID = 0,
                    PhoneNumber = item.Phone.PhoneNumber,
                    ProjectID = 0,
                    PersianDateTime = "",
                    ProjectPhoneDetail = item.Phone.FullName + " - " + item.Phone.PhoneNumber,
                    ProjectDetailUrl = "",
                    ProjectAdminDetail = "نا معلوم",
                    ProjectPhoneDetailUrl = NotifySetting.EmailStatics.PhoneBaseUrl + item.Phone.PhoneID.ToString(),
                    OrganizationID = item.Depo.OrganPositionID
                }).Where(phone => phoneIds.Any(id => id == phone.PhoneID)).ToListAsync();
            notifyDtos.ForEach(item => item.MergeWithNotifyType(notify)); ;
            notifyDtos.ForEach(item => item.PersianDateTime = (new PersianDateTime(DateTime.Now)).ToLongDateString()); ;
            return notifyDtos;
        }

        public async Task<List<Phone>> GetInvalidPhones(int threshHold)
        {
            var users = await _mapRaiContex.Phones.Where(item => (DateTime.Now - item.LastActiveTime)
                                                          .TotalMinutes > threshHold).ToListAsync();
            var invalidPhones = new List<Phone>();
            foreach (var invalidPhone in users)
            {
                if (await _notifyRepository.ChackIfIsLateNotify(invalidPhone.PhoneID, 7 * 24 * 60))
                    invalidPhones.Add(invalidPhone);
            }

            return invalidPhones;
        }
    }
}