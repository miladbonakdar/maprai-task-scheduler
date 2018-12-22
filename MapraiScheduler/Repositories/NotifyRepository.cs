using MapraiScheduler.Exception;
using MapraiScheduler.Models.Database;
using MapraiScheduler.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Repositories
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly MapRaiContex _mapRaiContex;

        public NotifyRepository(MapRaiContex mapRaiContex)
        {
            _mapRaiContex = mapRaiContex;
        }

        public async Task Insert(Notify notify)
        {
            try
            {
                _mapRaiContex.Notifiers.Add(notify);
                await _mapRaiContex.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "NotifyRepository Insert");
            }
        }

        public async Task AddRange(List<Notify> notifyRange)
        {
            try
            {
                _mapRaiContex.Notifiers.AddRange(notifyRange);
                await _mapRaiContex.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "NotifyRepository AddRange");
            }
        }

        //TESTED AND IT IS FINE
        public async Task<bool> ChechIfNotifyDoesNotExist(long invalidPhonePhoneId, int timeStampMiniute, long notifyType = 4/* for idle users */)
        {
            try
            {
                var lastNotify = await _mapRaiContex.Notifiers.Where(not =>
                    not.PhoneID != null && not.PhoneID == invalidPhonePhoneId && not.NotifyTypeID == notifyType &&
                    (DateTime.Now - not.CreationDate).TotalMinutes < timeStampMiniute).FirstOrDefaultAsync();
                return lastNotify == null;
            }
            catch (System.Exception e)
            {
                throw new AppDefaultException(e, "NotifyRepository ChechIfNotifyDoesNotExist");
            }
        }
    }
}