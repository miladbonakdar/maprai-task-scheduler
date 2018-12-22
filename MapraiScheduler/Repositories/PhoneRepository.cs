using MapraiScheduler.Models.Database;
using MapraiScheduler.Models.DTO;
using MapraiScheduler.Notifier;
using MD.PersianDateTime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Exception;
using MapraiScheduler.Repositories.Contracts;

namespace MapraiScheduler.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly INotifyRepository _notifyRepository;
        private readonly INotifyTypeRepository _notifyTypeRepository;
        private readonly MapRaiContex _mapRaiContex;

        public PhoneRepository(MapRaiContex mapRaiContex, INotifyTypeRepository notifyTypeRepository,
            INotifyRepository notifyRepository)
        {
            _notifyTypeRepository = notifyTypeRepository;
            _mapRaiContex = mapRaiContex;
            _notifyRepository = notifyRepository;
        }

        //TESTED AND IT IS FINE
        public async Task<List<NotifyDTO>> GetInvalidPhonesNotifies(List<long> phoneIds, string notifyUniqueName)
        {
            try
            {
                var notify = await _notifyTypeRepository.GetAsync(notifyUniqueName);
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
                        ProjectPhoneDetail = $"{item.Phone.FullName} - {item.Phone.PhoneNumber.ToString()}",
                        ProjectDetailUrl = "",
                        ProjectAdminDetail = "نا معلوم",
                        ProjectPhoneDetailUrl = NotifySetting.EmailStatics.PhoneBaseUrl + item.Phone.PhoneID.ToString(),
                        OrganizationID = item.Depo.OrganPositionID
                    }).Where(phone => phoneIds.Any(id => id == phone.PhoneID)).ToListAsync();
                notifyDtos.ForEach(item => item.MergeWithNotifyType(notify));
                ;
                notifyDtos.ForEach(item => item.PersianDateTime = (new PersianDateTime(DateTime.Now)).ToLongDateString());
                ;
                return notifyDtos;
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetInvalidPhonesNotifies");
            }
        }

        //TESTED AND IT IS FINE
        public async Task<List<Phone>> GetInvalidPhones(int threshHold)
        {
            try
            {
                var phones = await _mapRaiContex.Phones.Where(item => (DateTime.Now - item.LastActiveTime)
                                                          .TotalMinutes > threshHold).ToListAsync();
                var invalidPhones = new List<Phone>();
                foreach (var invalidPhone in phones)
                {
                    if (await _notifyRepository.ChechIfNotifyDoesNotExist(invalidPhone.PhoneID, threshHold))
                        invalidPhones.Add(invalidPhone);
                }

                return invalidPhones;
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "GetInvalidPhones");
            }
        }
    }
}